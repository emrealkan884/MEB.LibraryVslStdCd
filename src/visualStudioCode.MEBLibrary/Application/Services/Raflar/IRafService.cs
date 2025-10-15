using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Raflar;

public interface IRafService
{
    Task<Raf?> GetAsync(
        Expression<Func<Raf, bool>> predicate,
        Func<IQueryable<Raf>, IIncludableQueryable<Raf, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Raf>?> GetListAsync(
        Expression<Func<Raf, bool>>? predicate = null,
        Func<IQueryable<Raf>, IOrderedQueryable<Raf>>? orderBy = null,
        Func<IQueryable<Raf>, IIncludableQueryable<Raf, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Raf> AddAsync(Raf raf);
    Task<Raf> UpdateAsync(Raf raf);
    Task<Raf> DeleteAsync(Raf raf, bool permanent = false);
}
