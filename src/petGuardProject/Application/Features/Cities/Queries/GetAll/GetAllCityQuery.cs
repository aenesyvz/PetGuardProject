using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Caching;
using Core.Security.Contants;
using Domain.Entities;
using MediatR;
using static Application.Features.Cities.Contants.CitiesOperationClaims;

namespace Application.Features.Cities.Queries.GetAll;

public class GetAllCityQuery : IRequest<IList<GetAllCityListItemDto>>, ICachableRequest
{
    public string[] Roles => new[] { GeneralOperationClaims.Admin, Read };

    public bool BypassCache { get; }
    public string CacheKey => $"GetListCitiesWithOutPagination";
    public string CacheGroupKey => "GetCitiesWithOutPagination";
    public TimeSpan? SlidingExpiration { get; }


    public class GetListCityWithOutPaginationQueryHandler : IRequestHandler<GetAllCityQuery, IList<GetAllCityListItemDto>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public GetListCityWithOutPaginationQueryHandler(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }
        public async Task<IList<GetAllCityListItemDto>> Handle(GetAllCityQuery request, CancellationToken cancellationToken)
        {
            IList<City> cities = await _cityRepository.GetListWithOutPaginationAsync(
               orderBy: c => c.OrderBy(c => c.PlateCode),
               cancellationToken: cancellationToken
            );

            IList<GetAllCityListItemDto> response = _mapper.Map<IList<GetAllCityListItemDto>>(cities);

            return response;

        }
    }
}