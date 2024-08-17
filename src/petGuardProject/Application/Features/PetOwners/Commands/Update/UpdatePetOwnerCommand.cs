using Application.Features.PetOwners.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PetOwners.Commands.Update;

public class UpdatePetOwnerCommand : IRequest<UpdatedPetOwnerResponse>
{
    public Guid Id { get; set; }
    public Guid CityId { get; set; }
    public Guid DistrcitId { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }

    
    public class UpdatePetOwnerCommandHandler : IRequestHandler<UpdatePetOwnerCommand, UpdatedPetOwnerResponse>
    {
        private readonly IPetOwnerRepository _petOwnerRepository;
        private readonly IMapper _mapper;
        private readonly PetOwnerBusinessRules _petOwnerBusinessRules;

        public UpdatePetOwnerCommandHandler(IPetOwnerRepository petOwnerRepository, IMapper mapper, PetOwnerBusinessRules petOwnerBusinessRules)
        {
            _petOwnerRepository = petOwnerRepository;
            _mapper = mapper;
            _petOwnerBusinessRules = petOwnerBusinessRules;
        }

        public async Task<UpdatedPetOwnerResponse> Handle(UpdatePetOwnerCommand request, CancellationToken cancellationToken)
        {
            PetOwner? petOwner = await _petOwnerRepository.GetAsync(x => x.Id == request.Id, enableTracking: false);

            await _petOwnerBusinessRules.PetOwnerExistsWhenSelected(petOwner);

            _mapper.Map(request, petOwner);

            await _petOwnerRepository.UpdateAsync(petOwner!);

            UpdatedPetOwnerResponse response = _mapper.Map<UpdatedPetOwnerResponse>(petOwner);

            return response;

        }
    }
}
