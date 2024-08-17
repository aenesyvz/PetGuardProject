using Application.Features.Pets.Commands.Create;
using Application.Features.Pets.Commands.Delete;
using Application.Features.Pets.Commands.Update;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Pets.Profiles;

public class MappingProfiles:Profile
{
    public MappingProfiles()
    {
        CreateMap<Pet, CreatePetCommand>().ReverseMap();
        CreateMap<Pet, CreatedPetResponse>().ReverseMap();

        CreateMap<Pet, UpdatePetCommand>().ReverseMap();
        CreateMap<Pet, UpdatedPetResponse>().ReverseMap();

        CreateMap<Pet, DeletePetCommand>().ReverseMap();
        CreateMap<Pet, DeletedPetResponse>().ReverseMap();
    }
}
