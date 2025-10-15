using Application.Features.KatalogKayitlari.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.KatalogKayitlari;

public class KatalogKaydiManager : IKatalogKaydiService
{
    private readonly IKatalogKaydiRepository _katalogKaydiRepository;
    private readonly KatalogKaydiBusinessRules _katalogKaydiBusinessRules;

    public KatalogKaydiManager(IKatalogKaydiRepository katalogKaydiRepository, KatalogKaydiBusinessRules katalogKaydiBusinessRules)
    {
        _katalogKaydiRepository = katalogKaydiRepository;
        _katalogKaydiBusinessRules = katalogKaydiBusinessRules;
    }

    public async Task<KatalogKaydi?> GetAsync(
        Expression<Func<KatalogKaydi, bool>> predicate,
        Func<IQueryable<KatalogKaydi>, IIncludableQueryable<KatalogKaydi, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        KatalogKaydi? katalogKaydi = await _katalogKaydiRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return katalogKaydi;
    }

    public async Task<IPaginate<KatalogKaydi>?> GetListAsync(
        Expression<Func<KatalogKaydi, bool>>? predicate = null,
        Func<IQueryable<KatalogKaydi>, IOrderedQueryable<KatalogKaydi>>? orderBy = null,
        Func<IQueryable<KatalogKaydi>, IIncludableQueryable<KatalogKaydi, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<KatalogKaydi> katalogKaydiList = await _katalogKaydiRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return katalogKaydiList;
    }

    public async Task<KatalogKaydi> AddAsync(KatalogKaydi katalogKaydi)
    {
        KatalogKaydi addedKatalogKaydi = await _katalogKaydiRepository.AddAsync(katalogKaydi);

        return addedKatalogKaydi;
    }

    public async Task<KatalogKaydi> UpdateAsync(KatalogKaydi katalogKaydi)
    {
        KatalogKaydi updatedKatalogKaydi = await _katalogKaydiRepository.UpdateAsync(katalogKaydi);

        return updatedKatalogKaydi;
    }

    public async Task<KatalogKaydi> DeleteAsync(KatalogKaydi katalogKaydi, bool permanent = false)
    {
        KatalogKaydi deletedKatalogKaydi = await _katalogKaydiRepository.DeleteAsync(katalogKaydi);

        return deletedKatalogKaydi;
    }
}
