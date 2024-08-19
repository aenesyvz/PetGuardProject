using Application.Features.PetAds.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PetAds.Queries.GetListByDynamic;
public class GetListPetAdByDynamicModelQuery : IRequest<GetListResponse<GetListPetAdByDynamicModelListItemDto>>
{
    public DynamicQuery DynamicQuery { get; set; }
    public PageRequest PageRequest { get; set; }

    public class GetListPetAdByDynamicModelQueryHandler : IRequestHandler<GetListPetAdByDynamicModelQuery, GetListResponse<GetListPetAdByDynamicModelListItemDto>>
    {
        private readonly IPetAdRepository _petAdRepository;
        private readonly IMapper _mapper;
        private readonly PetAdBusinesRules _petAdBusinesRules;

        public GetListPetAdByDynamicModelQueryHandler(IPetAdRepository petAdRepository, IMapper mapper, PetAdBusinesRules petAdBusinesRules)
        {
            _petAdRepository = petAdRepository;
            _mapper = mapper;
            _petAdBusinesRules = petAdBusinesRules;
        }

        public async Task<GetListResponse<GetListPetAdByDynamicModelListItemDto>> Handle(GetListPetAdByDynamicModelQuery request, CancellationToken cancellationToken)
        {
            IPaginate<PetAd> paginate = await _petAdRepository.GetListByDynamicAsync(
                    request.DynamicQuery,
                    include: m => m.Include(m => m.PetOwner).Include(m => m.Pet).Include(m => m.City).Include(m => m.District),
                    index: request.PageRequest.PageIndex,
                    size: request.PageRequest.PageSize,
                    cancellationToken:cancellationToken
                );

            GetListResponse<GetListPetAdByDynamicModelListItemDto> response = _mapper.Map<GetListResponse<GetListPetAdByDynamicModelListItemDto>>(paginate);

            return response;
        }
    }
}
