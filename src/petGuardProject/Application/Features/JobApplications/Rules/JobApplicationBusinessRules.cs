using Application.Features.JobApplications.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Domain.Entities;
using NArchitecture.Core.Localization.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.JobApplications.Rules;

public class JobApplicationBusinessRules : BaseBusinessRules
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly ILocalizationService _localizationService;

    public JobApplicationBusinessRules(IJobApplicationRepository jobApplicationRepository, ILocalizationService localizationService)
    {
        _jobApplicationRepository = jobApplicationRepository;
        _localizationService = localizationService;
    }


    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, JobApplicationsMessages.SectionName);
    }

    public async Task JobApplicationExistsWhenSelected(JobApplication? jobApplication)
    {
        if (jobApplication is null)
            await throwBusinessException(JobApplicationsMessages.JobApplicationDontExists);
    }

    public async Task JobApplicationIdExistsWhenSelected(Guid id)
    {
        JobApplication? jobApplication = await _jobApplicationRepository.GetAsync(predicate: x => x.Id == id, enableTracking: false);
        await JobApplicationExistsWhenSelected(jobApplication);
    }

    public async Task CheckIfBackerHasAlreadyApplied(Guid petAdId,Guid backerId)
    {
        JobApplication? jobApplication = await _jobApplicationRepository.GetAsync(predicate: x => x.PetAdId == petAdId && x.BackerId == backerId, 
                                                                                  enableTracking: false);
        if (jobApplication is not null)
            await throwBusinessException(JobApplicationsMessages.JobApplicationAlreadyExists);
    }
}
