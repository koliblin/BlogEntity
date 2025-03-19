using BlogEntity;
using Microsoft.EntityFrameworkCore;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Используем SQLite
        optionsBuilder.UseSqlite("Data Source=app.db"); // Файл базы данных
    }
}
