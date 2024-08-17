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

namespace Application.Features.Backers.Commands.Delete;
public class DeleteBackerCommand : IRequest<DeletedBackerResponse>
{
    public Guid Id { get; set; }

    public class DeleteBackerCommandHandler : IRequestHandler<DeleteBackerCommand, DeletedBackerResponse>
    {
        private readonly IBackerRepository _backerRepository;
        private readonly IMapper _mapper;
        private readonly BackerBusinessRules _backerBusinessRules;

        public DeleteBackerCommandHandler(IBackerRepository backerRepository, IMapper mapper, BackerBusinessRules backerBusinessRules)
        {
            _backerRepository = backerRepository;
            _mapper = mapper;
            _backerBusinessRules = backerBusinessRules;
        }

        public async Task<DeletedBackerResponse> Handle(DeleteBackerCommand request, CancellationToken cancellationToken)
        {

            Backer? backer = await _backerRepository.GetAsync(x => x.Id == request.Id, enableTracking: false);

            await _backerBusinessRules.BackerShouldExistWhenSelected(backer);

            await _backerRepository.DeleteAsync(backer!);

            DeletedBackerResponse response = _mapper.Map<DeletedBackerResponse>(backer);

            return response;
        }
    }
}
