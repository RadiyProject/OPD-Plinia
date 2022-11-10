using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using plinia.dbcontexts;
using plinia.Models;
using System;
using System.Collections.Generic;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace plinia.Controllers
{

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
        // GET: api/<UserController>
        [HttpGet("getall")]
        public ActionResult Get()
        {
            UsersContexts context = HttpContext.RequestServices.GetService(typeof(UsersContexts)) as UsersContexts;
            var result = context.GetAllUsers();
            if (result == null)
                return NoContent();
            else
                return Ok(result);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            UsersContexts context = HttpContext.RequestServices.GetService(typeof(UsersContexts)) as UsersContexts;
            var result = context.GetUser(id);
            if (result == null)
                return NoContent();
            else
                return Ok(result);
        }

        // POST api/<UserController>
        [HttpPost]
        public ActionResult Post([FromForm] IFormCollection data)
        {
            UsersContexts context = HttpContext.RequestServices.GetService(typeof(UsersContexts)) as UsersContexts;
            var result = context.AddUser(data["name"], data["password"]);
            if (result == null)
                return BadRequest();
            else
                return Ok(result);
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
        public ActionResult Delete(int id)
        {
            UsersContexts context = HttpContext.RequestServices.GetService(typeof(UsersContexts)) as UsersContexts;
            var result = context.DeleteNumber(id);
            if (result == false)
                return BadRequest();
            else
                return Ok(result);
        }
    }
}