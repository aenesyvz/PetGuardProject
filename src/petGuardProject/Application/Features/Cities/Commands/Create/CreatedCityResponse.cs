﻿using Core.Application.Responses;

namespace Application.Features.Cities.Commands.Create;

public class CreatedCityResponse:IResponse
{
    public Guid Id { get; set; }
    public int PlateCode { get; set; }
    public string Name { get; set; }
}
