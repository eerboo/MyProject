using MyProject.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MyProject.DAL
{
    public class ParseContext : DbContext
    {

        public ParseContext() : base("MyContext")
        {
        }

        public DbSet<ParsedPost> ParsedPosts { get; set; }
        public DbSet<User> Users { get; set; }      

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();           
        }

      

    

        
    }
}