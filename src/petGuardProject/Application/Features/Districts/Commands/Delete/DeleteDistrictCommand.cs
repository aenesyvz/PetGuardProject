using Application.Features.Districts.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Districts.Commands.Delete;

public class DeleteDistrictCommand:IRequest<DeletedDistrictResponse>
{
    public Guid Id { get; set; }

    public class DeleteDistrictCommandHandler : IRequestHandler<DeleteDistrictCommand, DeletedDistrictResponse>
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly DistrictBusinessRules _districtBusinessRules;
        private readonly IMapper _mapper;

        public DeleteDistrictCommandHandler(IDistrictRepository districtRepository, DistrictBusinessRules districtBusinessRules, IMapper mapper)
        {
            _districtRepository = districtRepository;
            _districtBusinessRules = districtBusinessRules;
            _mapper = mapper;
        }

        public async Task<DeletedDistrictResponse> Handle(DeleteDistrictCommand request, CancellationToken cancellationToken)
        {
            District? district = await _districtRepository.GetAsync(x => x.Id == request.Id,cancellationToken:cancellationToken);

            await _districtBusinessRules.DistrictShouldExistWhenSelected(district);

            await _districtRepository.DeleteAsync(district!);

            DeletedDistrictResponse response = _mapper.Map<DeletedDistrictResponse>(district);

            return response;


        }
    }
}
