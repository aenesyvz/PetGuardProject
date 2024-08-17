using Application.Features.Pets.Commands.Create;
using Application.Features.Pets.Commands.Delete;
using Application.Features.Pets.Commands.Update;
using Application.Features.Pets.Queries.GetById;
using Application.Features.Pets.Queries.GetListByDynamic;
using AutoMapper;
using Core.Application.Responses;
using Core.Persistence.Paging;
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

        CreateMap<Pet, GetByIdPetResponse>()
            .ForMember(destinationMember: c => c.PetOwnerEmail, memberOptions: opt => opt.MapFrom(c => c.PetOwner.User.Email))
            .ForMember(destinationMember: c => c.PetOwnerFirstName, memberOptions: opt => opt.MapFrom(c => c.PetOwner.FirstName))
            .ForMember(destinationMember: c => c.PetOwnerLastName, memberOptions: opt => opt.MapFrom(c => c.PetOwner.LastName));

        CreateMap<Pet,GetListPetByDynamicModelListItemDto>()
            .ForMember(destinationMember: c => c.PetOwnerEmail, memberOptions: opt => opt.MapFrom(c => c.PetOwner.User.Email))
            .ForMember(destinationMember: c => c.PetOwnerFirstName, memberOptions: opt => opt.MapFrom(c => c.PetOwner.FirstName))
            .ForMember(destinationMember: c => c.PetOwnerLastName, memberOptions: opt => opt.MapFrom(c => c.PetOwner.LastName));
        CreateMap<IPaginate<Pet>, GetListResponse<GetListPetByDynamicModelListItemDto>>().ReverseMap();
    }
}
