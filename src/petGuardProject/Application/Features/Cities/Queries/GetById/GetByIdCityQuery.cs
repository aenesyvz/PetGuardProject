using Application.Features.Cities.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Security.Contants;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Features.Cities.Contants.CitiesOperationClaims;

namespace Application.Features.Cities.Queries.GetById;

public class GetByIdCityQuery : IRequest<GetByIdCityResponse>
{
    public Guid Id { get; set; }

    public string[] Roles => new[] { GeneralOperationClaims.Admin, Read };

    public class GetByIdCityQueryHandler : IRequestHandler<GetByIdCityQuery, GetByIdCityResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICityRepository _cityRepository;
        private readonly CityBusinessRules _cityBusinessRules;

        public GetByIdCityQueryHandler(IMapper mapper, ICityRepository cityRepository, CityBusinessRules cityBusinessRules)
        {
            _mapper = mapper;
            _cityRepository = cityRepository;
            _cityBusinessRules = cityBusinessRules;
        }

        public async Task<GetByIdCityResponse> Handle(GetByIdCityQuery request, CancellationToken cancellationToken)
        {
            City? city = await _cityRepository.GetAsync(
                    predicate: p => p.Id.Equals(request.Id),
                    enableTracking: false,
                    cancellationToken:cancellationToken
                );

            await _cityBusinessRules.CityShouldExistWhenSelected(city);

            GetByIdCityResponse response = _mapper.Map<GetByIdCityResponse>(city);

            return response;
        }
    }
}