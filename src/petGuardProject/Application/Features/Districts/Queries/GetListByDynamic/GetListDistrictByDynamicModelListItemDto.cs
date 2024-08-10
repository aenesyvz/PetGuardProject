using Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Districts.Queries.GetListByDynamic;


public class GetListDistrictByDynamicModelListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid CityId { get; set; }
    public string CityName { get; set; }
    public string Name { get; set; }
}
