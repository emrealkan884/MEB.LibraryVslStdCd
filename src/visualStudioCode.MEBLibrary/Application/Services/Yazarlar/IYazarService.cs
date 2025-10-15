using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Yazarlar;

public interface IYazarService
{
    Task<Yazar?> GetAsync(
        Expression<Func<Yazar, bool>> predicate,
        Func<IQueryable<Yazar>, IIncludableQueryable<Yazar, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Yazar>?> GetListAsync(
        Expression<Func<Yazar, bool>>? predicate = null,
        Func<IQueryable<Yazar>, IOrderedQueryable<Yazar>>? orderBy = null,
        Func<IQueryable<Yazar>, IIncludableQueryable<Yazar, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Yazar> AddAsync(Yazar yazar);
    Task<Yazar> UpdateAsync(Yazar yazar);
    Task<Yazar> DeleteAsync(Yazar yazar, bool permanent = false);
}
