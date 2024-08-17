using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Services.Repositories;

public interface IBackerRepository : IAsyncRepository<Backer,Guid>, IRepository<Backer,Guid> { }
