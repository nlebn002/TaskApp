using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskApp.Domain.Entities;

namespace TaskApp.Infrastructure.Persistence.Configurations;

public class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.DueDate);

        builder.Property(t => t.Status)
            .HasConversion<string>() // Store enum as string
            .IsRequired();
    }
}