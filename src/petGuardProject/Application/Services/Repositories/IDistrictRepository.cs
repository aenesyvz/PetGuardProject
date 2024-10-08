﻿using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Services.Repositories;

public interface IDistrictRepository : IAsyncRepository<District,Guid>,IRepository<District,Guid> { }
