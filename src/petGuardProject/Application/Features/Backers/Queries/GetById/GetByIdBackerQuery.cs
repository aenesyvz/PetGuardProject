using Application.Features.Backers.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Backers.Queries.GetById;
public class GetByIdBackerQuery : IRequest<GetByIdBackerResponse>
{
    public Guid Id { get; set; }

    public class GetByIdBackerQueryHandler : IRequestHandler<GetByIdBackerQuery, GetByIdBackerResponse>
    {
        private readonly IBackerRepository _backerRepository;
        private readonly IMapper _mapper;
        private readonly BackerBusinessRules _backerBusinessRules;

        public GetByIdBackerQueryHandler(IBackerRepository backerRepository, IMapper mapper, BackerBusinessRules backerBusinessRules)
        {
            _backerRepository = backerRepository;
            _mapper = mapper;
            _backerBusinessRules = backerBusinessRules;
        }

        public async Task<GetByIdBackerResponse> Handle(GetByIdBackerQuery request, CancellationToken cancellationToken)
        {
            Backer? backer = await _backerRepository.GetAsync(
                    predicate: x => x.Id == request.Id,
                    include: m => m.Include(m => m.City).Include(m => m.District),
                    enableTracking:false,
                    cancellationToken:cancellationToken);

            await _backerBusinessRules.BackerShouldExistWhenSelected(backer);

            GetByIdBackerResponse response = _mapper.Map<GetByIdBackerResponse>(backer);

            return response;
        }
    }
}
