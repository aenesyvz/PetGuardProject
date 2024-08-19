using Application.Features.Cities.Rules;
using Application.Features.Districts.Rules;
using Application.Features.PetAds.Rules;
using Application.Features.PetOwners.Rules;
using Application.Features.Pets.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.PetAds.Commands.Create;

public class CreatePetAdCommand : IRequest<CreatedPetAdResponse>
{
    public Guid PetOwnerId { get; set; }
    public Guid PetId { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AdStatus AdStatus { get; set; }
    public Guid CityId { get; set; }
    public Guid DistrictId { get; set; }


    public class CreatePetAdCommandHandler : IRequestHandler<CreatePetAdCommand, CreatedPetAdResponse>
    {
        private readonly IPetAdRepository _petAdRepository;
        private readonly IMapper _mapper;
        private readonly PetAdBusinesRules _petAdBusinesRules;
        private readonly PetBusinessRules _petBusinessRules;
        private readonly PetOwnerBusinessRules _petOwnerBusinessRules;
        private readonly CityBusinessRules _cityBusinessRules;
        private readonly DistrictBusinessRules _districtBusinessRules;

        public CreatePetAdCommandHandler(IPetAdRepository petAdRepository, IMapper mapper, PetAdBusinesRules petAdBusinesRules, PetBusinessRules petBusinessRules, PetOwnerBusinessRules petOwnerBusinessRules, CityBusinessRules cityBusinessRules, DistrictBusinessRules districtBusinessRules)
        {
            _petAdRepository = petAdRepository;
            _mapper = mapper;
            _petAdBusinesRules = petAdBusinesRules;
            _petBusinessRules = petBusinessRules;
            _petOwnerBusinessRules = petOwnerBusinessRules;
            _cityBusinessRules = cityBusinessRules;
            _districtBusinessRules = districtBusinessRules;
        }

        public async Task<CreatedPetAdResponse> Handle(CreatePetAdCommand request, CancellationToken cancellationToken)
        {
            await _petAdBusinesRules.PetAdStartDateIsBeforeEndDateAsync(request.StartDate, request.EndDate);
            await _petOwnerBusinessRules.PetOwnerIdExistsWhenSelected(request.PetOwnerId);
            await _petBusinessRules.PetIdExistsWhenSelected(request.PetId);
            await _cityBusinessRules.CityIdShouldExistWhenSelected(request.CityId);
            await _districtBusinessRules.DistrictIdShouldExistWhenSelected(request.DistrictId);

            PetAd petAd = _mapper.Map<PetAd>(request);

            await _petAdRepository.AddAsync(petAd);

            CreatedPetAdResponse response = _mapper.Map<CreatedPetAdResponse>(petAd);

            return response;
        }
    }
}
