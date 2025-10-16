using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IAuditLogRepository : IAsyncRepository<AuditLog, Guid>, IRepository<AuditLog, Guid>
{
}
