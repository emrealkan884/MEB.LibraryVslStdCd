using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IRezervasyonRepository : IAsyncRepository<Rezervasyon, Guid>, IRepository<Rezervasyon, Guid>
{
}
