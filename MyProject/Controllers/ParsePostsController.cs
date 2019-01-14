using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyProject.Services;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    public class ParsePostsController : ApiController
    {
        ParserService parse = new ParserService();
        // GET api/ParsePosts
        //[ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Get()
        {            
           string result = await parse.Parse();
            return Ok(result);
        }

        // GET api/<controller>/5
        public async Task<IHttpActionResult> Get(int id)
        {
            string result = await parse.Parse(id);
            return Ok(result);
        }

        
    }
}