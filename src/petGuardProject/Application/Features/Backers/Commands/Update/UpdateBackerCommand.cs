using Application.Features.Backers.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Backers.Commands.Update;
public class UpdateBackerCommand : IRequest<UpdatedBackerResponse>
{
    public Guid Id { get; set; }
    public Guid CityId { get; set; }
    public Guid DistrcitId { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }


    public class UpdateBackerCommandHandler : IRequestHandler<UpdateBackerCommand, UpdatedBackerResponse>
    {
        private readonly IBackerRepository _backerRepository;
        private readonly IMapper _mapper;
        private readonly BackerBusinessRules _backerBusinessRules;

        public UpdateBackerCommandHandler(IBackerRepository backerRepository, IMapper mapper, BackerBusinessRules backerBusinessRules)
        {
            _backerRepository = backerRepository;
            _mapper = mapper;
            _backerBusinessRules = backerBusinessRules;
        }

        public async Task<UpdatedBackerResponse> Handle(UpdateBackerCommand request, CancellationToken cancellationToken)
        {
            Backer? backer = await _backerRepository.GetAsync(x => x.Id == request.Id, enableTracking: false);
            
            await _backerBusinessRules.BackerShouldExistWhenSelected(backer);

            _mapper.Map(request, backer);

            await _backerRepository.UpdateAsync(backer!);

            UpdatedBackerResponse response = _mapper.Map<UpdatedBackerResponse>(backer);

            return response;


        }
    }
}
