using Application.Features.PetAds.Commands.Create;
using Application.Features.PetAds.Commands.Delete;
using Application.Features.PetAds.Commands.Update;
using Application.Features.PetAds.Queries.GetById;
using Application.Features.PetAds.Queries.GetListByDynamic;
using AutoMapper;
using Core.Application.Responses;
using Core.Persistence.Paging;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PetAds.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<PetAd, CreatePetAdCommand>().ReverseMap();
        CreateMap<PetAd, CreatedPetAdResponse>().ReverseMap();
        CreateMap<PetAd, UpdatePetAdCommand>().ReverseMap();
        CreateMap<PetAd, UpdatedPetAdResponse>().ReverseMap();
        CreateMap<PetAd, DeletePetAdCommand>().ReverseMap();
        CreateMap<PetAd, DeletedPetAdResponse>().ReverseMap();
        CreateMap<PetAd, GetByIdPetAdResponse>()
             .ForMember(destinationMember: c => c.PetOwnerFirstName, memberOptions: opt => opt.MapFrom(c => c.PetOwner.FirstName))
            .ForMember(destinationMember: c => c.PetOwnerLastName, memberOptions: opt => opt.MapFrom(c => c.PetOwner.LastName))
            .ForMember(destinationMember: c => c.PetOwnerImage, memberOptions: opt => opt.MapFrom(c => c.PetOwner.ImageUrl))
            .ForMember(destinationMember: c => c.PetName, memberOptions: opt => opt.MapFrom(c => c.Pet.Name))
            .ForMember(destinationMember: c => c.PetImage, memberOptions: opt => opt.MapFrom(c => c.Pet.ImageUrl))
            .ForMember(destinationMember: c => c.CityName, memberOptions: opt => opt.MapFrom(c => c.City.Name))
            .ForMember(destinationMember: c => c.DistrictName, memberOptions: opt => opt.MapFrom(c => c.District.Name));
        CreateMap<PetAd, GetListPetAdByDynamicModelListItemDto>()
            .ForMember(destinationMember: c => c.PetOwnerFirstName, memberOptions: opt => opt.MapFrom(c => c.PetOwner.FirstName))
            .ForMember(destinationMember: c => c.PetOwnerLastName, memberOptions: opt => opt.MapFrom(c => c.PetOwner.LastName))
            .ForMember(destinationMember: c => c.PetOwnerImage, memberOptions: opt => opt.MapFrom(c => c.PetOwner.ImageUrl))
            .ForMember(destinationMember: c => c.PetName, memberOptions: opt => opt.MapFrom(c => c.Pet.Name))
            .ForMember(destinationMember: c => c.PetImage, memberOptions: opt => opt.MapFrom(c => c.Pet.ImageUrl))
            .ForMember(destinationMember: c => c.CityName, memberOptions: opt => opt.MapFrom(c => c.City.Name))
            .ForMember(destinationMember: c => c.DistrictName, memberOptions: opt => opt.MapFrom(c => c.District.Name));

        CreateMap<IPaginate<PetAd>, GetListResponse<GetListPetAdByDynamicModelListItemDto>>().ReverseMap();
    }
}
