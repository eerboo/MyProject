using MyProject.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MyProject.DAL
{
    public class ParseContext : DbContext
    {

        public ParseContext() : base("SchoolContext")
        {
        }

        public DbSet<ParsedPost> ParsedPosts { get; set; }
      //  public DbSet<WebSite> WebSites { get; set; }        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}