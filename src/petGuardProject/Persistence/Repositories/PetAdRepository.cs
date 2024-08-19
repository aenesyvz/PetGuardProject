using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;
using Application.Services.Repositories;

namespace Persistence.Repositories;

public class PetAdRepository : EfRepositoryBase<PetAd, Guid, BaseDbContext>, IPetAdRepository
{
    public PetAdRepository(BaseDbContext context) : base(context)
    {
    }
}