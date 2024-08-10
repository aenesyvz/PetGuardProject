using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;
using Application.Services.Repositories;

namespace Persistence.Repositories;

public class CityRepository : EfRepositoryBase<City, Guid, BaseDbContext>, ICityRepository
{
    public CityRepository(BaseDbContext context) : base(context)
    {
    }
}