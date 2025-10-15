using Application.Features.KatalogKonulari.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.KatalogKonulari;

public class KatalogKonuManager : IKatalogKonuService
{
    private readonly IKatalogKonuRepository _katalogKonuRepository;
    private readonly KatalogKonuBusinessRules _katalogKonuBusinessRules;

    public KatalogKonuManager(IKatalogKonuRepository katalogKonuRepository, KatalogKonuBusinessRules katalogKonuBusinessRules)
    {
        _katalogKonuRepository = katalogKonuRepository;
        _katalogKonuBusinessRules = katalogKonuBusinessRules;
    }

    public async Task<KatalogKonu?> GetAsync(
        Expression<Func<KatalogKonu, bool>> predicate,
        Func<IQueryable<KatalogKonu>, IIncludableQueryable<KatalogKonu, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        KatalogKonu? katalogKonu = await _katalogKonuRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return katalogKonu;
    }

    public async Task<IPaginate<KatalogKonu>?> GetListAsync(
        Expression<Func<KatalogKonu, bool>>? predicate = null,
        Func<IQueryable<KatalogKonu>, IOrderedQueryable<KatalogKonu>>? orderBy = null,
        Func<IQueryable<KatalogKonu>, IIncludableQueryable<KatalogKonu, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<KatalogKonu> katalogKonuList = await _katalogKonuRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return katalogKonuList;
    }

    public async Task<KatalogKonu> AddAsync(KatalogKonu katalogKonu)
    {
        KatalogKonu addedKatalogKonu = await _katalogKonuRepository.AddAsync(katalogKonu);

        return addedKatalogKonu;
    }

    public async Task<KatalogKonu> UpdateAsync(KatalogKonu katalogKonu)
    {
        KatalogKonu updatedKatalogKonu = await _katalogKonuRepository.UpdateAsync(katalogKonu);

        return updatedKatalogKonu;
    }

    public async Task<KatalogKonu> DeleteAsync(KatalogKonu katalogKonu, bool permanent = false)
    {
        KatalogKonu deletedKatalogKonu = await _katalogKonuRepository.DeleteAsync(katalogKonu);

        return deletedKatalogKonu;
    }
}
