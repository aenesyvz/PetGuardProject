using Core.Application.Responses;
using Domain.Enums;

namespace Application.Features.Backers.Commands.Create;

public class CreatedBackerRespone: IResponse
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalityNumber { get; set; }
    public string DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public Guid CityId { get; set; }
    public Guid DistrcitId { get; set; }
    public string Address { get; set; }
    public string ImageUrl { get; set; }
    public string PhoneNumber { get; set; }
}
