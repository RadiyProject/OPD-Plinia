using Microsoft.AspNetCore.Mvc;
using NewServer.DBContext;
using NewServer.Models;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        // GET: api/<AuthorizationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuthorizationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("/login")]
        public IActionResult Post([FromForm] string name, [FromForm] string password)
        {
            using (UsersContext db = new UsersContext())
            {
                var users = db.Users.ToList();
                foreach (User u in users)
                    if (u.name == name && u.password == password)
                        return Ok("Вы успешно авторизовались!");
            }
            return BadRequest("Данные введены неверно!");
        }

        // POST api/register
        [HttpPost("/register")]
        public IActionResult Post([FromForm] string name, [FromForm] string email, [FromForm] string password)
        {
            using (UsersContext db = new UsersContext())
            {
                var users = db.Users.ToList();
                foreach (User u in users)
                    if (u.name == name)
                        return BadRequest("Пользователь с таким именем уже существует.");

                User user = new User
                {
                    name = name,
                    email = email,
                    password = password
                };

                db.Users.Add(user);
                db.SaveChanges();
            }
            return Ok("Пользователь успешно зарегистрирован!");
        }

        // PUT api/<AuthorizationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthorizationController>/5
        [HttpDelete("/delete")]
        public IActionResult Delete([FromForm] string name, [FromForm] string password)
        {
            using (UsersContext db = new UsersContext())
            {
                var users = db.Users.ToList();
                foreach (User u in users)
                {
                    if (u.name == name && u.password == password)
                    {
                        db.Users.Remove(u);
                        db.SaveChanges();
                        return Ok("Пользователь успешно удален!");
                    }
                }
            }
            return BadRequest("Пользователя с таким именем не существует или неверно введен пароль.");
        }
    }
}
