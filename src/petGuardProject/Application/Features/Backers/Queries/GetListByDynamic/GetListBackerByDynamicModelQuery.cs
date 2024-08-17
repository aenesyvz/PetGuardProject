using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Backers.Queries.GetListByDynamic;


public class GetListBackerByDynamicModelQuery : IRequest<GetListResponse<GetListBackerByDynamicModelListItemDto>>
{
    public DynamicQuery DynamicQuery { get; set; }
    public PageRequest PageRequest { get; set; }

    public class GetListBackerDynamicModelQueryHandler : IRequestHandler<GetListBackerByDynamicModelQuery, GetListResponse<GetListBackerByDynamicModelListItemDto>>
    {
        private readonly IBackerRepository _backerRepository;
        private readonly IMapper _mapper;

        public GetListBackerDynamicModelQueryHandler(IBackerRepository backerRepository, IMapper mapper)
        {
            _backerRepository = backerRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListBackerByDynamicModelListItemDto>> Handle(GetListBackerByDynamicModelQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Backer> paginate = await _backerRepository.GetListByDynamicAsync(
                    request.DynamicQuery,
                    include: m => m.Include(m => m.User).Include(m => m.City).Include(m => m.District),
                    index: request.PageRequest.PageIndex,
                    size: request.PageRequest.PageSize,
                    cancellationToken:cancellationToken
                );

            GetListResponse<GetListBackerByDynamicModelListItemDto> response = _mapper.Map<GetListResponse<GetListBackerByDynamicModelListItemDto>>( paginate );

            return response;
        }
    }
}