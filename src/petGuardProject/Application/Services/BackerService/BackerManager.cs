using Application.Services.Repositories;
using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.BackerService;

public class BackerManager : IBackerService
{
    private readonly IBackerRepository _backerRepository;

    public BackerManager(IBackerRepository backerRepository)
    {
        _backerRepository = backerRepository;
    }

    public async Task<Backer?> GetAsync(
          Expression<Func<Backer, bool>> predicate,
          Func<IQueryable<Backer>, IIncludableQueryable<Backer, object>>? include = null,
          bool withDeleted = false,
          bool enableTracking = true,
          CancellationToken cancellationToken = default
      )
    {
        Backer? backer = await _backerRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return backer;
    }

    public async Task<IPaginate<Backer>?> GetListAsync(
        Expression<Func<Backer, bool>>? predicate = null,
        Func<IQueryable<Backer>, IOrderedQueryable<Backer>>? orderBy = null,
        Func<IQueryable<Backer>, IIncludableQueryable<Backer, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Backer> BackerList = await _backerRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return BackerList;
    }

    public async Task<Backer> AddAsync(Backer backer)
    {
        Backer addedBacker = await _backerRepository.AddAsync(backer);

        return addedBacker;
    }

    public async Task<Backer> UpdateAsync(Backer backer)
    {
      

        Backer updatedBacker = await _backerRepository.UpdateAsync(backer);

        return updatedBacker;
    }

    public async Task<Backer> DeleteAsync(Backer backer, bool permanent = false)
    {
        Backer deletedBacker = await _backerRepository.DeleteAsync(backer);

        return deletedBacker;
    }
}