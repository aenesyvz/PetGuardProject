using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;
using Application.Services.Repositories;

namespace Persistence.Repositories;

public class JobApplicationRepository : EfRepositoryBase<JobApplication, Guid, BaseDbContext>, IJobApplicationRepository
{
    public JobApplicationRepository(BaseDbContext context) : base(context)
    {
    }
}