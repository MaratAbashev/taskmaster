using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class ToDoTaskConfiguration: IEntityTypeConfiguration<ToDoTask>
{
    public void Configure(EntityTypeBuilder<ToDoTask> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title)
            .IsRequired();
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();
        builder.HasOne(t => t.Board)
            .WithMany(tb => tb.BoardTasks)
            .HasForeignKey(t => t.BoardId);
        builder.HasOne(t => t.Author)
            .WithMany(a => a.AuthoredTasks)
            .HasForeignKey(t => t.AuthorId);
    }
}