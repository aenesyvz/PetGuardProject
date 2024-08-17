using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Services.Repositories;

public interface IPetRepository : IAsyncRepository<Pet,Guid>, IRepository<Pet,Guid> { }
