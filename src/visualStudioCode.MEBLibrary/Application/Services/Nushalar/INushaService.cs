using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Nushalar;

public interface INushaService
{
    Task<Nusha?> GetAsync(
        Expression<Func<Nusha, bool>> predicate,
        Func<IQueryable<Nusha>, IIncludableQueryable<Nusha, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Nusha>?> GetListAsync(
        Expression<Func<Nusha, bool>>? predicate = null,
        Func<IQueryable<Nusha>, IOrderedQueryable<Nusha>>? orderBy = null,
        Func<IQueryable<Nusha>, IIncludableQueryable<Nusha, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Nusha> AddAsync(Nusha nusha);
    Task<Nusha> UpdateAsync(Nusha nusha);
    Task<Nusha> DeleteAsync(Nusha nusha, bool permanent = false);
}
