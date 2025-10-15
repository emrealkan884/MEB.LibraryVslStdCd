using Application.Features.OduncIslemler.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.OduncIslemleri;

public class OduncIslemiManager : IOduncIslemiService
{
    private readonly IOduncIslemiRepository _oduncIslemiRepository;
    private readonly OduncIslemiBusinessRules _oduncIslemiBusinessRules;

    public OduncIslemiManager(IOduncIslemiRepository oduncIslemiRepository, OduncIslemiBusinessRules oduncIslemiBusinessRules)
    {
        _oduncIslemiRepository = oduncIslemiRepository;
        _oduncIslemiBusinessRules = oduncIslemiBusinessRules;
    }

    public async Task<OduncIslemi?> GetAsync(
        Expression<Func<OduncIslemi, bool>> predicate,
        Func<IQueryable<OduncIslemi>, IIncludableQueryable<OduncIslemi, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        OduncIslemi? oduncIslemi = await _oduncIslemiRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return oduncIslemi;
    }

    public async Task<IPaginate<OduncIslemi>?> GetListAsync(
        Expression<Func<OduncIslemi, bool>>? predicate = null,
        Func<IQueryable<OduncIslemi>, IOrderedQueryable<OduncIslemi>>? orderBy = null,
        Func<IQueryable<OduncIslemi>, IIncludableQueryable<OduncIslemi, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<OduncIslemi> oduncIslemiList = await _oduncIslemiRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return oduncIslemiList;
    }

    public async Task<OduncIslemi> AddAsync(OduncIslemi oduncIslemi)
    {
        OduncIslemi addedOduncIslemi = await _oduncIslemiRepository.AddAsync(oduncIslemi);

        return addedOduncIslemi;
    }

    public async Task<OduncIslemi> UpdateAsync(OduncIslemi oduncIslemi)
    {
        OduncIslemi updatedOduncIslemi = await _oduncIslemiRepository.UpdateAsync(oduncIslemi);

        return updatedOduncIslemi;
    }

    public async Task<OduncIslemi> DeleteAsync(OduncIslemi oduncIslemi, bool permanent = false)
    {
        OduncIslemi deletedOduncIslemi = await _oduncIslemiRepository.DeleteAsync(oduncIslemi);

        return deletedOduncIslemi;
    }
}
