using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IOduncIslemiRepository : IAsyncRepository<OduncIslemi, Guid>, IRepository<OduncIslemi, Guid>
{
}
