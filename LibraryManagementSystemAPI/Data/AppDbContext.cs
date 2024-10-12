using LibraryManagementSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystemAPI.Data
{
    /*
     AppDbContext class inherits DbContext class from Microsoft.EntityFrameworkCore. Coordinates EF functionality for the Book Model 
     and other future models which can be introduced.
     */
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Book> Books { get; set; } = null!;
    }
}
