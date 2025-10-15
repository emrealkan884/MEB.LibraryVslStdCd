using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.KatalogKonulari;

public interface IKatalogKonuService
{
    Task<KatalogKonu?> GetAsync(
        Expression<Func<KatalogKonu, bool>> predicate,
        Func<IQueryable<KatalogKonu>, IIncludableQueryable<KatalogKonu, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<KatalogKonu>?> GetListAsync(
        Expression<Func<KatalogKonu, bool>>? predicate = null,
        Func<IQueryable<KatalogKonu>, IOrderedQueryable<KatalogKonu>>? orderBy = null,
        Func<IQueryable<KatalogKonu>, IIncludableQueryable<KatalogKonu, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<KatalogKonu> AddAsync(KatalogKonu katalogKonu);
    Task<KatalogKonu> UpdateAsync(KatalogKonu katalogKonu);
    Task<KatalogKonu> DeleteAsync(KatalogKonu katalogKonu, bool permanent = false);
}
