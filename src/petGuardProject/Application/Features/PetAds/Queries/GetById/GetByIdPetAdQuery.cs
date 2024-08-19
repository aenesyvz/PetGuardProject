using Application.Features.PetAds.Rules;
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

namespace Application.Features.PetAds.Queries.GetById;
public class GetByIdPetAdQuery : IRequest<GetByIdPetAdResponse>
{
    public Guid Id { get; set; }

    public class GetByIdPetAdQueryHandler: IRequestHandler<GetByIdPetAdQuery, GetByIdPetAdResponse>
    {
        private readonly IPetAdRepository _petAdRepository;
        private readonly IMapper _mapper;
        private readonly PetAdBusinesRules _petAdBusinesRules;

        public GetByIdPetAdQueryHandler(IPetAdRepository petAdRepository, IMapper mapper, PetAdBusinesRules petAdBusinesRules)
        {
            _petAdRepository = petAdRepository;
            _mapper = mapper;
            _petAdBusinesRules = petAdBusinesRules;
        }

        public async Task<GetByIdPetAdResponse> Handle(GetByIdPetAdQuery request, CancellationToken cancellationToken)
        {
            PetAd? petAd = await _petAdRepository.GetAsync(predicate:x => x.Id == request.Id,
                                                           include: m => m.Include(m => m.PetOwner).Include(m => m.Pet).Include(m => m.City).Include(m => m.District),
                                                           enableTracking:true,
                                                           cancellationToken:cancellationToken);

            await _petAdBusinesRules.PetAdExistsWhenSelected(petAd);

            GetByIdPetAdResponse response = _mapper.Map<GetByIdPetAdResponse>(petAd);

            return response;
        }
    }
}
