using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class MateryalEtiketConfiguration : IEntityTypeConfiguration<MateryalEtiket>
{
    public void Configure(EntityTypeBuilder<MateryalEtiket> builder)
    {
        builder.ToTable("MateryalEtiketler").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.MateryalId).HasColumnName("MateryalId").IsRequired();
        builder.Property(e => e.Etiket).HasColumnName("Etiket").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.Materyal).WithMany(m => m.Etiketler).HasForeignKey(e => e.MateryalId);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
