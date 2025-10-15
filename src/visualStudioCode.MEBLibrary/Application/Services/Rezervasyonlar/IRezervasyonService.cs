using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Rezervasyonlar;

public interface IRezervasyonService
{
    Task<Rezervasyon?> GetAsync(
        Expression<Func<Rezervasyon, bool>> predicate,
        Func<IQueryable<Rezervasyon>, IIncludableQueryable<Rezervasyon, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Rezervasyon>?> GetListAsync(
        Expression<Func<Rezervasyon, bool>>? predicate = null,
        Func<IQueryable<Rezervasyon>, IOrderedQueryable<Rezervasyon>>? orderBy = null,
        Func<IQueryable<Rezervasyon>, IIncludableQueryable<Rezervasyon, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Rezervasyon> AddAsync(Rezervasyon rezervasyon);
    Task<Rezervasyon> UpdateAsync(Rezervasyon rezervasyon);
    Task<Rezervasyon> DeleteAsync(Rezervasyon rezervasyon, bool permanent = false);
}
