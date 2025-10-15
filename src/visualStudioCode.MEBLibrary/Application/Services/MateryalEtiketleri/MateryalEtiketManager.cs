using Application.Features.MateryalEtiketleri.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.MateryalEtiketleri;

public class MateryalEtiketManager : IMateryalEtiketService
{
    private readonly IMateryalEtiketRepository _materyalEtiketRepository;
    private readonly MateryalEtiketBusinessRules _materyalEtiketBusinessRules;

    public MateryalEtiketManager(IMateryalEtiketRepository materyalEtiketRepository, MateryalEtiketBusinessRules materyalEtiketBusinessRules)
    {
        _materyalEtiketRepository = materyalEtiketRepository;
        _materyalEtiketBusinessRules = materyalEtiketBusinessRules;
    }

    public async Task<MateryalEtiket?> GetAsync(
        Expression<Func<MateryalEtiket, bool>> predicate,
        Func<IQueryable<MateryalEtiket>, IIncludableQueryable<MateryalEtiket, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        MateryalEtiket? materyalEtiket = await _materyalEtiketRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return materyalEtiket;
    }

    public async Task<IPaginate<MateryalEtiket>?> GetListAsync(
        Expression<Func<MateryalEtiket, bool>>? predicate = null,
        Func<IQueryable<MateryalEtiket>, IOrderedQueryable<MateryalEtiket>>? orderBy = null,
        Func<IQueryable<MateryalEtiket>, IIncludableQueryable<MateryalEtiket, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<MateryalEtiket> materyalEtiketList = await _materyalEtiketRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return materyalEtiketList;
    }

    public async Task<MateryalEtiket> AddAsync(MateryalEtiket materyalEtiket)
    {
        MateryalEtiket addedMateryalEtiket = await _materyalEtiketRepository.AddAsync(materyalEtiket);

        return addedMateryalEtiket;
    }

    public async Task<MateryalEtiket> UpdateAsync(MateryalEtiket materyalEtiket)
    {
        MateryalEtiket updatedMateryalEtiket = await _materyalEtiketRepository.UpdateAsync(materyalEtiket);

        return updatedMateryalEtiket;
    }

    public async Task<MateryalEtiket> DeleteAsync(MateryalEtiket materyalEtiket, bool permanent = false)
    {
        MateryalEtiket deletedMateryalEtiket = await _materyalEtiketRepository.DeleteAsync(materyalEtiket);

        return deletedMateryalEtiket;
    }
}
