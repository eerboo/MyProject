using MyProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MyProject.Services
{
    public class PostsService
    {
        ParseContext context = new ParseContext();

        public string clean(string s)
        {
            StringBuilder sb = new StringBuilder(s);

            sb.Replace(" в ", " ");
            sb.Replace(" на ", " ");
            sb.Replace(" у ", " ");
            sb.Replace(" за ", " ");
            sb.Replace(" — ", " ");
            sb.Replace(" с ", " ");
            sb.Replace(" и ", " ");
            sb.Replace(" что ", " ");
            sb.Replace(" не ", " ");
            sb.Replace(" для ", " ");
            sb.Replace(" по ", " ");
            sb.Replace(" из ", " ");
            sb.Replace(" то ", " ");
            sb.Replace(" но ", " ");
            sb.Replace(" к ", " ");
            sb.Replace(" о ", " ");
            sb.Replace("/", " ");
            sb.Replace("\n", "");
            sb.Replace("\r", "");
            sb.Replace("&", " ");
            sb.Replace(",", "");
            sb.Replace("  ", " ");
            sb.Replace(" ", " ");
            sb.Replace("'", "");
            sb.Replace(".", "");
            sb.Replace("!;", "");

            return sb.ToString().ToLower();
        }

        public List<string> GetTopTen()
        {
            var posts = context.ParsedPosts.Select(x => x.PostBody);
            List<string> words = new List<string>();
            
            foreach (var post in posts)
            {
               string cleanPost = clean(post);
                words.AddRange((cleanPost.Split(' ', '!', '.', ',', '-')));
            }
           var result =  words.Where(x => x!="").GroupBy(w => w).Select(r => new { Word = r.First(), Count = r.Count() }).OrderByDescending(c => c.Count).Take(10).Select(a => a.Word);

            return result.ToList();
             
        }
    }
}