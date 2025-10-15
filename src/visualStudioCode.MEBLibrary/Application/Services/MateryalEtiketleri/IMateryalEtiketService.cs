using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.MateryalEtiketleri;

public interface IMateryalEtiketService
{
    Task<MateryalEtiket?> GetAsync(
        Expression<Func<MateryalEtiket, bool>> predicate,
        Func<IQueryable<MateryalEtiket>, IIncludableQueryable<MateryalEtiket, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<MateryalEtiket>?> GetListAsync(
        Expression<Func<MateryalEtiket, bool>>? predicate = null,
        Func<IQueryable<MateryalEtiket>, IOrderedQueryable<MateryalEtiket>>? orderBy = null,
        Func<IQueryable<MateryalEtiket>, IIncludableQueryable<MateryalEtiket, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<MateryalEtiket> AddAsync(MateryalEtiket materyalEtiket);
    Task<MateryalEtiket> UpdateAsync(MateryalEtiket materyalEtiket);
    Task<MateryalEtiket> DeleteAsync(MateryalEtiket materyalEtiket, bool permanent = false);
}
