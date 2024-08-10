using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Services.Repositories;

public interface IOtpAuthenticatorRepository : IAsyncRepository<OtpAuthenticator, Guid>, IRepository<OtpAuthenticator, Guid> { }
