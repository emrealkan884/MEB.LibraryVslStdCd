using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class OduncIslemiConfiguration : IEntityTypeConfiguration<OduncIslemi>
{
    public void Configure(EntityTypeBuilder<OduncIslemi> builder)
    {
        builder.ToTable("OduncIslemleri").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.KutuphaneId).HasColumnName("KutuphaneId").IsRequired();
        builder.Property(e => e.KullaniciId).HasColumnName("KullaniciId").IsRequired();
        builder.Property(e => e.NushaId).HasColumnName("NushaId").IsRequired();
        builder.Property(e => e.AlinmaTarihi).HasColumnName("AlinmaTarihi").IsRequired();
        builder.Property(e => e.SonTeslimTarihi).HasColumnName("SonTeslimTarihi").IsRequired();
        builder.Property(e => e.IadeTarihi).HasColumnName("IadeTarihi");
        builder.Property(e => e.Durumu).HasColumnName("Durumu").IsRequired();
        builder.Property(e => e.Not).HasColumnName("Not");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.Kutuphane).WithMany().HasForeignKey(e => e.KutuphaneId);
        builder.HasOne(e => e.Nusha).WithMany(n => n.OduncIslemleri).HasForeignKey(e => e.NushaId);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
