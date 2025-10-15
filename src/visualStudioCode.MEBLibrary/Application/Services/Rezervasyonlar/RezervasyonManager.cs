using Application.Features.Rezervasyonlar.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Rezervasyonlar;

public class RezervasyonManager : IRezervasyonService
{
    private readonly IRezervasyonRepository _rezervasyonRepository;
    private readonly RezervasyonBusinessRules _rezervasyonBusinessRules;

    public RezervasyonManager(IRezervasyonRepository rezervasyonRepository, RezervasyonBusinessRules rezervasyonBusinessRules)
    {
        _rezervasyonRepository = rezervasyonRepository;
        _rezervasyonBusinessRules = rezervasyonBusinessRules;
    }

    public async Task<Rezervasyon?> GetAsync(
        Expression<Func<Rezervasyon, bool>> predicate,
        Func<IQueryable<Rezervasyon>, IIncludableQueryable<Rezervasyon, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Rezervasyon? rezervasyon = await _rezervasyonRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return rezervasyon;
    }

    public async Task<IPaginate<Rezervasyon>?> GetListAsync(
        Expression<Func<Rezervasyon, bool>>? predicate = null,
        Func<IQueryable<Rezervasyon>, IOrderedQueryable<Rezervasyon>>? orderBy = null,
        Func<IQueryable<Rezervasyon>, IIncludableQueryable<Rezervasyon, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Rezervasyon> rezervasyonList = await _rezervasyonRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return rezervasyonList;
    }

    public async Task<Rezervasyon> AddAsync(Rezervasyon rezervasyon)
    {
        Rezervasyon addedRezervasyon = await _rezervasyonRepository.AddAsync(rezervasyon);

        return addedRezervasyon;
    }

    public async Task<Rezervasyon> UpdateAsync(Rezervasyon rezervasyon)
    {
        Rezervasyon updatedRezervasyon = await _rezervasyonRepository.UpdateAsync(rezervasyon);

        return updatedRezervasyon;
    }

    public async Task<Rezervasyon> DeleteAsync(Rezervasyon rezervasyon, bool permanent = false)
    {
        Rezervasyon deletedRezervasyon = await _rezervasyonRepository.DeleteAsync(rezervasyon);

        return deletedRezervasyon;
    }
}
