using Application.Features.JobApplications.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.JobApplications.Queries.GetListByDynamic;

public class GetListJobApplicationByDynamicModelQuery:IRequest<GetListResponse<GetListJobApplicationByDynamicModelListItemDto>>
{
    public DynamicQuery DynamicQuery { get; set; }
    public PageRequest PageRequest { get; set; }

    public class GetListJobApplicationByDynamicModelQueryHandler : IRequestHandler<GetListJobApplicationByDynamicModelQuery, GetListResponse<GetListJobApplicationByDynamicModelListItemDto>>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IMapper _mapper;
        private readonly JobApplicationBusinessRules _jobApplicationBusinessRules;

        public GetListJobApplicationByDynamicModelQueryHandler(IJobApplicationRepository jobApplicationRepository, IMapper mapper, JobApplicationBusinessRules jobApplicationBusinessRules)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _mapper = mapper;
            _jobApplicationBusinessRules = jobApplicationBusinessRules;
        }

        public async Task<GetListResponse<GetListJobApplicationByDynamicModelListItemDto>> Handle(GetListJobApplicationByDynamicModelQuery request, CancellationToken cancellationToken)
        {
            IPaginate<JobApplication> paginate = await _jobApplicationRepository.GetListByDynamicAsync(
                    request.DynamicQuery,
                    include: m => m.Include(m => m.PetAd).Include(m => m.Backer),
                    index:request.PageRequest.PageIndex,
                    size:request.PageRequest.PageSize,
                    cancellationToken:cancellationToken
                );

            GetListResponse<GetListJobApplicationByDynamicModelListItemDto> response = _mapper.Map<GetListResponse<GetListJobApplicationByDynamicModelListItemDto>>(paginate);

            return response;
        }
    }
}