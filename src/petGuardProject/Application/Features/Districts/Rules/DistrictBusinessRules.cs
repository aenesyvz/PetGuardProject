using Application.Features.Cities.Contants;
using Application.Features.Districts.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using NArchitecture.Core.Localization.Abstraction;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Districts.Rules;

public class DistrictBusinessRules : BaseBusinessRules
{
    private readonly IDistrictRepository _districtRepository;
    private readonly ILocalizationService _localizationService;

    public DistrictBusinessRules(IDistrictRepository districtRepository, ILocalizationService localizationService)
    {
        _districtRepository = districtRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, DistrictsMessages.SectionName);
        throw new BusinessException(message);
    }


    public async Task DistrictShouldExistWhenSelected(District? district)
    {
        if (district is null)
            await throwBusinessException(DistrictsMessages.DistrictDontExists);
    }

    public async Task DistrictIdShouldExistWhenSelected(Guid id)
    {
        District? district = await _districtRepository.GetAsync(x => x.Id == id, enableTracking: false);
        await DistrictShouldExistWhenSelected(district);
    }

    public async Task DistrictNameCannotBeDuplicatedInCityWhenInserted(string name,Guid cityId)
    {
        District? district = await _districtRepository.GetAsync(x => x.CityId == cityId && x.Name.ToLower().Equals(name.ToLower()));

        if(district is not null)
        {
            await throwBusinessException(DistrictsMessages.DistrictNameAlreadyExists);
        }
    }

    public async Task DistrictNameCannotBeDuplicatedInCityWhenUpdated(District district)
    {
        District? result = await _districtRepository.GetAsync(x => x.Id != district.Id && x.CityId == district.CityId && x.Name.ToLower().Equals(district.Name.ToLower()));
        
        if(result is not null)
        {
            await throwBusinessException(DistrictsMessages.DistrictNameAlreadyExists);
        }
    }
}
