using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Materyaller;

public interface IMateryalService
{
    Task<Materyal?> GetAsync(
        Expression<Func<Materyal, bool>> predicate,
        Func<IQueryable<Materyal>, IIncludableQueryable<Materyal, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Materyal>?> GetListAsync(
        Expression<Func<Materyal, bool>>? predicate = null,
        Func<IQueryable<Materyal>, IOrderedQueryable<Materyal>>? orderBy = null,
        Func<IQueryable<Materyal>, IIncludableQueryable<Materyal, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Materyal> AddAsync(Materyal materyal);
    Task<Materyal> UpdateAsync(Materyal materyal);
    Task<Materyal> DeleteAsync(Materyal materyal, bool permanent = false);
}
