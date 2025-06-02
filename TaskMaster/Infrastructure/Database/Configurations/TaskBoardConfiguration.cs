using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class TaskBoardConfiguration: IEntityTypeConfiguration<TaskBoard>
{
    public void Configure(EntityTypeBuilder<TaskBoard> builder)
    {
        builder.HasKey(tb => tb.Id);
        builder.HasMany(tb => tb.BoardUsers)
            .WithOne(tb => tb.Board)
            .HasForeignKey(tb => tb.BoardId);
        builder.Property(tb => tb.Title)
            .IsRequired();
        
    }
}