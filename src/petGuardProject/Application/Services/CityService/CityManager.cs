using Application.Features.Cities.Rules;
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

namespace Application.Services.CityService;




public class CityManager : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly CityBusinessRules _cityBusinessRules;

    public CityManager(ICityRepository CityRepository, CityBusinessRules cityBusinessRules)
    {
        _cityRepository = CityRepository;
        _cityBusinessRules = cityBusinessRules;
    }

    public async Task<City?> GetAsync(
        Expression<Func<City, bool>> predicate,
        Func<IQueryable<City>, IIncludableQueryable<City, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        City? city = await _cityRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return city;
    }

    public async Task<IPaginate<City>?> GetListAsync(
        Expression<Func<City, bool>>? predicate = null,
        Func<IQueryable<City>, IOrderedQueryable<City>>? orderBy = null,
        Func<IQueryable<City>, IIncludableQueryable<City, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<City> CityList = await _cityRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return CityList;
    }

    public async Task<City> AddAsync(City city)
    {
        await _cityBusinessRules.CityPlateCodeCannotBeDuplicatedWhenInserted(city.PlateCode);
        await _cityBusinessRules.CityNameCanNotBeDuplicatedWhenInserted(city.Name);

        City addedCity = await _cityRepository.AddAsync(city);

        return addedCity;
    }

    public async Task<City> UpdateAsync(City city)
    {
        await _cityBusinessRules.CityNameCanNotBeDuplicatedWhenUpdated(city);
        await _cityBusinessRules.CityPlateCodeCannotBeDuplicatedWhenUpdated(city);

        City updatedCity = await _cityRepository.UpdateAsync(city);

        return updatedCity;
    }

    public async Task<City> DeleteAsync(City city, bool permanent = false)
    {
        City deletedCity = await _cityRepository.DeleteAsync(city);

        return deletedCity;
    }
}