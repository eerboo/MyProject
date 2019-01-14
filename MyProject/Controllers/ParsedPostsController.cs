using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MyProject.DAL;
using MyProject.Models;
using MyProject.Services;

namespace MyProject.Controllers
{
    public class ParsedPostsController : ApiController
    {
        private ParseContext db = new ParseContext();

        // GET: api/ParsedPosts
        public IQueryable<ParsedPost> GetParsedPosts()
        {
            return db.ParsedPosts;
        }

        // GET: api/ParsedPosts/5
        [ResponseType(typeof(ParsedPost))]
        public IHttpActionResult GetParsedPost(int id)
        {
            ParsedPost parsedPost = db.ParsedPosts.Find(id);
            if (parsedPost == null)
            {
                return NotFound();
            }

            return Json(parsedPost);
        }
        [Route("api/posts")]
        public IQueryable<ParsedPost> Get(string from, string to)
        {
            DateTime start = DateTime.Parse(from);
            DateTime end = DateTime.Parse(to);
            return db.ParsedPosts.Where(x => (x.PostDate > start && x.PostDate < end));
        }
        [Route("api/search/")]
        public IQueryable<ParsedPost> Get(string text)
        {

            return db.ParsedPosts.Where(x => x.PostBody.Contains(text));
        }
        [Route("api/topten/")]
        public List<string> Get()
        {
            PostsService postService = new PostsService();
            var result = postService.GetTopTen();

            return result;
        }

        // PUT: api/ParsedPosts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutParsedPost(int id, ParsedPost parsedPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != parsedPost.ID)
            {
                return BadRequest();
            }

            db.Entry(parsedPost).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParsedPostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ParsedPosts
        [ResponseType(typeof(ParsedPost))]
        public IHttpActionResult PostParsedPost(ParsedPost parsedPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ParsedPosts.Add(parsedPost);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = parsedPost.ID }, parsedPost);
        }

        // DELETE: api/ParsedPosts/5
        [ResponseType(typeof(ParsedPost))]
        public IHttpActionResult DeleteParsedPost(int id)
        {
            ParsedPost parsedPost = db.ParsedPosts.Find(id);
            if (parsedPost == null)
            {
                return NotFound();
            }

            db.ParsedPosts.Remove(parsedPost);
            db.SaveChanges();

            return Ok(parsedPost);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParsedPostExists(int id)
        {
            return db.ParsedPosts.Count(e => e.ID == id) > 0;
        }
    }
}