using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.OtoriteKayitlari;

public interface IOtoriteKaydiService
{
    Task<OtoriteKaydi?> GetAsync(
        Expression<Func<OtoriteKaydi, bool>> predicate,
        Func<IQueryable<OtoriteKaydi>, IIncludableQueryable<OtoriteKaydi, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<OtoriteKaydi>?> GetListAsync(
        Expression<Func<OtoriteKaydi, bool>>? predicate = null,
        Func<IQueryable<OtoriteKaydi>, IOrderedQueryable<OtoriteKaydi>>? orderBy = null,
        Func<IQueryable<OtoriteKaydi>, IIncludableQueryable<OtoriteKaydi, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<OtoriteKaydi> AddAsync(OtoriteKaydi otoriteKaydi);
    Task<OtoriteKaydi> UpdateAsync(OtoriteKaydi otoriteKaydi);
    Task<OtoriteKaydi> DeleteAsync(OtoriteKaydi otoriteKaydi, bool permanent = false);
}
