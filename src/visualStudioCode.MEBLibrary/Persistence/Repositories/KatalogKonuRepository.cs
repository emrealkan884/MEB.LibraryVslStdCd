using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class KatalogKonuRepository : EfRepositoryBase<KatalogKonu, Guid, BaseDbContext>, IKatalogKonuRepository
{
    public KatalogKonuRepository(BaseDbContext context) : base(context)
    {
    }
}
