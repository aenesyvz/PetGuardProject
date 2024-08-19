using Application.Features.Backers.Rules;
using Application.Features.JobApplications.Rules;
using Application.Features.PetAds.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.JobApplications.Commands.Create;
public class CreateJobApplicationCommand:IRequest<CreatedJobApplicationResponse>
{
    public Guid PetAdId { get; set; }
    public Guid BackerId { get; set; }


    public class CreateJobApplicationCommandHandler : IRequestHandler<CreateJobApplicationCommand, CreatedJobApplicationResponse>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IMapper _mapper;
        private readonly JobApplicationBusinessRules _jobApplicationBusinesRules;
        private readonly PetAdBusinesRules _petAdBusinesRules;
        private readonly BackerBusinessRules _backerBusinessRules;
        public CreateJobApplicationCommandHandler(IJobApplicationRepository jobApplicationRepository, IMapper mapper, JobApplicationBusinessRules jobApplicationBusinesRules, PetAdBusinesRules petAdBusinesRules,BackerBusinessRules backerBusinessRules)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _mapper = mapper;
            _jobApplicationBusinesRules = jobApplicationBusinesRules;
            _petAdBusinesRules = petAdBusinesRules;
            _backerBusinessRules = backerBusinessRules;
        }

        public async Task<CreatedJobApplicationResponse> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
        {
            await _jobApplicationBusinesRules.CheckIfBackerHasAlreadyApplied(request.PetAdId, request.BackerId);
            await _petAdBusinesRules.PetAdIdExistsWhenSelected(request.PetAdId);
            await _backerBusinessRules.BackerIdShouldExistWhenSelected(request.BackerId);

            JobApplication jobApplication = _mapper.Map<JobApplication>(request);

            await _jobApplicationRepository.AddAsync(jobApplication);

            CreatedJobApplicationResponse response = _mapper.Map<CreatedJobApplicationResponse>(jobApplication);

            return response;
        }
    }
}
