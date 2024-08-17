using Application.Features.Cities.Rules;
using Application.Services.Repositories;
using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PetOwnersService;


public class PetOwnerManager : IPetOwnerService
{
    private readonly IPetOwnerRepository _petOwnerRepository;

    public PetOwnerManager(IPetOwnerRepository petOwnerRepository)
    {
        _petOwnerRepository = petOwnerRepository;
    }

    public async Task<PetOwner?> GetAsync(
      Expression<Func<PetOwner, bool>> predicate,
      Func<IQueryable<PetOwner>, IIncludableQueryable<PetOwner, object>>? include = null,
      bool withDeleted = false,
      bool enableTracking = true,
      CancellationToken cancellationToken = default
  )
    {
        PetOwner? petOwner = await _petOwnerRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return petOwner;
    }

    public async Task<IPaginate<PetOwner>?> GetListAsync(
        Expression<Func<PetOwner, bool>>? predicate = null,
        Func<IQueryable<PetOwner>, IOrderedQueryable<PetOwner>>? orderBy = null,
        Func<IQueryable<PetOwner>, IIncludableQueryable<PetOwner, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<PetOwner> PetOwnerList = await _petOwnerRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return PetOwnerList;
    }

    public async Task<PetOwner> AddAsync(PetOwner petOwner)
    {
        PetOwner addedPetOwner = await _petOwnerRepository.AddAsync(petOwner);

        return addedPetOwner;
    }

    public async Task<PetOwner> UpdateAsync(PetOwner petOwner)
    {
        PetOwner updatedPetOwner = await _petOwnerRepository.UpdateAsync(petOwner);

        return updatedPetOwner;
    }

    public async Task<PetOwner> DeleteAsync(PetOwner petOwner, bool permanent = false)
    {
        PetOwner deletedPetOwner = await _petOwnerRepository.DeleteAsync(petOwner);

        return deletedPetOwner;
    }
}