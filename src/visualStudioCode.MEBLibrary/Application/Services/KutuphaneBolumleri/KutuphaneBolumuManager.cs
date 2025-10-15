using Application.Features.KutuphaneBolumleri.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.KutuphaneBolumleri;

public class KutuphaneBolumuManager : IKutuphaneBolumuService
{
    private readonly IKutuphaneBolumuRepository _kutuphaneBolumuRepository;
    private readonly KutuphaneBolumuBusinessRules _kutuphaneBolumuBusinessRules;

    public KutuphaneBolumuManager(IKutuphaneBolumuRepository kutuphaneBolumuRepository, KutuphaneBolumuBusinessRules kutuphaneBolumuBusinessRules)
    {
        _kutuphaneBolumuRepository = kutuphaneBolumuRepository;
        _kutuphaneBolumuBusinessRules = kutuphaneBolumuBusinessRules;
    }

    public async Task<KutuphaneBolumu?> GetAsync(
        Expression<Func<KutuphaneBolumu, bool>> predicate,
        Func<IQueryable<KutuphaneBolumu>, IIncludableQueryable<KutuphaneBolumu, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        KutuphaneBolumu? kutuphaneBolumu = await _kutuphaneBolumuRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return kutuphaneBolumu;
    }

    public async Task<IPaginate<KutuphaneBolumu>?> GetListAsync(
        Expression<Func<KutuphaneBolumu, bool>>? predicate = null,
        Func<IQueryable<KutuphaneBolumu>, IOrderedQueryable<KutuphaneBolumu>>? orderBy = null,
        Func<IQueryable<KutuphaneBolumu>, IIncludableQueryable<KutuphaneBolumu, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<KutuphaneBolumu> kutuphaneBolumuList = await _kutuphaneBolumuRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return kutuphaneBolumuList;
    }

    public async Task<KutuphaneBolumu> AddAsync(KutuphaneBolumu kutuphaneBolumu)
    {
        KutuphaneBolumu addedKutuphaneBolumu = await _kutuphaneBolumuRepository.AddAsync(kutuphaneBolumu);

        return addedKutuphaneBolumu;
    }

    public async Task<KutuphaneBolumu> UpdateAsync(KutuphaneBolumu kutuphaneBolumu)
    {
        KutuphaneBolumu updatedKutuphaneBolumu = await _kutuphaneBolumuRepository.UpdateAsync(kutuphaneBolumu);

        return updatedKutuphaneBolumu;
    }

    public async Task<KutuphaneBolumu> DeleteAsync(KutuphaneBolumu kutuphaneBolumu, bool permanent = false)
    {
        KutuphaneBolumu deletedKutuphaneBolumu = await _kutuphaneBolumuRepository.DeleteAsync(kutuphaneBolumu);

        return deletedKutuphaneBolumu;
    }
}
