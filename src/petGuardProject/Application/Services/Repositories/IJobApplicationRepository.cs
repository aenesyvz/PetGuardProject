using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Services.Repositories;

public interface IJobApplicationRepository : IAsyncRepository<JobApplication,Guid>, IRepository<JobApplication,Guid> { }