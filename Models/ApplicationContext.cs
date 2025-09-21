using Microsoft.EntityFrameworkCore;

namespace TheBooksNook.Models;

public class ApplicationContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Book> Books { get; set; }

    public DbSet<Comment> Comments { get; set; }
}
