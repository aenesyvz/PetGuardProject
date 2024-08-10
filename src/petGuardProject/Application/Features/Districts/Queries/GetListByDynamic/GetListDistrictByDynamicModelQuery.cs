using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Districts.Queries.GetListByDynamic;

public class GetListDistrictByDynamicModelQuery : IRequest<GetListResponse<GetListDistrictByDynamicModelListItemDto>>
{
    public PageRequest PageRequest { get; set; }
    public DynamicQuery DynamicQuery { get; set; }

    public class GetListDistrictByDynamicModelQueryHandler : IRequestHandler<GetListDistrictByDynamicModelQuery, GetListResponse<GetListDistrictByDynamicModelListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IDistrictRepository _districtRepository;

        public GetListDistrictByDynamicModelQueryHandler(IMapper mapper, IDistrictRepository districtRepository)
        {
            _mapper = mapper;
            _districtRepository = districtRepository;
        }
        public async Task<GetListResponse<GetListDistrictByDynamicModelListItemDto>> Handle(GetListDistrictByDynamicModelQuery request, CancellationToken cancellationToken)
        {
            IPaginate<District> districts = await _districtRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                include: d => d.Include(d => d.City),
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize!
            );

            var mappedDistrictListModel = _mapper.Map<GetListResponse<GetListDistrictByDynamicModelListItemDto>>(districts);

            return mappedDistrictListModel;
        }
    }

}