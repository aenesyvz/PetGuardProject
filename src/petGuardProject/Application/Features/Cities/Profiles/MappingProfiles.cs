using Application.Features.Cities.Commands.Create;
using Application.Features.Cities.Commands.Delete;
using Application.Features.Cities.Commands.Update;
using Application.Features.Cities.Queries.GetAll;
using Application.Features.Cities.Queries.GetById;
using AutoMapper;
using Core.Application.Responses;
using Core.Persistence.Paging;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cities.Profiles;


public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<City, CreateCityCommand>().ReverseMap();
        CreateMap<City, CreatedCityResponse>().ReverseMap();
        CreateMap<City, UpdateCityCommand>().ReverseMap();
        CreateMap<City, UpdatedCityResponse>().ReverseMap();
        CreateMap<City, DeleteCityCommand>().ReverseMap();
        CreateMap<City, DeletedCityResponse>().ReverseMap();
        CreateMap<City, GetByIdCityResponse>().ReverseMap();
        CreateMap<City, GetAllCityListItemDto>().ReverseMap();

    }
}