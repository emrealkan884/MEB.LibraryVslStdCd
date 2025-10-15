using Application.Features.Nushalar.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Nushalar;

public class NushaManager : INushaService
{
    private readonly INushaRepository _nushaRepository;
    private readonly NushaBusinessRules _nushaBusinessRules;

    public NushaManager(INushaRepository nushaRepository, NushaBusinessRules nushaBusinessRules)
    {
        _nushaRepository = nushaRepository;
        _nushaBusinessRules = nushaBusinessRules;
    }

    public async Task<Nusha?> GetAsync(
        Expression<Func<Nusha, bool>> predicate,
        Func<IQueryable<Nusha>, IIncludableQueryable<Nusha, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Nusha? nusha = await _nushaRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return nusha;
    }

    public async Task<IPaginate<Nusha>?> GetListAsync(
        Expression<Func<Nusha, bool>>? predicate = null,
        Func<IQueryable<Nusha>, IOrderedQueryable<Nusha>>? orderBy = null,
        Func<IQueryable<Nusha>, IIncludableQueryable<Nusha, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Nusha> nushaList = await _nushaRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return nushaList;
    }

    public async Task<Nusha> AddAsync(Nusha nusha)
    {
        Nusha addedNusha = await _nushaRepository.AddAsync(nusha);

        return addedNusha;
    }

    public async Task<Nusha> UpdateAsync(Nusha nusha)
    {
        Nusha updatedNusha = await _nushaRepository.UpdateAsync(nusha);

        return updatedNusha;
    }

    public async Task<Nusha> DeleteAsync(Nusha nusha, bool permanent = false)
    {
        Nusha deletedNusha = await _nushaRepository.DeleteAsync(nusha);

        return deletedNusha;
    }
}
