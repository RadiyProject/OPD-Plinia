using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Server.DBContexts;
using System;
using System.Collections.Generic;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        /*private readonly IConfiguration _configuration;
        public ServerController(IConfiguration config)
        {
            _configuration = config;
        }*/

        string[] objs = { "1", "3", "2", "4", "6" };
        // GET: api/<SpeechSynthesisController>
        [HttpGet]
        public ActionResult Get()
        {
            ServerContext context = HttpContext.RequestServices.GetService(typeof(ServerContext)) as ServerContext;
            var result = context.GetAllNumbers();
            if (result == null)
                return NoContent();
            else
                return Ok(result);
        }

        // GET api/<SpeechSynthesisController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            ServerContext context = HttpContext.RequestServices.GetService(typeof(ServerContext)) as ServerContext;
            var result = context.GetNumber(id);
            if (result == null)
                return NoContent();
            else
                return Ok(result);
        }

        // POST api/<SpeechSynthesisController>
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            ServerContext context = HttpContext.RequestServices.GetService(typeof(ServerContext)) as ServerContext;
            int content; 
            Int32.TryParse(value, out content);
            var result = context.AddNumber(content);
            if (result == null)
                return BadRequest();
            else
                return Ok(result);
        }

        // PUT api/<SpeechSynthesisController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            ServerContext context = HttpContext.RequestServices.GetService(typeof(ServerContext)) as ServerContext;
            int content;
            Int32.TryParse(value, out content);
            var result = context.ChangeNumber(id, content);
            if (result == null)
                return BadRequest();
            else
                return Ok(result);
        }

        // DELETE api/<SpeechSynthesisController>/5
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
