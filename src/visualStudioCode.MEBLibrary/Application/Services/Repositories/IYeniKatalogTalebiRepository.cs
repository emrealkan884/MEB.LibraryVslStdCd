using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IYeniKatalogTalebiRepository : IAsyncRepository<YeniKatalogTalebi, Guid>, IRepository<YeniKatalogTalebi, Guid>
{
}
