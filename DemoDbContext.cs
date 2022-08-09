using BookSellingAssignment.Models;  
using Microsoft.EntityFrameworkCore;  
  
namespace BookSellingAssignment.DatabaseConnection
{
    public class DemoDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
     



        public DemoDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"DESKTOP-P3O2VMI\SQLEXPRESS;Database=BooksDb;Trusted_Connection=True;");
        }
}