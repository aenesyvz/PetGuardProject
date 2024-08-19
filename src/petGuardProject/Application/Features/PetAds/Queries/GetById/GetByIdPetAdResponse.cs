using Core.Application.Responses;
using Domain.Enums;

namespace Application.Features.PetAds.Queries.GetById;

public class GetByIdPetAdResponse: IResponse
{
    public Guid PetOwnerId { get; set; }
    public string PetOwnerFirstName { get; set; }
    public string PetOwnerLastName { get; set; }
    public string PetOwnerImage { get; set; }
    public Guid PetId { get; set; }
    public string PetName { get; set; }
    public string PetImage { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AdStatus AdStatus { get; set; }
    public Guid CityId { get; set; }
    public string CityName { get; set; }
    public Guid DistrictId { get; set; }
    public string DistrictName { get; set; }
}
