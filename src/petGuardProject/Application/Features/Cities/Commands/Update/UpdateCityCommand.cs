using Application.Features.Cities.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Cities.Commands.Update;

public class UpdateCityCommand : IRequest<UpdatedCityResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PlateCode { get; set; }

    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, UpdatedCityResponse>
    {
        private readonly ICityRepository _cityRepository;
        private readonly CityBusinessRules _cityBusinessRules;
        private readonly IMapper _mapper;

        public UpdateCityCommandHandler(ICityRepository cityRepository, CityBusinessRules cityBusinessRules, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _cityBusinessRules = cityBusinessRules;
            _mapper = mapper;
        }

        public async Task<UpdatedCityResponse> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            await _cityBusinessRules.CityIdShouldExistWhenSelected(request.Id);

            var mappedCity = _mapper.Map<City>(request);

            await _cityBusinessRules.CityNameCanNotBeDuplicatedWhenUpdated(mappedCity);
            await _cityBusinessRules.CityPlateCodeCannotBeDuplicatedWhenUpdated(mappedCity);

            await _cityRepository.UpdateAsync(mappedCity);

            var response = _mapper.Map<UpdatedCityResponse>(mappedCity);

            return response;

        }
    }
}