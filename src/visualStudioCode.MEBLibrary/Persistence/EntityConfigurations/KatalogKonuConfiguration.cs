using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class KatalogKonuConfiguration : IEntityTypeConfiguration<KatalogKonu>
{
    public void Configure(EntityTypeBuilder<KatalogKonu> builder)
    {
        builder.ToTable("KatalogKonular").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.KatalogKaydiId).HasColumnName("KatalogKaydiId").IsRequired();
        builder.Property(e => e.KonuBasligi).HasColumnName("KonuBasligi").IsRequired();
        builder.Property(e => e.OtoriteKaydiId).HasColumnName("OtoriteKaydiId");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.KatalogKaydi).WithMany(k => k.Konular).HasForeignKey(e => e.KatalogKaydiId);
        builder.HasOne(e => e.OtoriteKaydi).WithMany(o => o.KatalogKonulari).HasForeignKey(e => e.OtoriteKaydiId);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}