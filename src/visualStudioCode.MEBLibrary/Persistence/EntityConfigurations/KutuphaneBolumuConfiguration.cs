using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class KutuphaneBolumuConfiguration : IEntityTypeConfiguration<KutuphaneBolumu>
{
    public void Configure(EntityTypeBuilder<KutuphaneBolumu> builder)
    {
        builder.ToTable("KutuphaneBolumler").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.KutuphaneId).HasColumnName("KutuphaneId").IsRequired();
        builder.Property(e => e.Ad).HasColumnName("Ad").IsRequired();
        builder.Property(e => e.Aciklama).HasColumnName("Aciklama");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.Kutuphane).WithMany(k => k.Bolumler).HasForeignKey(e => e.KutuphaneId);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
