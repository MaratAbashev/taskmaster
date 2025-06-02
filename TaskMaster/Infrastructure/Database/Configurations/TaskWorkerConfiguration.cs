using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class TaskWorkerConfiguration: IEntityTypeConfiguration<TaskWorker>
{
    public void Configure(EntityTypeBuilder<TaskWorker> builder)
    {
        builder.HasKey(tw => tw.Id);
        builder.HasOne(tw => tw.Task)
            .WithMany(t => t.TaskWorkers)
            .HasForeignKey(tw => tw.TaskId);
        builder.Property(tw => tw.IsConfirmed)
            .HasDefaultValue(false);
        builder.HasOne(tw => tw.User)
            .WithMany(u => u.TaskWorkers)
            .HasForeignKey(tw => tw.UserId);
        builder.Property(tw => tw.Id)
            .ValueGeneratedOnAdd();
    }
}