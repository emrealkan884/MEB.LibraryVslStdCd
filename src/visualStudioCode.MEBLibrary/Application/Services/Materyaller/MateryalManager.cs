using Application.Features.Materyaller.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Materyaller;

public class MateryalManager : IMateryalService
{
    private readonly IMateryalRepository _materyalRepository;
    private readonly MateryalBusinessRules _materyalBusinessRules;

    public MateryalManager(IMateryalRepository materyalRepository, MateryalBusinessRules materyalBusinessRules)
    {
        _materyalRepository = materyalRepository;
        _materyalBusinessRules = materyalBusinessRules;
    }

    public async Task<Materyal?> GetAsync(
        Expression<Func<Materyal, bool>> predicate,
        Func<IQueryable<Materyal>, IIncludableQueryable<Materyal, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Materyal? materyal = await _materyalRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return materyal;
    }

    public async Task<IPaginate<Materyal>?> GetListAsync(
        Expression<Func<Materyal, bool>>? predicate = null,
        Func<IQueryable<Materyal>, IOrderedQueryable<Materyal>>? orderBy = null,
        Func<IQueryable<Materyal>, IIncludableQueryable<Materyal, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Materyal> materyalList = await _materyalRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return materyalList;
    }

    public async Task<Materyal> AddAsync(Materyal materyal)
    {
        Materyal addedMateryal = await _materyalRepository.AddAsync(materyal);

        return addedMateryal;
    }

    public async Task<Materyal> UpdateAsync(Materyal materyal)
    {
        Materyal updatedMateryal = await _materyalRepository.UpdateAsync(materyal);

        return updatedMateryal;
    }

    public async Task<Materyal> DeleteAsync(Materyal materyal, bool permanent = false)
    {
        Materyal deletedMateryal = await _materyalRepository.DeleteAsync(materyal);

        return deletedMateryal;
    }
}
