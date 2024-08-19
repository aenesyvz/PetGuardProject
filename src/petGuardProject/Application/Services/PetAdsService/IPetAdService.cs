using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PetAdsService;


public interface IPetAdService
{
    Task<PetAd?> GetAsync(
       Expression<Func<PetAd, bool>> predicate,
       Func<IQueryable<PetAd>, IIncludableQueryable<PetAd, object>>? include = null,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default
   );

    Task<IPaginate<PetAd>?> GetListAsync(
        Expression<Func<PetAd, bool>>? predicate = null,
        Func<IQueryable<PetAd>, IOrderedQueryable<PetAd>>? orderBy = null,
        Func<IQueryable<PetAd>, IIncludableQueryable<PetAd, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<PetAd> AddAsync(PetAd PetAd);
    Task<PetAd> UpdateAsync(PetAd PetAd);
    Task<PetAd> DeleteAsync(PetAd PetAd, bool permanent = false);
}
