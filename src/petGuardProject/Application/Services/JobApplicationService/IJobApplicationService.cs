using Application.Services.JobApplicationService;
using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.JobApplicationService;


public interface IJobApplicationService
{
    Task<JobApplication?> GetAsync(
       Expression<Func<JobApplication, bool>> predicate,
       Func<IQueryable<JobApplication>, IIncludableQueryable<JobApplication, object>>? include = null,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default
   );

    Task<IPaginate<JobApplication>?> GetListAsync(
        Expression<Func<JobApplication, bool>>? predicate = null,
        Func<IQueryable<JobApplication>, IOrderedQueryable<JobApplication>>? orderBy = null,
        Func<IQueryable<JobApplication>, IIncludableQueryable<JobApplication, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<JobApplication> AddAsync(JobApplication JobApplication);
    Task<JobApplication> UpdateAsync(JobApplication JobApplication);
    Task<JobApplication> DeleteAsync(JobApplication JobApplication, bool permanent = false);
}
