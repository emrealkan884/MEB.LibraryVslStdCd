using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class DeweySiniflamaRepository : EfRepositoryBase<DeweySiniflama, Guid, BaseDbContext>, IDeweySiniflamaRepository
{
    public DeweySiniflamaRepository(BaseDbContext context) : base(context)
    {
    }
}
