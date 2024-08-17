using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;
using Application.Services.Repositories;

namespace Persistence.Repositories;

public class PetRepository : EfRepositoryBase<Pet, Guid, BaseDbContext>, IPetRepository
{
    public PetRepository(BaseDbContext context) : base(context)
    {
    }
}