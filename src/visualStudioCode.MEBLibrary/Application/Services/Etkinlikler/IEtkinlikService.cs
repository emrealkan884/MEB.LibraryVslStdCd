using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Etkinlikler;

public interface IEtkinlikService
{
    Task<Etkinlik?> GetAsync(
        Expression<Func<Etkinlik, bool>> predicate,
        Func<IQueryable<Etkinlik>, IIncludableQueryable<Etkinlik, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Etkinlik>?> GetListAsync(
        Expression<Func<Etkinlik, bool>>? predicate = null,
        Func<IQueryable<Etkinlik>, IOrderedQueryable<Etkinlik>>? orderBy = null,
        Func<IQueryable<Etkinlik>, IIncludableQueryable<Etkinlik, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Etkinlik> AddAsync(Etkinlik etkinlik);
    Task<Etkinlik> UpdateAsync(Etkinlik etkinlik);
    Task<Etkinlik> DeleteAsync(Etkinlik etkinlik, bool permanent = false);
}
