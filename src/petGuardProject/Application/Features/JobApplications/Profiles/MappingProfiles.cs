using Application.Features.JobApplications.Commands.Create;
using Application.Features.JobApplications.Commands.Delete;
using Application.Features.JobApplications.Commands.Update;
using Application.Features.JobApplications.Queries.GetById;
using Application.Features.JobApplications.Queries.GetListByDynamic;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.JobApplications.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<JobApplication, CreateJobApplicationCommand>().ReverseMap();
        CreateMap<JobApplication, CreatedJobApplicationResponse>().ReverseMap();

        CreateMap<JobApplication, UpdateJobApplicationCommand>().ReverseMap();
        CreateMap<JobApplication, UpdatedJobApplicationResponse>().ReverseMap();

        CreateMap<JobApplication, DeleteJobApplicationCommand>().ReverseMap();
        CreateMap<JobApplication, DeletedJobApplicationResponse>().ReverseMap();

        CreateMap<JobApplication, GetByIdJobApplicationResponse>()
            .ForMember(destinationMember: c => c.BackerEmail, memberOptions: opt => opt.MapFrom(c => c.Backer.User.Email))
            .ForMember(destinationMember: c => c.BackerFirstName, memberOptions: opt => opt.MapFrom(c => c.Backer.FirstName))
            .ForMember(destinationMember: c => c.BackerLastName, memberOptions: opt => opt.MapFrom(c => c.Backer.LastName))
            .ForMember(destinationMember: c => c.BackerImageUrl, memberOptions: opt => opt.MapFrom(c => c.Backer.ImageUrl))
            .ForMember(destinationMember: c => c.BackerPhoneNumber, memberOptions: opt => opt.MapFrom(c => c.Backer.PhoneNumber));

        CreateMap<JobApplication, GetListJobApplicationByDynamicModelListItemDto>()
           .ForMember(destinationMember: c => c.BackerFirstName, memberOptions: opt => opt.MapFrom(c => c.Backer.FirstName))
           .ForMember(destinationMember: c => c.BackerLastName, memberOptions: opt => opt.MapFrom(c => c.Backer.LastName))
           .ForMember(destinationMember: c => c.BackerImageUrl, memberOptions: opt => opt.MapFrom(c => c.Backer.ImageUrl));

    }
}
