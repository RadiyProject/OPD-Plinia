using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using plinia.dbcontexts;
using plinia.Models;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json;
using plinia.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace plinia.Controllers
{
    [Route("api/security")]
    [ApiController]
    public class AutorizationController : ControllerBase
    {
        IConfiguration configuration { get; }

        public AutorizationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("activate/{uuid}")]
        public async Task<IActionResult> Activate(string uuid)
        {
            using (UsersContexts db = new UsersContexts())
            {
                var user = await db.Users.Where(x => x.uuid == uuid).FirstOrDefaultAsync();

                if (user == null)
                    return BadRequest("Link is incorrect or user wasn`t found.");

                if (user.mailConfirmed)
                    return Ok("Mail was confirmed.");

                user.mailConfirmed = true;

                await db.SaveChangesAsync();

                return Ok("Mail confirmed successfully!");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
        {
            using (UsersContexts db = new UsersContexts())
            {
                var user = await db.Users.Where(x => x.email == email).FirstOrDefaultAsync();

                if (user == null)
                    return BadRequest(SetResponse("Data is incorrect: ", "", ""));

                var hasher = new PasswordHasher<User>();

                var isCurrentHashValid = hasher.VerifyHashedPassword(user, user.password, password);

                if (isCurrentHashValid != PasswordVerificationResult.Success)
                    return BadRequest(SetResponse("Data is incorrect: ", "", ""));

                var token = new TokenService(configuration);
                await token.GenerateToken(user);

                var message = JsonSerializer.Serialize(SetResponse("User logged is successfully!", token.accessToken, token.refreshToken));
                return Ok(message);
            }
        }

        [HttpPost("register/")]
        public async Task<IActionResult> Register([FromForm] string name, [FromForm] string email, [FromForm] string password)
        {
            using (UsersContexts db = new UsersContexts());
            {
                var uuid = Guid.NewGuid();

                var user = new User
                {
                    name = name,
                    email = email,
                    premium = configuration.GetSection("DefaultParams")["Premium"],
                    uuid = uuid.ToString()
                };

                var hasher = new PasswordHasher<User>();

                var hash = hasher.HashPassword(user, password);

                user.password = hash;

                using (UsersContexts db = new UsersContexts())
                {
                    db.Users.Add(user);
                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch
                    {
                        return BadRequest(SetResponse("User with this email already exist.", "", ""));
                    }
                }

                    

                var host = configuration.GetSection("Mail")["Host"];

                var activationLink = host + "/api/user/activate" + uuid.ToString();

                try
                {
                    await new MailService(configuration).SendMessage(email, "Mail confirmation " + host, activationLink);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                }

                var token = new TokenService(configuration);

                await token.GenerateToken(user);

                var message = JsonSerializer.Serialize(SetResponse("User registered successfully!", token.accessToken, token.refreshToken));

                return Ok(message);
            }
        }
            
        [HttpGet("logout/{refreshToken}")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            if (refreshToken == null)
                return BadRequest("Token wasn`t sent.");
            else return Ok();
        }


        private object? SetResponse(string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }





        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("/register")]
        public IActionResult Post([FromForm] string name, [FromForm] string email, [FromForm] string password)
        {
            using (UsersContexts db = new UsersContexts())
            {
                var users = db.Users.ToList();
                foreach (User u in users)
                    if (u.name == name)
                        return BadRequest("Пользователь с таким именем уже существует.");

                User user = new User
                {
                    name = name,
                    email = email,//добавить проверку на существование почты
                    password = password
                };

                db.Users.Add(user);
                db.SaveChanges();
            }
            return Ok("Пользователь успешно зарегистрирован!");
        }

        IActionResult BadRequestResult(string v)
        {
            throw new NotImplementedException();
        }

        [Route("api/user")]
        [ApiController]
        public class UserController : ControllerBase
        {
            string[] objs = { "1", "3", "2", "4", "6" };
            private object tokenData;

            // GET: api/<UserController>
            [HttpGet("getall")]
            public ActionResult Get()
            {
                UsersContexts context = HttpContext.RequestServices.GetService(typeof(UsersContexts)) as UsersContexts;
                using (UsersContexts db = new UsersContexts())
                {
                    var result = db.Users.ToList();
                    if (result == null)
                        return NoContent();
                    else
                        return Ok(result);
                }

            }

            // GET api/<UserController>/5
            [HttpGet("{id}")]
            public ActionResult Get(int id)
            {
                UsersContexts context = HttpContext.RequestServices.GetService(typeof(UsersContexts)) as UsersContexts;
                using (UsersContexts db = new UsersContexts())
                {
                    var result = db.Users.Where(x => x.id == id).FirstOrDefaultAsync();
                    if (result == null)
                        return NoContent();
                    else
                        return Ok(result);
                }

            }

            // POST api/<UserController>
            // add user
            [HttpPost]
            public ActionResult Post([FromForm] IFormCollection data)
            {
                using (UsersContexts db = new UsersContexts())
                {
                    var user = new User()
                    {
                        name = data["name"],
                        password = data["password"],
                        email = data["email"],
                    };

                    db.Users.Add(user);
                    try
                    {
                         db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        return BadRequest("ok");
                    }
                    return Ok();
                }

                    
            }
            



            // PUT api/<UserController>/5
            [HttpPut("{id}")]
            public ActionResult Put(int id, [FromForm] IFormCollection data)
            {
                UsersContexts context = HttpContext.RequestServices.GetService(typeof(UsersContexts)) as UsersContexts;
                var result = context.ChangeUser(id, data["name"], data["password"]);
                if (result == null)
                    return BadRequest();
                else
                    return Ok(result);
            }

            // DELETE api/<UserController>/5
            [HttpDelete("{id}")]
            public async ActionResult Delete(int id)
            {
                UsersContexts context = HttpContext.RequestServices.GetService(typeof(UsersContexts)) as UsersContexts;
                using (UsersContexts db = new UsersContexts())
                {
                    var result = db.Tokens.Remove(tokenData);
                    await db.SaveChangesAsync(); ;
                    if (result == false)
                        return BadRequest();
                    else
                        return Ok(result);
                }
                    
            }
        }   
    }   
}