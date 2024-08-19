using Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.JobApplications.Queries.GetListByDynamic;

public class GetListJobApplicationByDynamicModelListItemDto:IDto
{
    public Guid Id { get; set; }
    public Guid PetAdId { get; set; }
    public Guid BackerId { get; set; }
    public string BackerFirstName { get; set; }
    public string BackerLastName { get; set; }
    public string BackerImageUrl { get; set; }
}
