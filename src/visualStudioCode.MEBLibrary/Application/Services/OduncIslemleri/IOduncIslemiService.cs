using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.OduncIslemleri;

public interface IOduncIslemiService
{
    Task<OduncIslemi?> GetAsync(
        Expression<Func<OduncIslemi, bool>> predicate,
        Func<IQueryable<OduncIslemi>, IIncludableQueryable<OduncIslemi, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<OduncIslemi>?> GetListAsync(
        Expression<Func<OduncIslemi, bool>>? predicate = null,
        Func<IQueryable<OduncIslemi>, IOrderedQueryable<OduncIslemi>>? orderBy = null,
        Func<IQueryable<OduncIslemi>, IIncludableQueryable<OduncIslemi, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<OduncIslemi> AddAsync(OduncIslemi oduncIslemi);
    Task<OduncIslemi> UpdateAsync(OduncIslemi oduncIslemi);
    Task<OduncIslemi> DeleteAsync(OduncIslemi oduncIslemi, bool permanent = false);
}
