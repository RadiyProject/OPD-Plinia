using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Server.DBContexts;
using System;
using System.Collections.Generic;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        string[] objs = { "1", "3", "2", "4", "6" };
        // GET: api/<UserController>
        [HttpGet("getall")]
        public ActionResult Get()
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
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
            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
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
            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
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
            UserContext context = HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
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
            ServerContext context = HttpContext.RequestServices.GetService(typeof(ServerContext)) as ServerContext;
            var result = context.DeleteNumber(id);
            if (result == false)
                return BadRequest();
            else
                return Ok(result);
        }
    }
}
