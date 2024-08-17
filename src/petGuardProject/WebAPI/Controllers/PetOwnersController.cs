using Application.Features.PetOwners.Commands.Create;
using Application.Features.PetOwners.Commands.Delete;
using Application.Features.PetOwners.Commands.Update;
using Application.Features.PetOwners.Queries.GetById;
using Application.Features.PetOwners.Queries.GetListByDynamic;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PetOwnersController : BaseController
{
    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromBody] PetOwnerForRegisterDto petOwnerForRegisterDto)
    {
        CreatePetOwnerCommand createPetOwnerCommand = new() { PetOwnerForRegisterDto = petOwnerForRegisterDto, IpAddress = getIpAddress() };
        CreatedPetOwnerResponse response = await Mediator.Send(createPetOwnerCommand);

        return Created(uri: "", response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdatePetOwnerCommand updatePetOwnerCommand)
    {
        UpdatedPetOwnerResponse response = await Mediator.Send(updatePetOwnerCommand);

        return Ok(response);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        DeletedPetOwnerResponse response = await Mediator.Send(new DeletePetOwnerCommand { Id = id });

        return Ok(response);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdPetOwnerResponse response = await Mediator.Send(new GetByIdPetOwnerQuery { Id = id });
        return Ok(response);
    }

    [HttpPost("GetList/ByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery? dynamicQuery = null)
    {
        GetListPetOwnerByDynamicModelQuery listPetOwnerByDynamicModelQuery = new() { PageRequest = pageRequest, DynamicQuery = dynamicQuery };
        GetListResponse<GetListPetOwnerByDynamicModelListItemDto> result = await Mediator.Send(listPetOwnerByDynamicModelQuery);
        return Ok(result);
    }
}
