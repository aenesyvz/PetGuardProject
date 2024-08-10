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

namespace Application.Features.Cities.Commands.Delete;


public class DeleteCityCommand : IRequest<DeletedCityResponse>
{
    public Guid Id { get; set; }

    public class DeleteCommandHandler : IRequestHandler<DeleteCityCommand, DeletedCityResponse>
    {
        private readonly ICityRepository _cityRepository;
        private readonly CityBusinessRules _cityBusinessRules;
        private readonly IMapper _mapper;

        public DeleteCommandHandler(ICityRepository cityRepository, CityBusinessRules cityBusinessRules, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _cityBusinessRules = cityBusinessRules;
            _mapper = mapper;
        }

        public async Task<DeletedCityResponse> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            City? city = await _cityRepository.GetAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            await _cityBusinessRules.CityShouldExistWhenSelected(city);

            await _cityRepository.DeleteAsync(city);

            DeletedCityResponse response = _mapper.Map<DeletedCityResponse>(city);

            return response;
        }
    }
}