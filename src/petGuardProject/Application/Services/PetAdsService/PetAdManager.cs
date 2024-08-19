using Application.Features.PetAds.Rules;
using Application.Services.Repositories;
using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.PetAdsService;

public class PetAdManager : IPetAdService
{
    private readonly IPetAdRepository _petAdRepository;
    private readonly PetAdBusinesRules _petAdBusinessRules;

    public PetAdManager(IPetAdRepository PetAdRepository, PetAdBusinesRules cityBusinessRules)
    {
        _petAdRepository = PetAdRepository;
        _petAdBusinessRules = cityBusinessRules;
    }

    public async Task<PetAd?> GetAsync(
        Expression<Func<PetAd, bool>> predicate,
        Func<IQueryable<PetAd>, IIncludableQueryable<PetAd, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        PetAd? city = await _petAdRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return city;
    }

    public async Task<IPaginate<PetAd>?> GetListAsync(
        Expression<Func<PetAd, bool>>? predicate = null,
        Func<IQueryable<PetAd>, IOrderedQueryable<PetAd>>? orderBy = null,
        Func<IQueryable<PetAd>, IIncludableQueryable<PetAd, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<PetAd> PetAdList = await _petAdRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return PetAdList;
    }

    public async Task<PetAd> AddAsync(PetAd district)
    {

        PetAd addedPetAd = await _petAdRepository.AddAsync(district);

        return addedPetAd;
    }

    public async Task<PetAd> UpdateAsync(PetAd district)
    {


        PetAd updatedPetAd = await _petAdRepository.UpdateAsync(district);

        return updatedPetAd;
    }

    public async Task<PetAd> DeleteAsync(PetAd district, bool permanent = false)
    {
        PetAd deletedPetAd = await _petAdRepository.DeleteAsync(district);

        return deletedPetAd;
    }
}