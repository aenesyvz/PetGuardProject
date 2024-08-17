using Application.Features.Pets.Commands.Create;
using Application.Features.Pets.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Pets.Commands.Update;

public class UpdatePetCommand: IRequest<UpdatedPetResponse>
{
    public Guid Id { get; set; }
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


    public class UpdatePetCommandHandler : IRequestHandler<UpdatePetCommand,UpdatedPetResponse>
    {
        private readonly IPetRepository _petRepository;
        private readonly IMapper _mapper;
        private readonly PetBusinessRules _petBusinessRules;

        public UpdatePetCommandHandler(IPetRepository petRepository, IMapper mapper, PetBusinessRules petBusinessRules)
        {
            _petRepository = petRepository;
            _mapper = mapper;
            _petBusinessRules = petBusinessRules;
        }

        public async Task<UpdatedPetResponse> Handle(UpdatePetCommand request, CancellationToken cancellationToken)
        {
            Pet? pet = await _petRepository.GetAsync(x => x.Id == request.Id, enableTracking: false);

            await _petBusinessRules.PetExistsWhenSelected(pet);

            _mapper.Map(request, pet);

            await _petRepository.UpdateAsync(pet!);

            UpdatedPetResponse response = _mapper.Map<UpdatedPetResponse>(pet);

            return response;
        }
    }
}
