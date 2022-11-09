using System.Net;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Plinia_AuthService.Models;
using Plinia_AuthService.Secure;

namespace Plinia_AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    private readonly SignInManager<IdentityUser> _signInManager;

    private readonly JwtSettings _settings;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IOptions<JwtSettings> options)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _settings = options.Value;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] Credentials credentials)
    {
        if (!ModelState.IsValid) return Error("Unexpected error!");

        var user = new IdentityUser { UserName = credentials.Username, Email = credentials.Email };
        var result = await _userManager.CreateAsync(user, credentials.Password);
        if (!result.Succeeded) return Errors(result);
        await _signInManager.SignInAsync(user, isPersistent: false);
        return new JsonResult(new Dictionary<string, object>
        {
            { "access_token", GetAccessToken(user.Email) },
            { "id_token", GetIdToken(user) }
        });
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn([FromBody] Credentials credentials)
    {
        if (!ModelState.IsValid) return Error("Unexpected error!");
        var result = await _signInManager.PasswordSignInAsync(credentials.Username, credentials.Password,
            isPersistent: false,
            lockoutOnFailure: false);
        if (!result.Succeeded)
            return new JsonResult("Unable to sign in") { StatusCode = (int)HttpStatusCode.Unauthorized };
        
        var user = await _userManager.FindByEmailAsync(credentials.Email);
        return new JsonResult(new Dictionary<string, object>
        {
            { "access_token", GetAccessToken(user.Email) },
            { "id_token", GetIdToken(user) }
        });
    }

    private string GetIdToken(IdentityUser user)
    {
        var payload = new Dictionary<string, object>
        {
            { "id", user.Id },
            { "sub", user.Email },
            { "email", user.Email },
            { "emailConfirmed", user.EmailConfirmed },
        };
        return GenerateToken(payload);
    }

    private string GetAccessToken(string email)
    {
        var payload = new Dictionary<string, object>
        {
            { "sub", email },
            { "email", email }
        };
        return GenerateToken(payload);
    }

    private string GenerateToken(Dictionary<string, object> payload)
    {
        var secretKey = _settings.SecretKey;

        payload.Add("iss", _settings.Issuer);
        payload.Add("aud", _settings.Audience);
        payload.Add("nbf", ConvertToUnixTimestamp(DateTime.Now));
        payload.Add("iat", ConvertToUnixTimestamp(DateTime.Now));
        payload.Add("exp", ConvertToUnixTimestamp(DateTime.Now.AddDays(7)));

        IJwtAlgorithm algorithm = new NoneAlgorithm();
        IJsonSerializer serializer = new JsonNetSerializer();
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

        return encoder.Encode(payload, secretKey);
    }

    private object ConvertToUnixTimestamp(DateTime dateTime)
    {
        var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var diff = dateTime.ToUniversalTime() - origin;
        return Math.Floor(diff.TotalSeconds);
    }

    private static IActionResult Error(string message)
    {
        return new JsonResult(message) { StatusCode = (int)HttpStatusCode.BadRequest };
    }

    private static JsonResult Errors(IdentityResult result)
    {
        var items = result.Errors
            .Select(x => x.Description)
            .ToArray();
        return new JsonResult(items) { StatusCode = (int)HttpStatusCode.BadRequest };
    }
}