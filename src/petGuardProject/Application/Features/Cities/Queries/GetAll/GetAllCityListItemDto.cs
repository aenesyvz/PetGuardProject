using Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cities.Queries.GetAll;


public class GetAllCityListItemDto : IDto
{
    public Guid Id { get; set; }
    public int PlateCode { get; set; }
    public string Name { get; set; }
}
