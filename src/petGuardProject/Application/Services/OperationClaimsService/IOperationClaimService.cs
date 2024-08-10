using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.OperationClaimsService;


public interface IOperationClaimService
{
    Task<OperationClaim?> GetAsync(
        Expression<Func<OperationClaim, bool>> predicate,
        Func<IQueryable<OperationClaim>, IIncludableQueryable<OperationClaim, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<IPaginate<OperationClaim>?> GetListAsync(
        Expression<Func<OperationClaim, bool>>? predicate = null,
        Func<IQueryable<OperationClaim>, IOrderedQueryable<OperationClaim>>? orderBy = null,
        Func<IQueryable<OperationClaim>, IIncludableQueryable<OperationClaim, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<OperationClaim> AddAsync(OperationClaim operationClaim);
    Task<OperationClaim> UpdateAsync(OperationClaim operationClaim);
    Task<OperationClaim> DeleteAsync(OperationClaim operationClaim, bool permanent = false);
}
