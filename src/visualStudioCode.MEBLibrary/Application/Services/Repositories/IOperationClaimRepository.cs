using Domain.Entities;
using Domain.Entities.Security;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IOperationClaimRepository : IAsyncRepository<OperationClaim, int>, IRepository<OperationClaim, int> { }
