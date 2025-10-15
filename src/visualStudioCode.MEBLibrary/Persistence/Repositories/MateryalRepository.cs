using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class MateryalRepository : EfRepositoryBase<Materyal, Guid, BaseDbContext>, IMateryalRepository
{
    public MateryalRepository(BaseDbContext context) : base(context)
    {
    }
}
