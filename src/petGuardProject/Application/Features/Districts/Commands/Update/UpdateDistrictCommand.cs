using Application.Features.Districts.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Districts.Commands.Update;

public class UpdateDistrictCommand : IRequest<UpdatedDistrictResponse>
{
    public Guid Id { get; set; }
    public Guid CityId { get; set; }
    public string Name { get; set; }

    public class UpdateDistrictCommandHandler : IRequestHandler<UpdateDistrictCommand, UpdatedDistrictResponse>
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly DistrictBusinessRules _districtBusinessRules;
        private readonly IMapper _mapper;

        public UpdateDistrictCommandHandler(IDistrictRepository districtRepository, DistrictBusinessRules districtBusinessRules, IMapper mapper)
        {
            _districtRepository = districtRepository;
            _districtBusinessRules = districtBusinessRules;
            _mapper = mapper;
        }

        public async Task<UpdatedDistrictResponse> Handle(UpdateDistrictCommand request, CancellationToken cancellationToken)
        {
            await _districtBusinessRules.DistrictIdShouldExistWhenSelected(request.Id);

            District mappedCity = _mapper.Map<District>(request);

            await _districtBusinessRules.DistrictNameCannotBeDuplicatedInCityWhenUpdated(mappedCity);

            await _districtRepository.UpdateAsync(mappedCity);

            UpdatedDistrictResponse response = _mapper.Map<UpdatedDistrictResponse>(mappedCity);

            return response;
        }
    }
}