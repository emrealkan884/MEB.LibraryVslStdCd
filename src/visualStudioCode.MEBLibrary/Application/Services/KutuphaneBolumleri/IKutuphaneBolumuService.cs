using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.KutuphaneBolumleri;

public interface IKutuphaneBolumuService
{
    Task<KutuphaneBolumu?> GetAsync(
        Expression<Func<KutuphaneBolumu, bool>> predicate,
        Func<IQueryable<KutuphaneBolumu>, IIncludableQueryable<KutuphaneBolumu, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<KutuphaneBolumu>?> GetListAsync(
        Expression<Func<KutuphaneBolumu, bool>>? predicate = null,
        Func<IQueryable<KutuphaneBolumu>, IOrderedQueryable<KutuphaneBolumu>>? orderBy = null,
        Func<IQueryable<KutuphaneBolumu>, IIncludableQueryable<KutuphaneBolumu, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<KutuphaneBolumu> AddAsync(KutuphaneBolumu kutuphaneBolumu);
    Task<KutuphaneBolumu> UpdateAsync(KutuphaneBolumu kutuphaneBolumu);
    Task<KutuphaneBolumu> DeleteAsync(KutuphaneBolumu kutuphaneBolumu, bool permanent = false);
}
