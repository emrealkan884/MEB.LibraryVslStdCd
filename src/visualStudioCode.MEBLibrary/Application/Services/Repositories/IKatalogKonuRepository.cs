using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IKatalogKonuRepository : IAsyncRepository<KatalogKonu, Guid>, IRepository<KatalogKonu, Guid>
{
}
