using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class TelegramGroupConfiguration: IEntityTypeConfiguration<TelegramGroup>
{
    public void Configure(EntityTypeBuilder<TelegramGroup> builder)
    {
        builder.HasKey(tg => tg.Id);
        builder.HasOne(tg => tg.Board)
            .WithOne(tb => tb.Group)
            .HasForeignKey<TelegramGroup>(tg => tg.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}