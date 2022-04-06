using Microsoft.EntityFrameworkCore;
using my_books1.Data.Models;

namespace my_books1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base (options)
        {
        }

        public DbSet<Book> Book { get; set; }
    }
}
