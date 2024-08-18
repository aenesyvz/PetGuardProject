using Application.Features.PetOwners.Commands.Create;
using Application.Features.PetOwners.Commands.Delete;
using Application.Features.PetOwners.Commands.Update;
using Application.Features.PetOwners.Queries.GetById;
using Application.Features.PetOwners.Queries.GetListByDynamic;
using AutoMapper;
using Core.Application.Responses;
using Core.Persistence.Paging;
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
        CreateMap<PetOwner, PetOwnerForRegisterDto>().ReverseMap();

        CreateMap<PetOwner, UpdatePetOwnerCommand>().ReverseMap();
        CreateMap<PetOwner, UpdatedPetOwnerResponse>().ReverseMap();

        CreateMap<PetOwner, DeletePetOwnerCommand>().ReverseMap();
        CreateMap<PetOwner, DeletedPetOwnerResponse>().ReverseMap();

        CreateMap<PetOwner, GetByIdPetOwnerResponse>()
            .ForMember(destinationMember: c => c.Email, memberOptions: opt => opt.MapFrom(c => c.User.Email))
            .ForMember(destinationMember: c => c.CityName, memberOptions: opt => opt.MapFrom(c => c.City.Name))
            .ForMember(destinationMember: c => c.DistrcitName, memberOptions: opt => opt.MapFrom(c => c.District.Name));

        CreateMap<PetOwner, GetListPetOwnerByDynamicModelListItemDto>()
             .ForMember(destinationMember: c => c.Email, memberOptions: opt => opt.MapFrom(c => c.User.Email))
            .ForMember(destinationMember: c => c.CityName, memberOptions: opt => opt.MapFrom(c => c.City.Name))
            .ForMember(destinationMember: c => c.DistrcitName, memberOptions: opt => opt.MapFrom(c => c.District.Name));
        CreateMap<IPaginate<PetOwner>, GetListResponse<GetListPetOwnerByDynamicModelListItemDto>>().ReverseMap();
    }
}
