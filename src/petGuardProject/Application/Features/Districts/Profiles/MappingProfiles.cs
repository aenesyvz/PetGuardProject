using Application.Features.Districts.Commands.Create;
using Application.Features.Districts.Commands.Delete;
using Application.Features.Districts.Commands.Update;
using Application.Features.Districts.Queries.GetAllByDynamic;
using Application.Features.Districts.Queries.GetById;
using Application.Features.Districts.Queries.GetListByDynamic;
using AutoMapper;
using Core.Application.Responses;
using Core.Persistence.Paging;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Districts.Profiles;


public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<District, CreateDistrictCommand>().ReverseMap();
        CreateMap<District, CreatedDistrictResponse>().ReverseMap();
        CreateMap<District, UpdateDistrictCommand>().ReverseMap();
        CreateMap<District, UpdatedDistrictResponse>().ReverseMap();
        CreateMap<District, DeleteDistrictCommand>().ReverseMap();
        CreateMap<District, DeletedDistrictResponse>().ReverseMap();
        CreateMap<District, GetByIdDistrictResponse>().ReverseMap();
        CreateMap<District, GetListDistrictByDynamicModelListItemDto>()
            .ForMember(destinationMember: c => c.CityName, memberOptions: opt => opt.MapFrom(c => c.City.Name));
        CreateMap<District, GetAllDistrictByDynamicModelListItemDto>()
           .ForMember(destinationMember: c => c.CityName, memberOptions: opt => opt.MapFrom(c => c.City.Name));
        CreateMap<IPaginate<District>, GetListResponse<GetListDistrictByDynamicModelListItemDto>>().ReverseMap();
    }
}