using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IMateryalEtiketRepository : IAsyncRepository<MateryalEtiket, Guid>, IRepository<MateryalEtiket, Guid>
{
}
