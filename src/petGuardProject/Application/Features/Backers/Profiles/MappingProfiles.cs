using Application.Features.Backers.Commands.Create;
using Application.Features.Backers.Commands.Delete;
using Application.Features.Backers.Commands.Update;
using Application.Features.Backers.Queries.GetById;
using Application.Features.Backers.Queries.GetListByDynamic;
using Application.Features.Districts.Queries.GetAllByDynamic;
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

namespace Application.Features.Backers.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Backer, CreateBackerCommand>().ReverseMap();
        CreateMap<Backer, CreatedBackerRespone>().ReverseMap();

        CreateMap<Backer, UpdateBackerCommand>().ReverseMap();
        CreateMap<Backer, UpdatedBackerResponse>().ReverseMap();

        CreateMap<Backer, DeleteBackerCommand>().ReverseMap();
        CreateMap<Backer, DeletedBackerResponse>().ReverseMap();

        CreateMap<Backer, GetByIdBackerResponse>()
            .ForMember(destinationMember: c => c.CityName, memberOptions: opt => opt.MapFrom(c => c.City.Name));

        CreateMap<Backer, GetListBackerDynamicModelListItemDto>()
          .ForMember(destinationMember: c => c.CityName, memberOptions: opt => opt.MapFrom(c => c.City.Name));
        CreateMap<IPaginate<Backer>, GetListResponse<GetListBackerDynamicModelListItemDto>>().ReverseMap();

    }
}
