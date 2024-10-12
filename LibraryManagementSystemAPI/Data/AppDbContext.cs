using LibraryManagementSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystemAPI.Data
{
    /*
     AppDbContext class inherits DbContext class from Microsoft.EntityFrameworkCore. Coordinates EF functionality for the Book Model 
     and other future models which can be introduced.
     */
    #pragma warning disable CS1591
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Book> Books { get; set; } = null!;
    }
    #pragma warning restore CS1591
}
