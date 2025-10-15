using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Kutuphaneler;

public interface IKutuphaneService
{
    Task<Kutuphane?> GetAsync(
        Expression<Func<Kutuphane, bool>> predicate,
        Func<IQueryable<Kutuphane>, IIncludableQueryable<Kutuphane, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Kutuphane>?> GetListAsync(
        Expression<Func<Kutuphane, bool>>? predicate = null,
        Func<IQueryable<Kutuphane>, IOrderedQueryable<Kutuphane>>? orderBy = null,
        Func<IQueryable<Kutuphane>, IIncludableQueryable<Kutuphane, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Kutuphane> AddAsync(Kutuphane kutuphane);
    Task<Kutuphane> UpdateAsync(Kutuphane kutuphane);
    Task<Kutuphane> DeleteAsync(Kutuphane kutuphane, bool permanent = false);
}
