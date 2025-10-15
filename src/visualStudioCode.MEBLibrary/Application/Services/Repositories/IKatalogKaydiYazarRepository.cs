using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IKatalogKaydiYazarRepository : IAsyncRepository<KatalogKaydiYazar, Guid>, IRepository<KatalogKaydiYazar, Guid>
{
}
