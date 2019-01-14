using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MyProject.Models;

namespace MyProject.DAL
{
    public class ParseInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ParseContext>
    {
        protected override void Seed(ParseContext context)
        {
           //Проверка на работоспособность бд
          /*  var parsedPost = new List<ParsedPost>
            {
                new ParsedPost { ID = 0, ParseDate = null, PostBody = "", PostDate = null, Topic = ""}
            };
            parsedPost.ForEach(s => context.ParsedPosts.Add(s));
            context.SaveChanges();
            */
        }
    }
}