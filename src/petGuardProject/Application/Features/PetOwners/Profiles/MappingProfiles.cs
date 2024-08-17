using Application.Features.PetOwners.Commands.Create;
using Application.Features.PetOwners.Commands.Delete;
using Application.Features.PetOwners.Commands.Update;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PetOwners.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<PetOwner, CreatePetOwnerCommand>().ReverseMap();
        CreateMap<PetOwner, CreatedPetOwnerResponse>().ReverseMap();

        CreateMap<PetOwner, UpdatePetOwnerCommand>().ReverseMap();
        CreateMap<PetOwner, UpdatedPetOwnerResponse>().ReverseMap();

        CreateMap<PetOwner, DeletePetOwnerCommand>().ReverseMap();
        CreateMap<PetOwner, DeletedPetOwnerResponse>().ReverseMap();
    }
}
