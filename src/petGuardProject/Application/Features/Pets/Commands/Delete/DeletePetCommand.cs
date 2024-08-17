using Application.Features.Pets.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Pets.Commands.Delete;

public class DeletePetCommand : IRequest<DeletedPetResponse>
{
    public Guid Id { get; set; }


    public class DeletePetCommandHandler : IRequestHandler<DeletePetCommand, DeletedPetResponse>
    {
        private readonly IPetRepository _petRepository;
        private readonly IMapper _mapper;
        private readonly PetBusinessRules _petBusinessRules;

        public DeletePetCommandHandler(IPetRepository petRepository, IMapper mapper, PetBusinessRules petBusinessRules)
        {
            _petRepository = petRepository;
            _mapper = mapper;
            _petBusinessRules = petBusinessRules;
        }

        public async Task<DeletedPetResponse> Handle(DeletePetCommand request, CancellationToken cancellationToken)
        {
            Pet? pet = await _petRepository.GetAsync(x => x.Id == request.Id);

            await _petBusinessRules.PetExistsWhenSelected(pet);

            await _petRepository.DeleteAsync(pet!);

            DeletedPetResponse response = _mapper.Map<DeletedPetResponse>(pet);

            return response;
        }
    }
}
