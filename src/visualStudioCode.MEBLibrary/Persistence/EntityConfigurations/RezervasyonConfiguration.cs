using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class RezervasyonConfiguration : IEntityTypeConfiguration<Rezervasyon>
{
    public void Configure(EntityTypeBuilder<Rezervasyon> builder)
    {
        builder.ToTable("Rezervasyonlar").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.KutuphaneId).HasColumnName("KutuphaneId").IsRequired();
        builder.Property(e => e.KullaniciId).HasColumnName("KullaniciId").IsRequired();
        builder.Property(e => e.MateryalId).HasColumnName("MateryalId").IsRequired();
        builder.Property(e => e.TalepTarihi).HasColumnName("TalepTarihi").IsRequired();
        builder.Property(e => e.HazirlanmaTarihi).HasColumnName("HazirlanmaTarihi");
        builder.Property(e => e.TamamlanmaTarihi).HasColumnName("TamamlanmaTarihi");
        builder.Property(e => e.Durumu).HasColumnName("Durumu").IsRequired();
        builder.Property(e => e.Not).HasColumnName("Not");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.Kutuphane).WithMany().HasForeignKey(e => e.KutuphaneId);
        builder.HasOne(e => e.Materyal).WithMany().HasForeignKey(e => e.MateryalId);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
