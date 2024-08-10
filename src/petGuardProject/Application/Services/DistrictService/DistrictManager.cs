using Application.Features.Cities.Rules;
using Application.Features.Districts.Rules;
using Application.Services.Repositories;
using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.DistrictService;




public class DistrictManager : IDistrictService
{
    private readonly IDistrictRepository _districtRepository;
    private readonly DistrictBusinessRules _districtBusinessRules;

    public DistrictManager(IDistrictRepository DistrictRepository, DistrictBusinessRules cityBusinessRules)
    {
        _districtRepository = DistrictRepository;
        _districtBusinessRules = cityBusinessRules;
    }

    public async Task<District?> GetAsync(
        Expression<Func<District, bool>> predicate,
        Func<IQueryable<District>, IIncludableQueryable<District, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        District? city = await _districtRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return city;
    }

    public async Task<IPaginate<District>?> GetListAsync(
        Expression<Func<District, bool>>? predicate = null,
        Func<IQueryable<District>, IOrderedQueryable<District>>? orderBy = null,
        Func<IQueryable<District>, IIncludableQueryable<District, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<District> DistrictList = await _districtRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return DistrictList;
    }

    public async Task<District> AddAsync(District district)
    {
        await _districtBusinessRules.DistrictNameCannotBeDuplicatedInCityWhenInserted(district.Name,district.CityId);

        District addedDistrict = await _districtRepository.AddAsync(district);

        return addedDistrict;
    }

    public async Task<District> UpdateAsync(District district)
    {
        await _districtBusinessRules.DistrictNameCannotBeDuplicatedInCityWhenUpdated(district);

        District updatedDistrict = await _districtRepository.UpdateAsync(district);

        return updatedDistrict;
    }

    public async Task<District> DeleteAsync(District district, bool permanent = false)
    {
        District deletedDistrict = await _districtRepository.DeleteAsync(district);

        return deletedDistrict;
    }
}