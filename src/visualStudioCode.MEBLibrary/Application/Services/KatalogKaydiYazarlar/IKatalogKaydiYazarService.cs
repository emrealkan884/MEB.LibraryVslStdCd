using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.KatalogKaydiYazarlar;

public interface IKatalogKaydiYazarService
{
    Task<KatalogKaydiYazar?> GetAsync(
        Expression<Func<KatalogKaydiYazar, bool>> predicate,
        Func<IQueryable<KatalogKaydiYazar>, IIncludableQueryable<KatalogKaydiYazar, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<KatalogKaydiYazar>?> GetListAsync(
        Expression<Func<KatalogKaydiYazar, bool>>? predicate = null,
        Func<IQueryable<KatalogKaydiYazar>, IOrderedQueryable<KatalogKaydiYazar>>? orderBy = null,
        Func<IQueryable<KatalogKaydiYazar>, IIncludableQueryable<KatalogKaydiYazar, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<KatalogKaydiYazar> AddAsync(KatalogKaydiYazar katalogKaydiYazar);
    Task<KatalogKaydiYazar> UpdateAsync(KatalogKaydiYazar katalogKaydiYazar);
    Task<KatalogKaydiYazar> DeleteAsync(KatalogKaydiYazar katalogKaydiYazar, bool permanent = false);
}
