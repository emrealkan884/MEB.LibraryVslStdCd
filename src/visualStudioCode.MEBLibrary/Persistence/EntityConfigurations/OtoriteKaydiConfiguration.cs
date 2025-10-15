using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class OtoriteKaydiConfiguration : IEntityTypeConfiguration<OtoriteKaydi>
{
    public void Configure(EntityTypeBuilder<OtoriteKaydi> builder)
    {
        builder.ToTable("OtoriteKayitlari").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.YetkiliBaslik).HasColumnName("YetkiliBaslik").IsRequired();
        builder.Property(e => e.OtoriteTuru).HasColumnName("OtoriteTuru").IsRequired();
        builder.Property(e => e.AlternatifBasliklar).HasColumnName("AlternatifBasliklar");
        builder.Property(e => e.BagliTerimler).HasColumnName("BagliTerimler");
        builder.Property(e => e.Aciklama).HasColumnName("Aciklama");
        builder.Property(e => e.HariciKayitNo).HasColumnName("HariciKayitNo");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");



        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
