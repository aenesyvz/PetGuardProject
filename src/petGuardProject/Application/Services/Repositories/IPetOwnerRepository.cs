using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Services.Repositories;

public interface IPetOwnerRepository : IAsyncRepository<PetOwner,Guid>, IRepository<PetOwner,Guid> { }