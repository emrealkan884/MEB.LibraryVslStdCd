using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class DeweySiniflamaConfiguration : IEntityTypeConfiguration<DeweySiniflama>
{
    public void Configure(EntityTypeBuilder<DeweySiniflama> builder)
    {
        builder.ToTable("DeweySiniflamalar").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.Kod).HasColumnName("Kod").IsRequired();
        builder.Property(e => e.Baslik).HasColumnName("Baslik").IsRequired();
        builder.Property(e => e.Aciklama).HasColumnName("Aciklama");
        builder.Property(e => e.UstSinifId).HasColumnName("UstSinifId");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.UstSinif).WithMany(d => d.AltSiniflar).HasForeignKey(e => e.UstSinifId);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
