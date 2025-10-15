using Application.Features.MateryalFormatDetaylari.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.MateryalFormatDetaylari;

public class MateryalFormatDetayManager : IMateryalFormatDetayService
{
    private readonly IMateryalFormatDetayRepository _materyalFormatDetayRepository;
    private readonly MateryalFormatDetayBusinessRules _materyalFormatDetayBusinessRules;

    public MateryalFormatDetayManager(IMateryalFormatDetayRepository materyalFormatDetayRepository, MateryalFormatDetayBusinessRules materyalFormatDetayBusinessRules)
    {
        _materyalFormatDetayRepository = materyalFormatDetayRepository;
        _materyalFormatDetayBusinessRules = materyalFormatDetayBusinessRules;
    }

    public async Task<MateryalFormatDetay?> GetAsync(
        Expression<Func<MateryalFormatDetay, bool>> predicate,
        Func<IQueryable<MateryalFormatDetay>, IIncludableQueryable<MateryalFormatDetay, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        MateryalFormatDetay? materyalFormatDetay = await _materyalFormatDetayRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return materyalFormatDetay;
    }

    public async Task<IPaginate<MateryalFormatDetay>?> GetListAsync(
        Expression<Func<MateryalFormatDetay, bool>>? predicate = null,
        Func<IQueryable<MateryalFormatDetay>, IOrderedQueryable<MateryalFormatDetay>>? orderBy = null,
        Func<IQueryable<MateryalFormatDetay>, IIncludableQueryable<MateryalFormatDetay, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<MateryalFormatDetay> materyalFormatDetayList = await _materyalFormatDetayRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return materyalFormatDetayList;
    }

    public async Task<MateryalFormatDetay> AddAsync(MateryalFormatDetay materyalFormatDetay)
    {
        MateryalFormatDetay addedMateryalFormatDetay = await _materyalFormatDetayRepository.AddAsync(materyalFormatDetay);

        return addedMateryalFormatDetay;
    }

    public async Task<MateryalFormatDetay> UpdateAsync(MateryalFormatDetay materyalFormatDetay)
    {
        MateryalFormatDetay updatedMateryalFormatDetay = await _materyalFormatDetayRepository.UpdateAsync(materyalFormatDetay);

        return updatedMateryalFormatDetay;
    }

    public async Task<MateryalFormatDetay> DeleteAsync(MateryalFormatDetay materyalFormatDetay, bool permanent = false)
    {
        MateryalFormatDetay deletedMateryalFormatDetay = await _materyalFormatDetayRepository.DeleteAsync(materyalFormatDetay);

        return deletedMateryalFormatDetay;
    }
}
