using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class EtkinlikRepository : EfRepositoryBase<Etkinlik, Guid, BaseDbContext>, IEtkinlikRepository
{
    public EtkinlikRepository(BaseDbContext context) : base(context)
    {
    }
}
