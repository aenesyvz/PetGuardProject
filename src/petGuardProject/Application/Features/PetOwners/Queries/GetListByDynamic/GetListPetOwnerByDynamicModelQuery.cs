using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PetOwners.Queries.GetListByDynamic;

public class GetListPetOwnerByDynamicModelQuery : IRequest<GetListResponse<GetListPetOwnerByDynamicModelListItemDto>>
{
    public DynamicQuery DynamicQuery { get; set; }
    public PageRequest PageRequest { get; set; }

    public class GetListPetOwnerByDynamicModelQueryHandler : IRequestHandler<GetListPetOwnerByDynamicModelQuery, GetListResponse<GetListPetOwnerByDynamicModelListItemDto>>
    {
        private readonly IPetOwnerRepository _petOwnerRepository;
        private readonly IMapper _mapper;

        public GetListPetOwnerByDynamicModelQueryHandler(IPetOwnerRepository petOwnerRepository, IMapper mapper)
        {
            _petOwnerRepository = petOwnerRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListPetOwnerByDynamicModelListItemDto>> Handle(GetListPetOwnerByDynamicModelQuery request, CancellationToken cancellationToken)
        {
            IPaginate<PetOwner> paginate = await _petOwnerRepository.GetListByDynamicAsync(
                    request.DynamicQuery,
                    include: m => m.Include(m => m.User).Include(m => m.City).Include(m => m.District),
                    index: request.PageRequest.PageIndex,
                    size: request.PageRequest.PageSize,
                    cancellationToken:cancellationToken
                );

            GetListResponse<GetListPetOwnerByDynamicModelListItemDto> response = _mapper.Map<GetListResponse<GetListPetOwnerByDynamicModelListItemDto>>(paginate);

            return response;
        }
    }
}