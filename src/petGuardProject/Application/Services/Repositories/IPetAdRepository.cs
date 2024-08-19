using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Services.Repositories;

public interface IPetAdRepository : IAsyncRepository<PetAd,Guid>, IRepository<PetAd, Guid> { }