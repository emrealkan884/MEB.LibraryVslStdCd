using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.YeniKatalogTalepleri;

public interface IYeniKatalogTalebiService
{
    Task<YeniKatalogTalebi?> GetAsync(
        Expression<Func<YeniKatalogTalebi, bool>> predicate,
        Func<IQueryable<YeniKatalogTalebi>, IIncludableQueryable<YeniKatalogTalebi, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<YeniKatalogTalebi>?> GetListAsync(
        Expression<Func<YeniKatalogTalebi, bool>>? predicate = null,
        Func<IQueryable<YeniKatalogTalebi>, IOrderedQueryable<YeniKatalogTalebi>>? orderBy = null,
        Func<IQueryable<YeniKatalogTalebi>, IIncludableQueryable<YeniKatalogTalebi, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<YeniKatalogTalebi> AddAsync(YeniKatalogTalebi yeniKatalogTalebi);
    Task<YeniKatalogTalebi> UpdateAsync(YeniKatalogTalebi yeniKatalogTalebi);
    Task<YeniKatalogTalebi> DeleteAsync(YeniKatalogTalebi yeniKatalogTalebi, bool permanent = false);
}
