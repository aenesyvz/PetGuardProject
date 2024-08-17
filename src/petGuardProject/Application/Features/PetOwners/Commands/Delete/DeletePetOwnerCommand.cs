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

namespace Application.Features.PetOwners.Commands.Delete;

public class DeletePetOwnerCommand : IRequest<DeletedPetOwnerResponse>
{
    public Guid Id { get; set; }

    public class DeletePetOwnerCommandHandler : IRequestHandler<DeletePetOwnerCommand, DeletedPetOwnerResponse>
    {
        private readonly IPetOwnerRepository _petOwnerRepository;
        private readonly IMapper _mapper;
        private readonly PetOwnerBusinessRules _petOwnerBusinessRules;

        public DeletePetOwnerCommandHandler(IPetOwnerRepository petOwnerRepository, IMapper mapper, PetOwnerBusinessRules petOwnerBusinessRules)
        {
            _petOwnerRepository = petOwnerRepository;
            _mapper = mapper;
            _petOwnerBusinessRules = petOwnerBusinessRules;
        }

        public async Task<DeletedPetOwnerResponse> Handle(DeletePetOwnerCommand request, CancellationToken cancellationToken)
        {
            PetOwner? petOwner = await _petOwnerRepository.GetAsync(x => x.Id == request.Id, enableTracking: false);

            await _petOwnerBusinessRules.PetOwnerExistsWhenSelected(petOwner);

            await _petOwnerRepository.DeleteAsync(petOwner!);

            DeletedPetOwnerResponse response = _mapper.Map<DeletedPetOwnerResponse>(petOwner);

            return response;
        }
    }
}
