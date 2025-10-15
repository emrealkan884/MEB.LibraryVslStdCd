using Application.Features.Yazarlar.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Yazarlar;

public class YazarManager : IYazarService
{
    private readonly IYazarRepository _yazarRepository;
    private readonly YazarBusinessRules _yazarBusinessRules;

    public YazarManager(IYazarRepository yazarRepository, YazarBusinessRules yazarBusinessRules)
    {
        _yazarRepository = yazarRepository;
        _yazarBusinessRules = yazarBusinessRules;
    }

    public async Task<Yazar?> GetAsync(
        Expression<Func<Yazar, bool>> predicate,
        Func<IQueryable<Yazar>, IIncludableQueryable<Yazar, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Yazar? yazar = await _yazarRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return yazar;
    }

    public async Task<IPaginate<Yazar>?> GetListAsync(
        Expression<Func<Yazar, bool>>? predicate = null,
        Func<IQueryable<Yazar>, IOrderedQueryable<Yazar>>? orderBy = null,
        Func<IQueryable<Yazar>, IIncludableQueryable<Yazar, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Yazar> yazarList = await _yazarRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return yazarList;
    }

    public async Task<Yazar> AddAsync(Yazar yazar)
    {
        Yazar addedYazar = await _yazarRepository.AddAsync(yazar);

        return addedYazar;
    }

    public async Task<Yazar> UpdateAsync(Yazar yazar)
    {
        Yazar updatedYazar = await _yazarRepository.UpdateAsync(yazar);

        return updatedYazar;
    }

    public async Task<Yazar> DeleteAsync(Yazar yazar, bool permanent = false)
    {
        Yazar deletedYazar = await _yazarRepository.DeleteAsync(yazar);

        return deletedYazar;
    }
}
