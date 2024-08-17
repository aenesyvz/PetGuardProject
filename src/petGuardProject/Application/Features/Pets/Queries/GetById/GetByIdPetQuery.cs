using Application.Features.Pets.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Pets.Queries.GetById;
public class GetByIdPetQuery : IRequest<GetByIdPetResponse>
{
    public Guid Id { get; set; }

    public class GetByIdPetQueryHandler : IRequestHandler<GetByIdPetQuery, GetByIdPetResponse>
    {
        private readonly IPetRepository _petRepository;
        private readonly IMapper _mapper;
        private readonly PetBusinessRules _petBusinessRules;

        public GetByIdPetQueryHandler(IPetRepository petRepository, IMapper mapper, PetBusinessRules petBusinessRules)
        {
            _petRepository = petRepository;
            _mapper = mapper;
            _petBusinessRules = petBusinessRules;
        }

        public async Task<GetByIdPetResponse> Handle(GetByIdPetQuery request, CancellationToken cancellationToken)
        {
            Pet? pet = await _petRepository.GetAsync(
                    predicate: x => x.Id == request.Id,
                    include: m => m.Include(m => m.PetOwner).Include(m => m.PetOwner.User),
                    cancellationToken:cancellationToken
                );

            await _petBusinessRules.PetExistsWhenSelected(pet);

            GetByIdPetResponse response = _mapper.Map<GetByIdPetResponse>(pet);

            return response;
        }
    }
}
