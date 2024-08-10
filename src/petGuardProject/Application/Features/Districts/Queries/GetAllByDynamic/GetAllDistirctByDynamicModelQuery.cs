using Application.Services.Repositories;
using AutoMapper;
using Core.Persistence.Dynamic;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Districts.Queries.GetAllByDynamic;

public class GetAllDistrictByDynamicModelQuery : IRequest<IList<GetAllDistrictByDynamicModelListItemDto>>
{
    public DynamicQuery DynamicQuery { get; set; }

    public class GetListDistirctWithOutPaginationByDynamicModelQueryHandler : IRequestHandler<GetAllDistrictByDynamicModelQuery, IList<GetAllDistrictByDynamicModelListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IDistrictRepository _districtRepository;

        public GetListDistirctWithOutPaginationByDynamicModelQueryHandler(IMapper mapper, IDistrictRepository districtRepository)
        {
            _mapper = mapper;
            _districtRepository = districtRepository;
        }

        public async Task<IList<GetAllDistrictByDynamicModelListItemDto>> Handle(GetAllDistrictByDynamicModelQuery request, CancellationToken cancellationToken)
        {
            IList<District> districts = await _districtRepository.GetListByDynamicWithOutPaginationAsync(
                    request.DynamicQuery,
                    include: m => m.Include(m => m.City)
                );

            IList<GetAllDistrictByDynamicModelListItemDto> response = _mapper.Map<IList<GetAllDistrictByDynamicModelListItemDto>>(districts);

            return response;
        }
    }
}