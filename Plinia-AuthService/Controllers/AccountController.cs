﻿using System.Net;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
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
            { "access_token", _tokenService.GetAccessToken(user.Email) },
            { "id_token", _tokenService.GetIdToken(user) }
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
            { "access_token", _tokenService.GetAccessToken(user.Email) },
            { "id_token", _tokenService.GetIdToken(user) }
        });
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