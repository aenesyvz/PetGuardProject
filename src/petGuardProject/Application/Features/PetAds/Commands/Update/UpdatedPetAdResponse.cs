using Application.Features.Backers.Commands.Update;
using Core.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PetAds.Commands.Update;

public class UpdatedPetAdResponse : IResponse
{
    public Guid Id { get; set; }
}
