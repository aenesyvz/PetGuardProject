using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;
using Application.Services.Repositories;

namespace Persistence.Repositories;

public class PetOwnerRepository : EfRepositoryBase<PetOwner, Guid, BaseDbContext>, IPetOwnerRepository
{
    public PetOwnerRepository(BaseDbContext context) : base(context)
    {
    }
}