using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class RafConfiguration : IEntityTypeConfiguration<Raf>
{
    public void Configure(EntityTypeBuilder<Raf> builder)
    {
        builder.ToTable("Raflar").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.KutuphaneBolumuId).HasColumnName("KutuphaneBolumuId").IsRequired();
        builder.Property(e => e.Kod).HasColumnName("Kod").IsRequired();
        builder.Property(e => e.Aciklama).HasColumnName("Aciklama");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.Bolum).WithMany(k => k.Raflar).HasForeignKey(e => e.KutuphaneBolumuId);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
