using Application.Features.Cities.Contants;
using Domain.Entities;

using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using NArchitecture.Core.Localization.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace Application.Features.Cities.Rules;

public class CityBusinessRules:BaseBusinessRules
{
    private readonly ICityRepository _cityRepository;
    private readonly ILocalizationService _localizationService;

    public CityBusinessRules(ICityRepository cityRepository,ILocalizationService localizationService)
    {
        _cityRepository = cityRepository;
        _localizationService = localizationService;
    }


    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, CitiesMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task CityShouldExistWhenSelected(City? city)
    {
        if(city is null)
            await throwBusinessException(CitiesMessages.CityDontExists);
    }

    public async Task CityIdShouldExistWhenSelected(Guid id)
    {
        City? city = await _cityRepository.GetAsync(x => x.Id == id, enableTracking: false);
        await CityShouldExistWhenSelected(city);
    }

    public async Task CityNameCanNotBeDuplicatedWhenInserted(string name)
    {
        City? result = await _cityRepository.GetAsync(x => x.Name.ToLower() == name.ToLower());
        if (result != null)
            await throwBusinessException(CitiesMessages.CityNameAlreadyExists);
    }

    public async Task CityNameCanNotBeDuplicatedWhenUpdated(City city)
    {
        City? result = await _cityRepository.GetAsync(x => x.Id != city.Id && x.Name.ToLower() == city.Name.ToLower());

        if (result != null)
            await throwBusinessException(CitiesMessages.CityNameAlreadyExists);
    }

    public async Task CityPlateCodeCannotBeDuplicatedWhenInserted(int plateCode)
    {
        City? result = await _cityRepository.GetAsync(x => x.PlateCode == plateCode, enableTracking: false);

        if(result is not null)
        {
            await throwBusinessException(CitiesMessages.CityPlateAlreadyExists);
        }
    }

    public async Task CityPlateCodeCannotBeDuplicatedWhenUpdated(City city)
    {
        City? result = await _cityRepository.GetAsync(x => x.Id != city.Id && x.PlateCode == city.PlateCode);

        if(result is not null)
        {
            await throwBusinessException(CitiesMessages.CityPlateAlreadyExists);
        }
    }
}
