using Application.Features.JobApplications.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.JobApplications.Queries.GetById;
public class GetByIdJobApplicationQuery : IRequest<GetByIdJobApplicationResponse>
{
    public Guid Id { get; set; }

    public class GetByIdJobApplicationQueryHandler : IRequestHandler<GetByIdJobApplicationQuery, GetByIdJobApplicationResponse>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IMapper _mapper;
        private readonly JobApplicationBusinessRules _jobApplicationBusinessRules;

        public GetByIdJobApplicationQueryHandler(IJobApplicationRepository jobApplicationRepository, IMapper mapper, JobApplicationBusinessRules jobApplicationBusinessRules)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _mapper = mapper;
            _jobApplicationBusinessRules = jobApplicationBusinessRules;
        }

        public async Task<GetByIdJobApplicationResponse> Handle(GetByIdJobApplicationQuery request, CancellationToken cancellationToken)
        {
            JobApplication? jobApplication = await _jobApplicationRepository.GetAsync(predicate: x => x.Id == request.Id,
                                                                                      include: m => m.Include(m => m.PetAd).Include(m => m.Backer).Include(m => m.Backer.User),
                                                                                      enableTracking:false);

            await _jobApplicationBusinessRules.JobApplicationExistsWhenSelected(jobApplication);

            GetByIdJobApplicationResponse response = _mapper.Map<GetByIdJobApplicationResponse>(jobApplication);

            return response;
        }
    }
}
