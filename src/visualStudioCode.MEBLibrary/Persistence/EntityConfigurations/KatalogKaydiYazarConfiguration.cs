using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class KatalogKaydiYazarConfiguration : IEntityTypeConfiguration<KatalogKaydiYazar>
{
    public void Configure(EntityTypeBuilder<KatalogKaydiYazar> builder)
    {
        builder.ToTable("KatalogKaydiYazarlar").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.KatalogKaydiId).HasColumnName("KatalogKaydiId").IsRequired();
        builder.Property(e => e.YazarId).HasColumnName("YazarId").IsRequired();
        builder.Property(e => e.OtoriteKaydiId).HasColumnName("OtoriteKaydiId");
        builder.Property(e => e.Rol).HasColumnName("Rol").IsRequired();
        builder.Property(e => e.Sira).HasColumnName("Sira").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.KatalogKaydi).WithMany(k => k.KatalogYazarlar).HasForeignKey(e => e.KatalogKaydiId);
        builder.HasOne(e => e.Yazar).WithMany().HasForeignKey(e => e.YazarId);
        builder.HasOne(e => e.OtoriteKaydi).WithMany(o => o.IlgiliYazarKayitlari).HasForeignKey(e => e.OtoriteKaydiId);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
