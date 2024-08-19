using Application.Features.JobApplications.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.JobApplications.Commands.Delete;

public class DeleteJobApplicationCommand : IRequest<DeletedJobApplicationResponse>
{
    public Guid Id { get; set; }

    public class DeleteJobApplicationCommandHandler: IRequestHandler<DeleteJobApplicationCommand, DeletedJobApplicationResponse>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IMapper _mapper;
        private readonly JobApplicationBusinessRules _jobApplicationBusinesRules;

        public DeleteJobApplicationCommandHandler(IJobApplicationRepository jobApplicationRepository, IMapper mapper, JobApplicationBusinessRules jobApplicationBusinessRules)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _mapper = mapper;
            _jobApplicationBusinesRules = jobApplicationBusinessRules;
        }

        public async Task<DeletedJobApplicationResponse> Handle(DeleteJobApplicationCommand request, CancellationToken cancellationToken)
        {
            JobApplication? jobApplication = await _jobApplicationRepository.GetAsync(predicate: x => x.Id == request.Id, enableTracking: false);

            await _jobApplicationBusinesRules.JobApplicationExistsWhenSelected(jobApplication);

            await _jobApplicationRepository.DeleteAsync(jobApplication);

            DeletedJobApplicationResponse response = _mapper.Map<DeletedJobApplicationResponse>(jobApplication);

            return response;
        }
    }
}