using Core.Application.Dtos;
using Domain.Enums;

namespace Application.Features.Backers.Queries.GetListByDynamic;

public class GetListBackerDynamicModelListItemDto : IDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
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
