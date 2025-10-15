using Application.Features.Kutuphaneler.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Kutuphaneler;

public class KutuphaneManager : IKutuphaneService
{
    private readonly IKutuphaneRepository _kutuphaneRepository;
    private readonly KutuphaneBusinessRules _kutuphaneBusinessRules;

    public KutuphaneManager(IKutuphaneRepository kutuphaneRepository, KutuphaneBusinessRules kutuphaneBusinessRules)
    {
        _kutuphaneRepository = kutuphaneRepository;
        _kutuphaneBusinessRules = kutuphaneBusinessRules;
    }

    public async Task<Kutuphane?> GetAsync(
        Expression<Func<Kutuphane, bool>> predicate,
        Func<IQueryable<Kutuphane>, IIncludableQueryable<Kutuphane, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Kutuphane? kutuphane = await _kutuphaneRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return kutuphane;
    }

    public async Task<IPaginate<Kutuphane>?> GetListAsync(
        Expression<Func<Kutuphane, bool>>? predicate = null,
        Func<IQueryable<Kutuphane>, IOrderedQueryable<Kutuphane>>? orderBy = null,
        Func<IQueryable<Kutuphane>, IIncludableQueryable<Kutuphane, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Kutuphane> kutuphaneList = await _kutuphaneRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return kutuphaneList;
    }

    public async Task<Kutuphane> AddAsync(Kutuphane kutuphane)
    {
        Kutuphane addedKutuphane = await _kutuphaneRepository.AddAsync(kutuphane);

        return addedKutuphane;
    }

    public async Task<Kutuphane> UpdateAsync(Kutuphane kutuphane)
    {
        Kutuphane updatedKutuphane = await _kutuphaneRepository.UpdateAsync(kutuphane);

        return updatedKutuphane;
    }

    public async Task<Kutuphane> DeleteAsync(Kutuphane kutuphane, bool permanent = false)
    {
        Kutuphane deletedKutuphane = await _kutuphaneRepository.DeleteAsync(kutuphane);

        return deletedKutuphane;
    }
}
