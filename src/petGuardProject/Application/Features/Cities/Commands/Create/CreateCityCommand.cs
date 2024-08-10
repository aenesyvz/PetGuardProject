using Application.Features.Cities.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cities.Commands.Create;


public class CreateCityCommand: IRequest<CreatedCityResponse>
{
    public int PlateCode { get; set; }
    public string Name { get; set; }


    public class CreatedCityCommandHandler : IRequestHandler<CreateCityCommand, CreatedCityResponse>
    {
        private readonly ICityRepository _cityRepository;
        private readonly CityBusinessRules _cityBusinessRules;
        private readonly IMapper _mapper;
        public CreatedCityCommandHandler(ICityRepository cityRepository,CityBusinessRules cityBusinessRules,IMapper mapper)
        {
            _cityRepository = cityRepository;
            _cityBusinessRules = cityBusinessRules;
            _mapper = mapper;
        }


        public async Task<CreatedCityResponse> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            await _cityBusinessRules.CityNameCanNotBeDuplicatedWhenInserted(request.Name);
            await _cityBusinessRules.CityPlateCodeCannotBeDuplicatedWhenInserted(request.PlateCode);

            City mappedCity = _mapper.Map<City>(request);
            City createdCity = await _cityRepository.AddAsync(mappedCity);

            CreatedCityResponse createdCityResponse = _mapper.Map<CreatedCityResponse>(createdCity);

            return createdCityResponse;
        }
    }
}