using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class KutuphaneConfiguration : IEntityTypeConfiguration<Kutuphane>
{
    public void Configure(EntityTypeBuilder<Kutuphane> builder)
    {
        builder.ToTable("Kutuphaneler").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.Kod).HasColumnName("Kod").IsRequired();
        builder.Property(e => e.Ad).HasColumnName("Ad").IsRequired();
        builder.Property(e => e.Tip).HasColumnName("Tip").IsRequired();
        builder.Property(e => e.Adres).HasColumnName("Adres").IsRequired();
        builder.Property(e => e.Telefon).HasColumnName("Telefon");
        builder.Property(e => e.EPosta).HasColumnName("EPosta");
        builder.Property(e => e.Aktif).HasColumnName("Aktif").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");



        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
