﻿using Core.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PetAds.Commands.Delete;

public class DeletedPetAdResponse : IResponse
{
    public Guid Id { get; set; }
}
