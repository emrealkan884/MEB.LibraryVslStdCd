using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class YazarRepository : EfRepositoryBase<Yazar, Guid, BaseDbContext>, IYazarRepository
{
    public YazarRepository(BaseDbContext context) : base(context)
    {
    }
}