using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api")]
    [ApiController]
    public class SpeechSynthesisController : ControllerBase
    {
        string[] objs = { "1", "3", "2", "4", "6" };
        // GET: api/<SpeechSynthesisController>
        [HttpGet("getall")]
        public ActionResult Get()
        {
            if (objs != null && objs.Length > 0)
            {
                string temp = "";
                foreach (string str in objs)
                    temp += str + "\n";
                return Ok("Всё отлично" + "\n" + temp);
            }
            return NoContent();
        }

        // GET api/<SpeechSynthesisController>/5
        [HttpGet("get/{id}")]
        public ActionResult Get(int id)
        {
            if (objs != null && objs.Length > 0 && objs.Length > id)
            {
                return Ok("Всё отлично" + "\n" + objs[id]);
            }
            else if (objs.Length <= id)
                return NotFound();
            else
                return NoContent();
        }

        // POST api/<SpeechSynthesisController>
        [HttpPost("send")]
        public ActionResult Post([FromBody] string value)
        {
            if (value != null && value != "")
            {
                string[] newObjs = new string[objs.Length + 1];
                for (int i = 0; i < objs.Length; i++)
                    newObjs[i] = objs[i];
                newObjs[objs.Length] = value;
                objs = newObjs;
                string temp = "";
                foreach (string str in objs)
                    temp += str + "\n";
                return Ok(temp);
            }
            return BadRequest();//поправить
        }

        // PUT api/<SpeechSynthesisController>/5
        [HttpPut("change/{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            if (value != null && value != "" && objs != null && objs.Length > 0 && objs.Length > id)
            {
                objs[id] = value;
                string temp = "";
                foreach (string str in objs)
                    temp += str + "\n";
                return Ok(temp);
            }
            else
                return BadRequest();//поправить
        }

        // DELETE api/<SpeechSynthesisController>/5
        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (objs != null && objs.Length > 0 && objs.Length > id)
            {
                string[] newObjs = new string[objs.Length - 1];
                int j = 0;
                for (int i = 0; i < objs.Length; i++)
                    if (i != id)
                    {
                        newObjs[j] = objs[i];
                        j++;
                    }
                objs = newObjs;
                string temp = "";
                foreach (string str in objs)
                    temp += str + "\n";
                return Ok(temp);
            }
            else
                return BadRequest();//поправить
        }
    }
}
