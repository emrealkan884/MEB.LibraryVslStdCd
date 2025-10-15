using Application.Features.OtoriteKayitlari.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.OtoriteKayitlari;

public class OtoriteKaydiManager : IOtoriteKaydiService
{
    private readonly IOtoriteKaydiRepository _otoriteKaydiRepository;
    private readonly OtoriteKaydiBusinessRules _otoriteKaydiBusinessRules;

    public OtoriteKaydiManager(IOtoriteKaydiRepository otoriteKaydiRepository, OtoriteKaydiBusinessRules otoriteKaydiBusinessRules)
    {
        _otoriteKaydiRepository = otoriteKaydiRepository;
        _otoriteKaydiBusinessRules = otoriteKaydiBusinessRules;
    }

    public async Task<OtoriteKaydi?> GetAsync(
        Expression<Func<OtoriteKaydi, bool>> predicate,
        Func<IQueryable<OtoriteKaydi>, IIncludableQueryable<OtoriteKaydi, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        OtoriteKaydi? otoriteKaydi = await _otoriteKaydiRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return otoriteKaydi;
    }

    public async Task<IPaginate<OtoriteKaydi>?> GetListAsync(
        Expression<Func<OtoriteKaydi, bool>>? predicate = null,
        Func<IQueryable<OtoriteKaydi>, IOrderedQueryable<OtoriteKaydi>>? orderBy = null,
        Func<IQueryable<OtoriteKaydi>, IIncludableQueryable<OtoriteKaydi, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<OtoriteKaydi> otoriteKaydiList = await _otoriteKaydiRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return otoriteKaydiList;
    }

    public async Task<OtoriteKaydi> AddAsync(OtoriteKaydi otoriteKaydi)
    {
        OtoriteKaydi addedOtoriteKaydi = await _otoriteKaydiRepository.AddAsync(otoriteKaydi);

        return addedOtoriteKaydi;
    }

    public async Task<OtoriteKaydi> UpdateAsync(OtoriteKaydi otoriteKaydi)
    {
        OtoriteKaydi updatedOtoriteKaydi = await _otoriteKaydiRepository.UpdateAsync(otoriteKaydi);

        return updatedOtoriteKaydi;
    }

    public async Task<OtoriteKaydi> DeleteAsync(OtoriteKaydi otoriteKaydi, bool permanent = false)
    {
        OtoriteKaydi deletedOtoriteKaydi = await _otoriteKaydiRepository.DeleteAsync(otoriteKaydi);

        return deletedOtoriteKaydi;
    }
}
