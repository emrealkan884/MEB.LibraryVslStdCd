using Domain.Entities;
using Domain.Entities.Security;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IEmailAuthenticatorRepository
    : IAsyncRepository<EmailAuthenticator, Guid>,
        IRepository<EmailAuthenticator, Guid> { }
