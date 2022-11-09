using Microsoft.AspNetCore.Mvc;
using Plinia_AuthService.DB;
using Plinia_AuthService.Entities;

namespace Plinia_AuthService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : Controller
{
    private readonly MySqlDbContext _db;


    public UserController(MySqlDbContext context)
    {
        _db = context;
    }

    [HttpGet("Users")]
    public IEnumerable<User> GetUsers()
    {
        return _db.Users.ToList();
    }
    
    [HttpPost("Users")]
    public User SignIn()
}