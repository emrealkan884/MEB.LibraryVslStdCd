using Application.Features.YeniKatalogTalepleri.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.YeniKatalogTalepleri;

public class YeniKatalogTalebiManager : IYeniKatalogTalebiService
{
    private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
    private readonly YeniKatalogTalebiBusinessRules _yeniKatalogTalebiBusinessRules;

    public YeniKatalogTalebiManager(IYeniKatalogTalebiRepository yeniKatalogTalebiRepository, YeniKatalogTalebiBusinessRules yeniKatalogTalebiBusinessRules)
    {
        _yeniKatalogTalebiRepository = yeniKatalogTalebiRepository;
        _yeniKatalogTalebiBusinessRules = yeniKatalogTalebiBusinessRules;
    }

    public async Task<YeniKatalogTalebi?> GetAsync(
        Expression<Func<YeniKatalogTalebi, bool>> predicate,
        Func<IQueryable<YeniKatalogTalebi>, IIncludableQueryable<YeniKatalogTalebi, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        YeniKatalogTalebi? yeniKatalogTalebi = await _yeniKatalogTalebiRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return yeniKatalogTalebi;
    }

    public async Task<IPaginate<YeniKatalogTalebi>?> GetListAsync(
        Expression<Func<YeniKatalogTalebi, bool>>? predicate = null,
        Func<IQueryable<YeniKatalogTalebi>, IOrderedQueryable<YeniKatalogTalebi>>? orderBy = null,
        Func<IQueryable<YeniKatalogTalebi>, IIncludableQueryable<YeniKatalogTalebi, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<YeniKatalogTalebi> yeniKatalogTalebiList = await _yeniKatalogTalebiRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return yeniKatalogTalebiList;
    }

    public async Task<YeniKatalogTalebi> AddAsync(YeniKatalogTalebi yeniKatalogTalebi)
    {
        YeniKatalogTalebi addedYeniKatalogTalebi = await _yeniKatalogTalebiRepository.AddAsync(yeniKatalogTalebi);

        return addedYeniKatalogTalebi;
    }

    public async Task<YeniKatalogTalebi> UpdateAsync(YeniKatalogTalebi yeniKatalogTalebi)
    {
        YeniKatalogTalebi updatedYeniKatalogTalebi = await _yeniKatalogTalebiRepository.UpdateAsync(yeniKatalogTalebi);

        return updatedYeniKatalogTalebi;
    }

    public async Task<YeniKatalogTalebi> DeleteAsync(YeniKatalogTalebi yeniKatalogTalebi, bool permanent = false)
    {
        YeniKatalogTalebi deletedYeniKatalogTalebi = await _yeniKatalogTalebiRepository.DeleteAsync(yeniKatalogTalebi);

        return deletedYeniKatalogTalebi;
    }
}
