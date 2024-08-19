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

namespace Application.Features.PetAds.Commands.Update;

public class UpdatePetAdCommand : IRequest<UpdatedPetAdResponse>
{
    public Guid Id { get; set; }
    public Guid PetOwnerId { get; set; }
    public Guid PetId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AdStatus AdStatus { get; set; }
    public Guid CityId { get; set; }
    public Guid DistrictId { get; set; }


    public class UpdatePetAdCommandHandler : IRequestHandler<UpdatePetAdCommand, UpdatedPetAdResponse>
    {
        private readonly IPetAdRepository _petAdRepository;
        private readonly IMapper _mapper;
        private readonly PetAdBusinesRules _petAdBusinesRules;
        private readonly PetBusinessRules _petBusinessRules;
        private readonly PetOwnerBusinessRules _petOwnerBusinessRules;
        private readonly CityBusinessRules _cityBusinessRules;
        private readonly DistrictBusinessRules _districtBusinessRules;

        public UpdatePetAdCommandHandler(IPetAdRepository petAdRepository, IMapper mapper, PetAdBusinesRules petAdBusinesRules, PetBusinessRules petBusinessRules, PetOwnerBusinessRules petOwnerBusinessRules, CityBusinessRules cityBusinessRules, DistrictBusinessRules districtBusinessRules)
        {
            _petAdRepository = petAdRepository;
            _mapper = mapper;
            _petAdBusinesRules = petAdBusinesRules;
            _petBusinessRules = petBusinessRules;
            _petOwnerBusinessRules = petOwnerBusinessRules;
            _cityBusinessRules = cityBusinessRules;
            _districtBusinessRules = districtBusinessRules;
        }

        public async Task<UpdatedPetAdResponse> Handle(UpdatePetAdCommand request, CancellationToken cancellationToken)
        {
            PetAd? petAd = await _petAdRepository.GetAsync(predicate: x => x.Id == request.Id, enableTracking: false, cancellationToken: cancellationToken);
            
            await _petAdBusinesRules.PetAdExistsWhenSelected(petAd);
            await _petAdBusinesRules.PetAdStartDateIsBeforeEndDateAsync(request.StartDate, request.EndDate);
            await _petOwnerBusinessRules.PetOwnerIdExistsWhenSelected(request.PetOwnerId);
            await _petBusinessRules.PetIdExistsWhenSelected(request.PetId);
            await _cityBusinessRules.CityIdShouldExistWhenSelected(request.CityId);
            await _districtBusinessRules.DistrictIdShouldExistWhenSelected(request.DistrictId);

            _mapper.Map(request, petAd);

            await _petAdRepository.UpdateAsync(petAd!);


            UpdatedPetAdResponse response = _mapper.Map<UpdatedPetAdResponse>(petAd);

            return response;

            
        }
    }
}
