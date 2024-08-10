using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;
using Application.Services.Repositories;

namespace Persistence.Repositories;

public class UserRepository : EfRepositoryBase<User, Guid, BaseDbContext>, IUserRepository
{
    public UserRepository(BaseDbContext context)
        : base(context) { }
}