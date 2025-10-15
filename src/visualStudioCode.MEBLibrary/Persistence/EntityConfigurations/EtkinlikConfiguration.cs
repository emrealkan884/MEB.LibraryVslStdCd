using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class EtkinlikConfiguration : IEntityTypeConfiguration<Etkinlik>
{
    public void Configure(EntityTypeBuilder<Etkinlik> builder)
    {
        builder.ToTable("Etkinlikler").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.KutuphaneId).HasColumnName("KutuphaneId").IsRequired();
        builder.Property(e => e.Baslik).HasColumnName("Baslik").IsRequired();
        builder.Property(e => e.Aciklama).HasColumnName("Aciklama");
        builder.Property(e => e.BaslangicTarihi).HasColumnName("BaslangicTarihi").IsRequired();
        builder.Property(e => e.BitisTarihi).HasColumnName("BitisTarihi").IsRequired();
        builder.Property(e => e.Konum).HasColumnName("Konum");
        builder.Property(e => e.AfisDosyasi).HasColumnName("AfisDosyasi");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.Kutuphane).WithMany(k => k.Etkinlikler).HasForeignKey(e => e.KutuphaneId);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
