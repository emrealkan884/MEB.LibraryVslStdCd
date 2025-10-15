using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IEtkinlikRepository : IAsyncRepository<Etkinlik, Guid>, IRepository<Etkinlik, Guid>
{
}
