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

            var user = new User() { Username = "admin", Password = "pswd", IsActive = true, UserId = 1 };
            
            context.Users.Add(user);
            context.SaveChanges();
            
        }
    }
}