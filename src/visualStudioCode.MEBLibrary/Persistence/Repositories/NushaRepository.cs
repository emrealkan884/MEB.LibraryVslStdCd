using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class NushaRepository : EfRepositoryBase<Nusha, Guid, BaseDbContext>, INushaRepository
{
    public NushaRepository(BaseDbContext context) : base(context)
    {
    }
}
