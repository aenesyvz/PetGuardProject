using Core.Application.Responses;

namespace Application.Features.JobApplications.Queries.GetById;

public class GetByIdJobApplicationResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid PetAdId { get; set; }
    public Guid BackerId { get; set; }
    public string BackerEmail { get; set; }
    public string BackerFirstName { get; set; }
    public string BackerLastName { get; set; }
    public string BackerPhoneNumber { get; set; }
    public string  BackerImageUrl { get; set; }

}
