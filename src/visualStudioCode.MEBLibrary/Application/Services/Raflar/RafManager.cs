using Application.Features.Raflar.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Raflar;

public class RafManager : IRafService
{
    private readonly IRafRepository _rafRepository;
    private readonly RafBusinessRules _rafBusinessRules;

    public RafManager(IRafRepository rafRepository, RafBusinessRules rafBusinessRules)
    {
        _rafRepository = rafRepository;
        _rafBusinessRules = rafBusinessRules;
    }

    public async Task<Raf?> GetAsync(
        Expression<Func<Raf, bool>> predicate,
        Func<IQueryable<Raf>, IIncludableQueryable<Raf, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Raf? raf = await _rafRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return raf;
    }

    public async Task<IPaginate<Raf>?> GetListAsync(
        Expression<Func<Raf, bool>>? predicate = null,
        Func<IQueryable<Raf>, IOrderedQueryable<Raf>>? orderBy = null,
        Func<IQueryable<Raf>, IIncludableQueryable<Raf, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Raf> rafList = await _rafRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return rafList;
    }

    public async Task<Raf> AddAsync(Raf raf)
    {
        Raf addedRaf = await _rafRepository.AddAsync(raf);

        return addedRaf;
    }

    public async Task<Raf> UpdateAsync(Raf raf)
    {
        Raf updatedRaf = await _rafRepository.UpdateAsync(raf);

        return updatedRaf;
    }

    public async Task<Raf> DeleteAsync(Raf raf, bool permanent = false)
    {
        Raf deletedRaf = await _rafRepository.DeleteAsync(raf);

        return deletedRaf;
    }
}
