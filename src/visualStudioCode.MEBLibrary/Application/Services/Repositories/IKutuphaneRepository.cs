using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IKutuphaneRepository : IAsyncRepository<Kutuphane, Guid>, IRepository<Kutuphane, Guid>
{
}
