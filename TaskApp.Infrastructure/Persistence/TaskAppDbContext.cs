using Microsoft.EntityFrameworkCore;
using TaskApp.Domain.Entities;

namespace TaskApp.Infrastructure.Persistence;

public class TaskAppDbContext : DbContext
{
    public TaskAppDbContext(DbContextOptions<TaskAppDbContext> options)
        : base(options) { }

    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskAppDbContext).Assembly);
    }
}