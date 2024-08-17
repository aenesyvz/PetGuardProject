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

namespace Application.Services.PetService;


public class PetManager : IPetService
{
    private readonly IPetRepository _petRepository;

    public PetManager(IPetRepository petRepository)
    {
        _petRepository = petRepository;
    }

    public async Task<Pet?> GetAsync(
      Expression<Func<Pet, bool>> predicate,
      Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>>? include = null,
      bool withDeleted = false,
      bool enableTracking = true,
      CancellationToken cancellationToken = default
  )
    {
        Pet? pet = await _petRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return pet;
    }

    public async Task<IPaginate<Pet>?> GetListAsync(
        Expression<Func<Pet, bool>>? predicate = null,
        Func<IQueryable<Pet>, IOrderedQueryable<Pet>>? orderBy = null,
        Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Pet> PetList = await _petRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return PetList;
    }

    public async Task<Pet> AddAsync(Pet pet)
    {

        Pet addedPet = await _petRepository.AddAsync(pet);

        return addedPet;
    }

    public async Task<Pet> UpdateAsync(Pet pet)
    {

        Pet updatedPet = await _petRepository.UpdateAsync(pet);

        return updatedPet;
    }

    public async Task<Pet> DeleteAsync(Pet pet, bool permanent = false)
    {
        Pet deletedPet = await _petRepository.DeleteAsync(pet);

        return deletedPet;
    }
}