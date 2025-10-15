using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IMateryalFormatDetayRepository : IAsyncRepository<MateryalFormatDetay, Guid>, IRepository<MateryalFormatDetay, Guid>
{
}
