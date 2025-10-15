using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class KutuphaneBolumuRepository : EfRepositoryBase<KutuphaneBolumu, Guid, BaseDbContext>, IKutuphaneBolumuRepository
{
    public KutuphaneBolumuRepository(BaseDbContext context) : base(context)
    {
    }
}
