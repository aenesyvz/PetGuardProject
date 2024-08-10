using Core.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cities.Commands.Update;

public class UpdatedCityResponse:IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PlateCode { get; set; }
}
