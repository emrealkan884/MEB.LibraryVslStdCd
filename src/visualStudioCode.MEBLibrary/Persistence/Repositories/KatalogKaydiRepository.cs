using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class KatalogKaydiRepository : EfRepositoryBase<KatalogKaydi, Guid, BaseDbContext>, IKatalogKaydiRepository
{
    public KatalogKaydiRepository(BaseDbContext context) : base(context)
    {
    }
}
