using Api3Assignment.Models;
using Microsoft.EntityFrameworkCore;

namespace Api3Assignment.Data
{
    public class DemoDbContext : DbContext
    {


        public DemoDbContext(DbContextOptions<DemoDbContext> options) :
          base(options)
        {
        
        }

        public DbSet<Book> BookDetails { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>().HasIndex(Book => Book.Id).IsUnique();

        }
    }
}
