using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs").HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
        builder.Property(x => x.UserId).HasColumnName("UserId");
        builder.Property(x => x.UserName).HasColumnName("UserName").HasMaxLength(256);
        builder.Property(x => x.ActionName).HasColumnName("ActionName").HasMaxLength(256).IsRequired();
        builder.Property(x => x.Payload).HasColumnName("Payload");
        builder.Property(x => x.ClientIp).HasColumnName("ClientIp").HasMaxLength(64);
        builder.Property(x => x.UserAgent).HasColumnName("UserAgent").HasMaxLength(512);
        builder.Property(x => x.OccurredOn).HasColumnName("OccurredOn").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(x => x.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(x => !x.DeletedDate.HasValue);
    }
}
