using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.DeweySiniflamalari;

public interface IDeweySiniflamaService
{
    Task<DeweySiniflama?> GetAsync(
        Expression<Func<DeweySiniflama, bool>> predicate,
        Func<IQueryable<DeweySiniflama>, IIncludableQueryable<DeweySiniflama, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<DeweySiniflama>?> GetListAsync(
        Expression<Func<DeweySiniflama, bool>>? predicate = null,
        Func<IQueryable<DeweySiniflama>, IOrderedQueryable<DeweySiniflama>>? orderBy = null,
        Func<IQueryable<DeweySiniflama>, IIncludableQueryable<DeweySiniflama, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<DeweySiniflama> AddAsync(DeweySiniflama deweySiniflama);
    Task<DeweySiniflama> UpdateAsync(DeweySiniflama deweySiniflama);
    Task<DeweySiniflama> DeleteAsync(DeweySiniflama deweySiniflama, bool permanent = false);
}
