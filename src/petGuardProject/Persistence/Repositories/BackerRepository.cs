using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;
using Application.Services.Repositories;

namespace Persistence.Repositories;

public class BackerRepository : EfRepositoryBase<Backer, Guid, BaseDbContext>, IBackerRepository
{
    public BackerRepository(BaseDbContext context) : base(context)
    {
    }
}