using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class YeniKatalogTalebiRepository : EfRepositoryBase<YeniKatalogTalebi, Guid, BaseDbContext>, IYeniKatalogTalebiRepository
{
    public YeniKatalogTalebiRepository(BaseDbContext context) : base(context)
    {
    }
}
