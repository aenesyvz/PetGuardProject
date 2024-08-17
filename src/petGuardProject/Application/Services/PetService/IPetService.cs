using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.PetService;

public interface IPetService
{
    Task<Pet?> GetAsync(
       Expression<Func<Pet, bool>> predicate,
       Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>>? include = null,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default
   );

    Task<IPaginate<Pet>?> GetListAsync(
        Expression<Func<Pet, bool>>? predicate = null,
        Func<IQueryable<Pet>, IOrderedQueryable<Pet>>? orderBy = null,
        Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<Pet> AddAsync(Pet Pet);
    Task<Pet> UpdateAsync(Pet Pet);
    Task<Pet> DeleteAsync(Pet Pet, bool permanent = false);
}
