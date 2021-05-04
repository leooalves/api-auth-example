using Microsoft.EntityFrameworkCore;
using api_auth_example.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
}