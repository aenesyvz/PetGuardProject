using Application.Features.PetOwners.Rules;
using Application.Features.Pets.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Pets.Commands.Create;

public class CreatePetCommand: IRequest<CreatedPetResponse>
{
    public string Name { get; set; }
    public PetType PetType { get; set; }
    public string About { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? ImageUrl { get; set; }
    public bool Vaccinate { get; set; }
    public string Weight { get; set; }
    public string Height { get; set; }
    public Guid PetOwnerId { get; set; }

    public class CreatePetCommandHandler : IRequestHandler<CreatePetCommand, CreatedPetResponse>
    {
        private readonly IPetRepository _petRepository;
        private readonly IMapper _mapper;
        private readonly PetBusinessRules _petBusinessRules;
        private readonly PetOwnerBusinessRules _petOwnerBusinessRules;

        public CreatePetCommandHandler(IPetRepository petRepository, IMapper mapper, PetBusinessRules petBusinessRules,PetOwnerBusinessRules petOwnerBusinessRules)
        {
            _petRepository = petRepository;
            _mapper = mapper;
            _petBusinessRules = petBusinessRules;
            _petOwnerBusinessRules = petOwnerBusinessRules;
        }

        public async Task<CreatedPetResponse> Handle(CreatePetCommand request, CancellationToken cancellationToken)
        {
            await _petOwnerBusinessRules.PetOwnerIdExistsWhenSelected(request.PetOwnerId);

            Pet pet = _mapper.Map<Pet>(request);

            await _petRepository.AddAsync(pet);

            CreatedPetResponse response = _mapper.Map<CreatedPetResponse>(pet);

            return response;
        }
    }
}
