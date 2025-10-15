using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class YeniKatalogTalebiConfiguration : IEntityTypeConfiguration<YeniKatalogTalebi>
{
    public void Configure(EntityTypeBuilder<YeniKatalogTalebi> builder)
    {
        builder.ToTable("YeniKatalogTalepleri").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.TalepEdenKutuphaneId).HasColumnName("TalepEdenKutuphaneId").IsRequired();
        builder.Property(e => e.Baslik).HasColumnName("Baslik").IsRequired();
        builder.Property(e => e.AltBaslik).HasColumnName("AltBaslik");
        builder.Property(e => e.YazarMetni).HasColumnName("YazarMetni");
        builder.Property(e => e.Isbn).HasColumnName("Isbn");
        builder.Property(e => e.Issn).HasColumnName("Issn");
        builder.Property(e => e.MateryalTuru).HasColumnName("MateryalTuru");
        builder.Property(e => e.MateryalAltTuru).HasColumnName("MateryalAltTuru");
        builder.Property(e => e.Dil).HasColumnName("Dil");
        builder.Property(e => e.Yayinevi).HasColumnName("Yayinevi");
        builder.Property(e => e.YayinYeri).HasColumnName("YayinYeri");
        builder.Property(e => e.YayinYili).HasColumnName("YayinYili");
        builder.Property(e => e.Aciklama).HasColumnName("Aciklama");
        builder.Property(e => e.Durum).HasColumnName("Durum").IsRequired();
        builder.Property(e => e.TalepTarihi).HasColumnName("TalepTarihi").IsRequired();
        builder.Property(e => e.SonGuncellemeTarihi).HasColumnName("SonGuncellemeTarihi");
        builder.Property(e => e.KatalogKaydiId).HasColumnName("KatalogKaydiId");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.TalepEdenKutuphane).WithMany(k => k.YeniKatalogTalepleri).HasForeignKey(e => e.TalepEdenKutuphaneId);
        builder.HasOne(e => e.KatalogKaydi).WithMany().HasForeignKey(e => e.KatalogKaydiId);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
