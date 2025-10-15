using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class YazarConfiguration : IEntityTypeConfiguration<Yazar>
{
    public void Configure(EntityTypeBuilder<Yazar> builder)
    {
        builder.ToTable("Yazarlar").HasKey(y => y.Id);

        builder.Property(y => y.Id).HasColumnName("Id").IsRequired();
        builder.Property(y => y.AdSoyad).HasColumnName("AdSoyad").IsRequired();
        builder.Property(y => y.DogumTarihi).HasColumnName("DogumTarihi");
        builder.Property(y => y.Uyruk).HasColumnName("Uyruk");
        builder.Property(y => y.Aciklama).HasColumnName("Aciklama");
        builder.Property(y => y.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(y => y.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(y => y.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(y => !y.DeletedDate.HasValue);
    }
}