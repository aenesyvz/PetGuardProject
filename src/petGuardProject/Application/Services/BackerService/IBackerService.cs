using Application.Features.Cities.Rules;
using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.BackerService;


public interface IBackerService
{
    Task<Backer?> GetAsync(
       Expression<Func<Backer, bool>> predicate,
       Func<IQueryable<Backer>, IIncludableQueryable<Backer, object>>? include = null,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default
   );

    Task<IPaginate<Backer>?> GetListAsync(
        Expression<Func<Backer, bool>>? predicate = null,
        Func<IQueryable<Backer>, IOrderedQueryable<Backer>>? orderBy = null,
        Func<IQueryable<Backer>, IIncludableQueryable<Backer, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<Backer> AddAsync(Backer Backer);
    Task<Backer> UpdateAsync(Backer Backer);
    Task<Backer> DeleteAsync(Backer Backer, bool permanent = false);
}
