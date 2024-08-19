using Application.Features.Cities.Contants;
using Application.Features.PetAds.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using NArchitecture.Core.Localization.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PetAds.Rules;

public class PetAdBusinesRules : BaseBusinessRules
{
    private readonly IPetAdRepository _petAdRepository;
    private readonly ILocalizationService _localizationService;

    public PetAdBusinesRules(IPetAdRepository petAdRepository, ILocalizationService localizationService)
    {
        _petAdRepository = petAdRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, PetAdsMessages.SectionName);
        throw new BusinessException(message);
    }
 
    public async Task PetAdExistsWhenSelected(PetAd? petAd)
    {
        if (petAd is null)
            await throwBusinessException(PetAdsMessages.PetAdsDontExists);
    }

    public async Task PetAdIdExistsWhenSelected(Guid id)
    {
        PetAd? petAd = await _petAdRepository.GetAsync(predicate: x => x.Id == id, enableTracking: false);
        await PetAdExistsWhenSelected(petAd);
    }

    public async Task PetAdStartDateIsBeforeEndDateAsync(DateTime startDate, DateTime endDate)
    {
        if (startDate.ToUniversalTime() <= endDate.ToUniversalTime())
            await throwBusinessException(PetAdsMessages.StartDateMustBeBeforeEndDate);
    }
}
