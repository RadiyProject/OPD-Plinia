using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plinia_AuthService.Models;
using Plinia_AuthService.Services;

namespace Plinia_AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    private readonly SignInManager<IdentityUser> _signInManager;

    private readonly TokenService _tokenService;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] Credentials credentials)
    {
        if (!ModelState.IsValid) return Error("Unexpected error!");

        var user = new IdentityUser { UserName = credentials.Username, Email = credentials.Email };
        var result = await _userManager.CreateAsync(user, credentials.Password);
        if (!result.Succeeded) return Errors(result);
        await _signInManager.SignInAsync(user, isPersistent: false);
        return new JsonResult(new Dictionary<string, object>
        {
            { "access_token", _tokenService.GetAccessToken(user.Email) },
            { "id_token", _tokenService.GetIdToken(user) }
        });
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> SignIn([FromBody] Credentials credentials)
    {
        if (!ModelState.IsValid) return Error("Unexpected error!");
        var result = await _signInManager.PasswordSignInAsync(credentials.Username, credentials.Password,
            isPersistent: false,
            lockoutOnFailure: false);
        if (!result.Succeeded)
            return new UnauthorizedObjectResult("Unable to sign in");

        var user = await _userManager.FindByEmailAsync(credentials.Email);
        return new JsonResult(new Dictionary<string, object>
        {
            { "access_token", _tokenService.GetAccessToken(user.Email) },
            { "id_token", _tokenService.GetIdToken(user) }
        });
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Logout(AuthenticationProperties properties)
    {
        if (!ModelState.IsValid) return Error("Unexpected error!");
        await _signInManager.SignOutAsync();
        return new SignOutResult(properties);
    }

    private static IActionResult Error(string message)
    {
        return new BadRequestObjectResult(message);
    }

    private static IActionResult Errors(IdentityResult result)
    {
        var items = result.Errors
            .Select(x => x.Description)
            .ToArray();
        return new BadRequestObjectResult(items);
    }
}