using Application.Features.Pets.Constants;
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

namespace Application.Features.Pets.Rules;

public class PetBusinessRules : BaseBusinessRules
{
    private readonly IPetRepository _petRepository;
    private readonly ILocalizationService _localizationService;

    public PetBusinessRules(IPetRepository petRepository, ILocalizationService localizationService)
    {
        _petRepository = petRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, PetsMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task PetExistsWhenSelected(Pet? pet)
    {
        if (pet is null)
            await throwBusinessException(PetsMessages.PetDontExists);
    }

    public async Task PetIdExistsWhenSelected(Guid id)
    {
        Pet? pet = await _petRepository.GetAsync(x => x.Id == id, enableTracking: false);
        await PetExistsWhenSelected(pet);
    }
}
