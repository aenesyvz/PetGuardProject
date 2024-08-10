using Core.Application.Responses;

namespace Application.Features.Cities.Queries.GetById;

public class GetByIdCityResponse : IResponse
{
    public Guid Id { get; set; }
    public int PlateCode { get; set; }
    public string Name { get; set; }
}
