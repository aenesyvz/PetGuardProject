using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;
using Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;

namespace Persistence.Repositories;


public class EmailAuthenticatorRepository
    : EfRepositoryBase<EmailAuthenticator, Guid, BaseDbContext>,
        IEmailAuthenticatorRepository
{
    public EmailAuthenticatorRepository(BaseDbContext context)
        : base(context) { }
}
