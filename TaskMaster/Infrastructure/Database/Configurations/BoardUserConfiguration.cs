using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class BoardUserConfiguration: IEntityTypeConfiguration<BoardUser>
{
    public void Configure(EntityTypeBuilder<BoardUser> builder)
    {
        builder.HasKey(bu => bu.Id);
        builder.HasOne(bu => bu.User)
            .WithMany(u => u.BoardUsers)
            .HasForeignKey(bu => bu.UserId);
        builder.Property(bu => bu.Id)
            .ValueGeneratedOnAdd();
    }
}