using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.DistrictService;

public interface IDistrictService
{
    Task<District?> GetAsync(
       Expression<Func<District, bool>> predicate,
       Func<IQueryable<District>, IIncludableQueryable<District, object>>? include = null,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default
   );

    Task<IPaginate<District>?> GetListAsync(
        Expression<Func<District, bool>>? predicate = null,
        Func<IQueryable<District>, IOrderedQueryable<District>>? orderBy = null,
        Func<IQueryable<District>, IIncludableQueryable<District, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<District> AddAsync(District District);
    Task<District> UpdateAsync(District District);
    Task<District> DeleteAsync(District District, bool permanent = false);
}
