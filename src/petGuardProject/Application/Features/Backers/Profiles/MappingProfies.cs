using Application.Features.Backers.Commands.Create;
using Application.Features.Backers.Commands.Delete;
using Application.Features.Backers.Commands.Update;
using AutoMapper;
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
   
    }
}
