using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.CityService;

public interface ICityService
{
    Task<City?> GetAsync(
       Expression<Func<City, bool>> predicate,
       Func<IQueryable<City>, IIncludableQueryable<City, object>>? include = null,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default
   );

    Task<IPaginate<City>?> GetListAsync(
        Expression<Func<City, bool>>? predicate = null,
        Func<IQueryable<City>, IOrderedQueryable<City>>? orderBy = null,
        Func<IQueryable<City>, IIncludableQueryable<City, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<City> AddAsync(City City);
    Task<City> UpdateAsync(City City);
    Task<City> DeleteAsync(City City, bool permanent = false);
}
