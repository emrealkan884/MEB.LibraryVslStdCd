using Application.Features.Etkinlikler.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Etkinlikler;

public class EtkinlikManager : IEtkinlikService
{
    private readonly IEtkinlikRepository _etkinlikRepository;
    private readonly EtkinlikBusinessRules _etkinlikBusinessRules;

    public EtkinlikManager(IEtkinlikRepository etkinlikRepository, EtkinlikBusinessRules etkinlikBusinessRules)
    {
        _etkinlikRepository = etkinlikRepository;
        _etkinlikBusinessRules = etkinlikBusinessRules;
    }

    public async Task<Etkinlik?> GetAsync(
        Expression<Func<Etkinlik, bool>> predicate,
        Func<IQueryable<Etkinlik>, IIncludableQueryable<Etkinlik, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Etkinlik? etkinlik = await _etkinlikRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return etkinlik;
    }

    public async Task<IPaginate<Etkinlik>?> GetListAsync(
        Expression<Func<Etkinlik, bool>>? predicate = null,
        Func<IQueryable<Etkinlik>, IOrderedQueryable<Etkinlik>>? orderBy = null,
        Func<IQueryable<Etkinlik>, IIncludableQueryable<Etkinlik, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Etkinlik> etkinlikList = await _etkinlikRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return etkinlikList;
    }

    public async Task<Etkinlik> AddAsync(Etkinlik etkinlik)
    {
        Etkinlik addedEtkinlik = await _etkinlikRepository.AddAsync(etkinlik);

        return addedEtkinlik;
    }

    public async Task<Etkinlik> UpdateAsync(Etkinlik etkinlik)
    {
        Etkinlik updatedEtkinlik = await _etkinlikRepository.UpdateAsync(etkinlik);

        return updatedEtkinlik;
    }

    public async Task<Etkinlik> DeleteAsync(Etkinlik etkinlik, bool permanent = false)
    {
        Etkinlik deletedEtkinlik = await _etkinlikRepository.DeleteAsync(etkinlik);

        return deletedEtkinlik;
    }
}
