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

namespace Application.Features.JobApplications.Commands.Update;


public class UpdateJobApplicationCommand : IRequest<UpdatedJobApplicationResponse>
{
    public Guid PetAdId { get; set; }
    public Guid BackerId { get; set; }


    public class UpdateJobApplicationCommandHandler : IRequestHandler<UpdateJobApplicationCommand, UpdatedJobApplicationResponse>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IMapper _mapper;
        private readonly JobApplicationBusinessRules _jobApplicationBusinessRules;
        private readonly PetAdBusinesRules _petAdBusinesRules;
        private readonly BackerBusinessRules _backerBusinessRules;
        public UpdateJobApplicationCommandHandler(IJobApplicationRepository jobApplicationRepository, IMapper mapper, JobApplicationBusinessRules jobApplicationBusinessRules, PetAdBusinesRules petAdBusinesRules, BackerBusinessRules backerBusinessRules)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _mapper = mapper;
            _jobApplicationBusinessRules = jobApplicationBusinessRules;
            _petAdBusinesRules = petAdBusinesRules;
            _backerBusinessRules = backerBusinessRules;
        }

        public async Task<UpdatedJobApplicationResponse> Handle(UpdateJobApplicationCommand request, CancellationToken cancellationToken)
        {
            await _jobApplicationBusinessRules.CheckIfBackerHasAlreadyApplied(request.PetAdId, request.BackerId);
            await _petAdBusinesRules.PetAdIdExistsWhenSelected(request.PetAdId);
            await _backerBusinessRules.BackerIdShouldExistWhenSelected(request.BackerId);

            JobApplication jobApplication = _mapper.Map<JobApplication>(request);

            await _jobApplicationRepository.AddAsync(jobApplication);

            UpdatedJobApplicationResponse response = _mapper.Map<UpdatedJobApplicationResponse>(jobApplication);

            return response;
        }
    }
}
