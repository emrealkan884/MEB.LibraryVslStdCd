using Domain.Entities;
using Domain.ValueObjects.Marc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Persistence.EntityConfigurations;

public class KatalogKaydiConfiguration : IEntityTypeConfiguration<KatalogKaydi>
{
    public void Configure(EntityTypeBuilder<KatalogKaydi> builder)
    {
        builder.ToTable("KatalogKayitlari").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.DeweySiniflamaId).HasColumnName("DeweySiniflamaId");
        builder.Property(e => e.YeniKatalogTalebiId).HasColumnName("YeniKatalogTalebiId");
        builder.Property(e => e.KutuphaneId).HasColumnName("KutuphaneId").IsRequired();
        builder.Property(e => e.Baslik).HasColumnName("Baslik").IsRequired();
        builder.Property(e => e.AltBaslik).HasColumnName("AltBaslik");
        builder.Property(e => e.Isbn).HasColumnName("Isbn");
        builder.Property(e => e.Yayinevi).HasColumnName("Yayinevi");
        builder.Property(e => e.YayinYeri).HasColumnName("YayinYeri");
        builder.Property(e => e.YayinYili).HasColumnName("YayinYili");
        builder.Property(e => e.SayfaSayisi).HasColumnName("SayfaSayisi");
        builder.Property(e => e.Dil).HasColumnName("Dil");
        builder.Property(e => e.Dizi).HasColumnName("Dizi");
        builder.Property(e => e.Baski).HasColumnName("Baski");
        builder.Property(e => e.Ozet).HasColumnName("Ozet");
        builder.Property(e => e.Notlar).HasColumnName("Notlar");
        builder.Property(e => e.KapakResmiYolu).HasColumnName("KapakResmiYolu");
        builder.Property(e => e.MateryalTuru).HasColumnName("MateryalTuru").IsRequired();
        builder.Property(e => e.MateryalAltTuru).HasColumnName("MateryalAltTuru");
        builder.Property(e => e.Marc21Verisi).HasColumnName("Marc21Verisi");
        builder.Property(e => e.MarcAlanlari)
            .HasColumnName("MarcAlanlari")
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                v => JsonSerializer.Deserialize<List<MarcAlan>>(v, new JsonSerializerOptions())
            );
        builder.Property(e => e.RdaUyumlu).HasColumnName("RdaUyumlu").IsRequired();
        builder.Property(e => e.KayitTarihi).HasColumnName("KayitTarihi").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.Kutuphane).WithMany(k => k.KatalogKayitlari).HasForeignKey(e => e.KutuphaneId);
        builder.HasOne(e => e.DeweySiniflama).WithMany(d => d.KatalogKayitlari).HasForeignKey(e => e.DeweySiniflamaId);
        builder.HasOne(e => e.YeniKatalogTalebi).WithMany().HasForeignKey(e => e.YeniKatalogTalebiId);

        //Global query filter for soft delete
        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
