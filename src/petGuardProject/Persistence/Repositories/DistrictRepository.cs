using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;
using Application.Services.Repositories;

namespace Persistence.Repositories;

public class DistrictRepository : EfRepositoryBase<District, Guid, BaseDbContext>, IDistrictRepository
{
    public DistrictRepository(BaseDbContext context) : base(context)
    {
    }
}