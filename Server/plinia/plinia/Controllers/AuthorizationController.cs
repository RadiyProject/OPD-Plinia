using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using plinia.dbcontexts;
using System;
using System.Collections.Generic;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace plinia.Controllers
{
    [HttpPost("/register")]
    public IActionResult Post([FromForm] string name, [FromForm] string email, [FromForm] string password)
    {

    }

    [Route("api/[controller]")]
    [ApiController]
    public class Controller : ControllerBase
    {
        // GET: api/<Controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Controller>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
