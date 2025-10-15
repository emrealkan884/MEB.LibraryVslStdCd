using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class MateryalEtiketRepository : EfRepositoryBase<MateryalEtiket, Guid, BaseDbContext>, IMateryalEtiketRepository
{
    public MateryalEtiketRepository(BaseDbContext context) : base(context)
    {
    }
}
