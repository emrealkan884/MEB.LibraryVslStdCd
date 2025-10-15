using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.MateryalFormatDetaylari;

public interface IMateryalFormatDetayService
{
    Task<MateryalFormatDetay?> GetAsync(
        Expression<Func<MateryalFormatDetay, bool>> predicate,
        Func<IQueryable<MateryalFormatDetay>, IIncludableQueryable<MateryalFormatDetay, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<MateryalFormatDetay>?> GetListAsync(
        Expression<Func<MateryalFormatDetay, bool>>? predicate = null,
        Func<IQueryable<MateryalFormatDetay>, IOrderedQueryable<MateryalFormatDetay>>? orderBy = null,
        Func<IQueryable<MateryalFormatDetay>, IIncludableQueryable<MateryalFormatDetay, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<MateryalFormatDetay> AddAsync(MateryalFormatDetay materyalFormatDetay);
    Task<MateryalFormatDetay> UpdateAsync(MateryalFormatDetay materyalFormatDetay);
    Task<MateryalFormatDetay> DeleteAsync(MateryalFormatDetay materyalFormatDetay, bool permanent = false);
}
