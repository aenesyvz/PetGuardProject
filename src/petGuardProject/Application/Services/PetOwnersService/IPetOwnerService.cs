using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.PetOwnersService;

public interface IPetOwnerService
{
    Task<PetOwner?> GetAsync(
       Expression<Func<PetOwner, bool>> predicate,
       Func<IQueryable<PetOwner>, IIncludableQueryable<PetOwner, object>>? include = null,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default
   );

    Task<IPaginate<PetOwner>?> GetListAsync(
        Expression<Func<PetOwner, bool>>? predicate = null,
        Func<IQueryable<PetOwner>, IOrderedQueryable<PetOwner>>? orderBy = null,
        Func<IQueryable<PetOwner>, IIncludableQueryable<PetOwner, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<PetOwner> AddAsync(PetOwner PetOwner);
    Task<PetOwner> UpdateAsync(PetOwner PetOwner);
    Task<PetOwner> DeleteAsync(PetOwner PetOwner, bool permanent = false);
}
