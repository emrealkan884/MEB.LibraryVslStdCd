using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class KutuphaneRepository : EfRepositoryBase<Kutuphane, Guid, BaseDbContext>, IKutuphaneRepository
{
    public KutuphaneRepository(BaseDbContext context) : base(context)
    {
    }
}
