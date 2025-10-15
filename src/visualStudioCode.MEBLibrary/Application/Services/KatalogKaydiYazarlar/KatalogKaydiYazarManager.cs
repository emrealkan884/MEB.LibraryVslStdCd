using Application.Features.KatalogKaydiYazarlar.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.KatalogKaydiYazarlar;

public class KatalogKaydiYazarManager : IKatalogKaydiYazarService
{
    private readonly IKatalogKaydiYazarRepository _katalogKaydiYazarRepository;
    private readonly KatalogKaydiYazarBusinessRules _katalogKaydiYazarBusinessRules;

    public KatalogKaydiYazarManager(IKatalogKaydiYazarRepository katalogKaydiYazarRepository, KatalogKaydiYazarBusinessRules katalogKaydiYazarBusinessRules)
    {
        _katalogKaydiYazarRepository = katalogKaydiYazarRepository;
        _katalogKaydiYazarBusinessRules = katalogKaydiYazarBusinessRules;
    }

    public async Task<KatalogKaydiYazar?> GetAsync(
        Expression<Func<KatalogKaydiYazar, bool>> predicate,
        Func<IQueryable<KatalogKaydiYazar>, IIncludableQueryable<KatalogKaydiYazar, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        KatalogKaydiYazar? katalogKaydiYazar = await _katalogKaydiYazarRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return katalogKaydiYazar;
    }

    public async Task<IPaginate<KatalogKaydiYazar>?> GetListAsync(
        Expression<Func<KatalogKaydiYazar, bool>>? predicate = null,
        Func<IQueryable<KatalogKaydiYazar>, IOrderedQueryable<KatalogKaydiYazar>>? orderBy = null,
        Func<IQueryable<KatalogKaydiYazar>, IIncludableQueryable<KatalogKaydiYazar, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<KatalogKaydiYazar> katalogKaydiYazarList = await _katalogKaydiYazarRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return katalogKaydiYazarList;
    }

    public async Task<KatalogKaydiYazar> AddAsync(KatalogKaydiYazar katalogKaydiYazar)
    {
        KatalogKaydiYazar addedKatalogKaydiYazar = await _katalogKaydiYazarRepository.AddAsync(katalogKaydiYazar);

        return addedKatalogKaydiYazar;
    }

    public async Task<KatalogKaydiYazar> UpdateAsync(KatalogKaydiYazar katalogKaydiYazar)
    {
        KatalogKaydiYazar updatedKatalogKaydiYazar = await _katalogKaydiYazarRepository.UpdateAsync(katalogKaydiYazar);

        return updatedKatalogKaydiYazar;
    }

    public async Task<KatalogKaydiYazar> DeleteAsync(KatalogKaydiYazar katalogKaydiYazar, bool permanent = false)
    {
        KatalogKaydiYazar deletedKatalogKaydiYazar = await _katalogKaydiYazarRepository.DeleteAsync(katalogKaydiYazar);

        return deletedKatalogKaydiYazar;
    }
}
