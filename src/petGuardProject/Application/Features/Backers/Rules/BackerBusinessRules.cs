using Application.Features.Backers.Constants;
using Application.Features.Cities.Contants;
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

namespace Application.Features.Backers.Rules;

public class BackerBusinessRules : BaseBusinessRules
{
    private readonly IBackerRepository _backerRepository;
    private readonly ILocalizationService _localizationService;

    public BackerBusinessRules(IBackerRepository backerRepository, ILocalizationService localizationService)
    {
        _backerRepository = backerRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, BackersMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task BackerShouldExistWhenSelected(Backer? backer)
    {
        if (backer is null)
            await throwBusinessException(BackersMessages.BackerDontExists);
    }

    public async Task BackerIdShouldExistWhenSelected(Guid id)
    {
        Backer? backer = await _backerRepository.GetAsync(x => x.Id == id, enableTracking: false);
        await BackerShouldExistWhenSelected(backer);
    }

}
