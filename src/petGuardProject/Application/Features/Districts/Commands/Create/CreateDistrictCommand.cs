using Application.Features.Districts.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Districts.Commands.Create;

public class CreateDistrictCommand: IRequest<CreatedDistrictResponse>
{
    public Guid CityId { get; set; }
    public string Name { get; set; }

    public class CreateDistrictCommandHandler : IRequestHandler<CreateDistrictCommand, CreatedDistrictResponse>
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly DistrictBusinessRules _districtBusinessRules;
        private readonly IMapper _mapper;

        public CreateDistrictCommandHandler(IDistrictRepository districtRepository, DistrictBusinessRules districtBusinessRules, IMapper mapper)
        {
            _districtRepository = districtRepository;
            _districtBusinessRules = districtBusinessRules;
            _mapper = mapper;
        }

        public async Task<CreatedDistrictResponse> Handle(CreateDistrictCommand request, CancellationToken cancellationToken)
        {
            await _districtBusinessRules.DistrictNameCannotBeDuplicatedInCityWhenInserted(request.Name, request.CityId);

            District mappedDistrict = _mapper.Map<District>(request);

            await _districtRepository.AddAsync(mappedDistrict);

            CreatedDistrictResponse response = _mapper.Map<CreatedDistrictResponse>(mappedDistrict);

            return response;
        }
    }
}
