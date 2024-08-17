using Core.Application.Responses;
using Domain.Enums;

namespace Application.Features.Backers.Queries.GetById;

public class GetByIdBackerResponse: IResponse
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalityNumber { get; set; }
    public string DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string CityName { get; set; }
    public string DistrcitName { get; set; }
    public string Address { get; set; }
    public string? ImageUrl { get; set; }
    public string PhoneNumber { get; set; }
}
