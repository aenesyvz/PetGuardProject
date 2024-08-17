using Application.Features.PetOwners.Rules;
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

namespace Application.Features.PetOwners.Queries.GetById;
public class GetByIdPetOwnerQuery: IRequest<GetByIdPetOwnerResponse>
{
    public Guid Id { get; set; }


    public class GetByIdPetOwnerQueryHandler : IRequestHandler<GetByIdPetOwnerQuery, GetByIdPetOwnerResponse>
    {
        private readonly IPetOwnerRepository _petOwnerRepository;
        private readonly IMapper _mapper;
        private readonly PetOwnerBusinessRules _petOwnerBusinessRules;

        public GetByIdPetOwnerQueryHandler(IPetOwnerRepository petOwnerRepository, IMapper mapper, PetOwnerBusinessRules petOwnerBusinessRules)
        {
            _petOwnerRepository = petOwnerRepository;
            _mapper = mapper;
            _petOwnerBusinessRules = petOwnerBusinessRules;
        }

        public async Task<GetByIdPetOwnerResponse> Handle(GetByIdPetOwnerQuery request, CancellationToken cancellationToken)
        {
            PetOwner? petOwner = await _petOwnerRepository.GetAsync(
                    predicate: x => x.Id == request.Id,
                    include: m => m.Include(m => m.User).Include(m => m.City).Include(m => m.District),
                    cancellationToken:cancellationToken
                );

            await _petOwnerBusinessRules.PetOwnerExistsWhenSelected(petOwner);

            GetByIdPetOwnerResponse response = _mapper.Map<GetByIdPetOwnerResponse>(petOwner);

            return response;
        }
    }
}
