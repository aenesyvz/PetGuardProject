using Application.Features.Cities.Contants;
using Application.Features.PetOwners.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using NArchitecture.Core.Localization.Abstraction;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PetOwners.Rules;

public class PetOwnerBusinessRules : BaseBusinessRules
{
    private readonly IPetOwnerRepository _petOwnerRepository;
    private readonly ILocalizationService _localizationService;

    public PetOwnerBusinessRules(IPetOwnerRepository petOwnerRepository, ILocalizationService localizationService)
    {
        _petOwnerRepository = petOwnerRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, CitiesMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task PetOwnerExistsWhenSelected(PetOwner? petOwner)
    {
        if (petOwner is null)
            await throwBusinessException(PetOwnersMessages.PetOwnerDontExists);
    }

    public async Task PetOwnerIdExistsWhenSelected(Guid Id)
    {
        PetOwner? petOwner = await _petOwnerRepository.GetAsync(x => x.Id == Id, enableTracking: false);
        await PetOwnerExistsWhenSelected(petOwner);
    }
}
