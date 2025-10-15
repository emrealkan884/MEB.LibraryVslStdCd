using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IDeweySiniflamaRepository : IAsyncRepository<DeweySiniflama, Guid>, IRepository<DeweySiniflama, Guid>
{
}
