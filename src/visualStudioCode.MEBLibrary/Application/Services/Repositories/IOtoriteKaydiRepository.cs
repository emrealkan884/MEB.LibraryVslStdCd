using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IOtoriteKaydiRepository : IAsyncRepository<OtoriteKaydi, Guid>, IRepository<OtoriteKaydi, Guid>
{
}
