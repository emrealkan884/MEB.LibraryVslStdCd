using Application.Features.DeweySiniflamalari.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.DeweySiniflamalari;

public class DeweySiniflamaManager : IDeweySiniflamaService
{
    private readonly IDeweySiniflamaRepository _deweySiniflamaRepository;
    private readonly DeweySiniflamaBusinessRules _deweySiniflamaBusinessRules;

    public DeweySiniflamaManager(IDeweySiniflamaRepository deweySiniflamaRepository, DeweySiniflamaBusinessRules deweySiniflamaBusinessRules)
    {
        _deweySiniflamaRepository = deweySiniflamaRepository;
        _deweySiniflamaBusinessRules = deweySiniflamaBusinessRules;
    }

    public async Task<DeweySiniflama?> GetAsync(
        Expression<Func<DeweySiniflama, bool>> predicate,
        Func<IQueryable<DeweySiniflama>, IIncludableQueryable<DeweySiniflama, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        DeweySiniflama? deweySiniflama = await _deweySiniflamaRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return deweySiniflama;
    }

    public async Task<IPaginate<DeweySiniflama>?> GetListAsync(
        Expression<Func<DeweySiniflama, bool>>? predicate = null,
        Func<IQueryable<DeweySiniflama>, IOrderedQueryable<DeweySiniflama>>? orderBy = null,
        Func<IQueryable<DeweySiniflama>, IIncludableQueryable<DeweySiniflama, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<DeweySiniflama> deweySiniflamaList = await _deweySiniflamaRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return deweySiniflamaList;
    }

    public async Task<DeweySiniflama> AddAsync(DeweySiniflama deweySiniflama)
    {
        DeweySiniflama addedDeweySiniflama = await _deweySiniflamaRepository.AddAsync(deweySiniflama);

        return addedDeweySiniflama;
    }

    public async Task<DeweySiniflama> UpdateAsync(DeweySiniflama deweySiniflama)
    {
        DeweySiniflama updatedDeweySiniflama = await _deweySiniflamaRepository.UpdateAsync(deweySiniflama);

        return updatedDeweySiniflama;
    }

    public async Task<DeweySiniflama> DeleteAsync(DeweySiniflama deweySiniflama, bool permanent = false)
    {
        DeweySiniflama deletedDeweySiniflama = await _deweySiniflamaRepository.DeleteAsync(deweySiniflama);

        return deletedDeweySiniflama;
    }
}
