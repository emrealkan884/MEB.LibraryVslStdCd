using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class MateryalFormatDetayConfiguration : IEntityTypeConfiguration<MateryalFormatDetay>
{
    public void Configure(EntityTypeBuilder<MateryalFormatDetay> builder)
    {
        builder.ToTable("MateryalFormatDetaylar").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.KatalogKaydiId).HasColumnName("KatalogKaydiId").IsRequired();
        builder.Property(e => e.MateryalTuru).HasColumnName("MateryalTuru").IsRequired();
        builder.Property(e => e.FizikselTanimi).HasColumnName("FizikselTanimi");
        builder.Property(e => e.SureBilgisi).HasColumnName("SureBilgisi");
        builder.Property(e => e.FormatBilgisi).HasColumnName("FormatBilgisi");
        builder.Property(e => e.Dil).HasColumnName("Dil");
        builder.Property(e => e.ErişimBilgisi).HasColumnName("ErişimBilgisi");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.KatalogKaydi).WithMany(k => k.FormatDetaylari).HasForeignKey(e => e.KatalogKaydiId);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
