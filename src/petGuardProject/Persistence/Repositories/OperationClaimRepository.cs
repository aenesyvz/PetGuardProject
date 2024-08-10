using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;
using Application.Services.Repositories;

namespace Persistence.Repositories;

public class OperationClaimRepository : EfRepositoryBase<OperationClaim, int, BaseDbContext>, IOperationClaimRepository
{
    public OperationClaimRepository(BaseDbContext context)
        : base(context) { }
}
