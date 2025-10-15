using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class MateryalConfiguration : IEntityTypeConfiguration<Materyal>
{
    public void Configure(EntityTypeBuilder<Materyal> builder)
    {
        builder.ToTable("Materyaller").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.KatalogKaydiId).HasColumnName("KatalogKaydiId").IsRequired();
        builder.Property(e => e.KutuphaneId).HasColumnName("KutuphaneId").IsRequired();
        builder.Property(e => e.KutuphaneBolumuId).HasColumnName("KutuphaneBolumuId");
        builder.Property(e => e.Formati).HasColumnName("Formati").IsRequired();
        builder.Property(e => e.RezervasyonaAcik).HasColumnName("RezervasyonaAcik").IsRequired();
        builder.Property(e => e.MaksimumOduncSuresiGun).HasColumnName("MaksimumOduncSuresiGun").IsRequired();
        builder.Property(e => e.Not).HasColumnName("Not");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");


        builder.HasOne(e => e.KatalogKaydi).WithMany(k => k.Materyaller).HasForeignKey(e => e.KatalogKaydiId);
        builder.HasOne(e => e.Kutuphane).WithMany(k => k.Materyaller).HasForeignKey(e => e.KutuphaneId);
        builder.HasOne(e => e.Bolum).WithMany(k => k.Materyaller).HasForeignKey(e => e.KutuphaneBolumuId);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}
