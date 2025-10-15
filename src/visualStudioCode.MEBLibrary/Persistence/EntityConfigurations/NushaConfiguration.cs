using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class NushaConfiguration : IEntityTypeConfiguration<Nusha>
{
    public void Configure(EntityTypeBuilder<Nusha> builder)
    {
        builder.ToTable("Nushalar").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.MateryalId).HasColumnName("MateryalId").IsRequired();
        builder.Property(e => e.RafId).HasColumnName("RafId");
        builder.Property(e => e.Barkod).HasColumnName("Barkod").IsRequired();
        builder.Property(e => e.Durumu).HasColumnName("Durumu").IsRequired();
        builder.Property(e => e.EklenmeTarihi).HasColumnName("EklenmeTarihi").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.Materyal).WithMany(m => m.Nushalar).HasForeignKey(e => e.MateryalId);
        builder.HasOne(e => e.Raf).WithMany(r => r.Nushalar).HasForeignKey(e => e.RafId);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
