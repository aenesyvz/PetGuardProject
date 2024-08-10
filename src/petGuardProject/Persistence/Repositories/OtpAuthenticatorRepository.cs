using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;
using Application.Services.Repositories;

namespace Persistence.Repositories;

public class OtpAuthenticatorRepository : EfRepositoryBase<OtpAuthenticator, Guid, BaseDbContext>, IOtpAuthenticatorRepository
{
    public OtpAuthenticatorRepository(BaseDbContext context)
        : base(context) { }
}
