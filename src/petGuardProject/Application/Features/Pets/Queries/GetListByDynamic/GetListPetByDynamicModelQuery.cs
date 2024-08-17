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

namespace Application.Features.Pets.Queries.GetListByDynamic;


public class GetListPetByDynamicModelQuery : IRequest<GetListResponse<GetListPetByDynamicModelListItemDto>>
{
    public DynamicQuery DynamicQuery { get; set; }
    public PageRequest PageRequest { get; set; }


    public class GetListPetByDynamicModelQueryHandler : IRequestHandler<GetListPetByDynamicModelQuery, GetListResponse<GetListPetByDynamicModelListItemDto>>
    {
        private readonly IPetRepository _petRepository;
        private readonly IMapper _mapper;

        public async Task<GetListResponse<GetListPetByDynamicModelListItemDto>> Handle(GetListPetByDynamicModelQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Pet> paginate = await _petRepository.GetListByDynamicAsync(
                    request.DynamicQuery,
                    include: m => m.Include(m => m.PetOwner).Include(m => m.PetOwner.User),
                    index: request.PageRequest.PageIndex,
                    size:request.PageRequest.PageSize,
                    cancellationToken:cancellationToken
                );

            GetListResponse<GetListPetByDynamicModelListItemDto> response = _mapper.Map<GetListResponse<GetListPetByDynamicModelListItemDto>>(paginate);

            return response;
        }
    }
}