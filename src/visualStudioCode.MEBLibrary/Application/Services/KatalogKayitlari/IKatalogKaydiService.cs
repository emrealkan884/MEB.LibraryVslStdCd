using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.KatalogKayitlari;

public interface IKatalogKaydiService
{
    Task<KatalogKaydi?> GetAsync(
        Expression<Func<KatalogKaydi, bool>> predicate,
        Func<IQueryable<KatalogKaydi>, IIncludableQueryable<KatalogKaydi, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<KatalogKaydi>?> GetListAsync(
        Expression<Func<KatalogKaydi, bool>>? predicate = null,
        Func<IQueryable<KatalogKaydi>, IOrderedQueryable<KatalogKaydi>>? orderBy = null,
        Func<IQueryable<KatalogKaydi>, IIncludableQueryable<KatalogKaydi, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<KatalogKaydi> AddAsync(KatalogKaydi katalogKaydi);
    Task<KatalogKaydi> UpdateAsync(KatalogKaydi katalogKaydi);
    Task<KatalogKaydi> DeleteAsync(KatalogKaydi katalogKaydi, bool permanent = false);
}
