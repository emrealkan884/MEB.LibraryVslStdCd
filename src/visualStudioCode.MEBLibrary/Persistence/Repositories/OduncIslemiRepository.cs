using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class OduncIslemiRepository : EfRepositoryBase<OduncIslemi, Guid, BaseDbContext>, IOduncIslemiRepository
{
    public OduncIslemiRepository(BaseDbContext context) : base(context)
    {
    }
}
