using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class RafRepository : EfRepositoryBase<Raf, Guid, BaseDbContext>, IRafRepository
{
    public RafRepository(BaseDbContext context) : base(context)
    {
    }
}
