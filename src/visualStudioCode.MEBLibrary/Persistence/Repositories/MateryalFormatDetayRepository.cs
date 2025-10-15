using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class MateryalFormatDetayRepository : EfRepositoryBase<MateryalFormatDetay, Guid, BaseDbContext>, IMateryalFormatDetayRepository
{
    public MateryalFormatDetayRepository(BaseDbContext context) : base(context)
    {
    }
}
