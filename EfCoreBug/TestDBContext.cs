using Microsoft.EntityFrameworkCore;

namespace EfCoreBug;

public class TestDBContext : DbContext
{
    public DbSet<TestEntity> TestEntities => Set<TestEntity>();

    public TestDBContext(
        DbContextOptions<TestDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder
            .Entity<TestEntity>()
            .HasNoKey();
    }
}
