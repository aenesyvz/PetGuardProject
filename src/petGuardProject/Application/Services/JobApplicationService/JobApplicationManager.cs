using Application.Features.JobApplications.Rules;
using Application.Services.Repositories;
using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.JobApplicationService;

public class JobApplicationManager : IJobApplicationService
{
    private readonly IJobApplicationRepository _districtRepository;
    private readonly JobApplicationBusinessRules _districtBusinessRules;

    public JobApplicationManager(IJobApplicationRepository JobApplicationRepository, JobApplicationBusinessRules cityBusinessRules)
    {
        _districtRepository = JobApplicationRepository;
        _districtBusinessRules = cityBusinessRules;
    }

    public async Task<JobApplication?> GetAsync(
        Expression<Func<JobApplication, bool>> predicate,
        Func<IQueryable<JobApplication>, IIncludableQueryable<JobApplication, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        JobApplication? city = await _districtRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return city;
    }

    public async Task<IPaginate<JobApplication>?> GetListAsync(
        Expression<Func<JobApplication, bool>>? predicate = null,
        Func<IQueryable<JobApplication>, IOrderedQueryable<JobApplication>>? orderBy = null,
        Func<IQueryable<JobApplication>, IIncludableQueryable<JobApplication, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<JobApplication> JobApplicationList = await _districtRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return JobApplicationList;
    }

    public async Task<JobApplication> AddAsync(JobApplication district)
    {
       

        JobApplication addedJobApplication = await _districtRepository.AddAsync(district);

        return addedJobApplication;
    }

    public async Task<JobApplication> UpdateAsync(JobApplication district)
    {

        JobApplication updatedJobApplication = await _districtRepository.UpdateAsync(district);

        return updatedJobApplication;
    }

    public async Task<JobApplication> DeleteAsync(JobApplication district, bool permanent = false)
    {
        JobApplication deletedJobApplication = await _districtRepository.DeleteAsync(district);

        return deletedJobApplication;
    }
}