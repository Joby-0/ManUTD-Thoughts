using DbModels;
using Microsoft.EntityFrameworkCore;

namespace DbContext;

public class MainDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}