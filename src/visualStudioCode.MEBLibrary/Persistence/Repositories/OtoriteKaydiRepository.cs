using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class OtoriteKaydiRepository : EfRepositoryBase<OtoriteKaydi, Guid, BaseDbContext>, IOtoriteKaydiRepository
{
    public OtoriteKaydiRepository(BaseDbContext context) : base(context)
    {
    }
}
