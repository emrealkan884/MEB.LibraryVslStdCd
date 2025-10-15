using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class KatalogKaydiYazarRepository : EfRepositoryBase<KatalogKaydiYazar, Guid, BaseDbContext>, IKatalogKaydiYazarRepository
{
    public KatalogKaydiYazarRepository(BaseDbContext context) : base(context)
    {
    }
}
