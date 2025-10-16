using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class AuditLogRepository : EfRepositoryBase<AuditLog, Guid, BaseDbContext>, IAuditLogRepository
{
    public AuditLogRepository(BaseDbContext context)
        : base(context)
    {
    }
}
