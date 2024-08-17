using Application.Features.Pets.Queries.GetListByDynamic;
using Application.Features.Pets.Commands.Create;
using Application.Features.Pets.Commands.Delete;
using Application.Features.Pets.Commands.Update;
using Application.Features.Pets.Queries.GetById;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PetsController : BaseController
{
    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromBody] CreatePetCommand createPetCommand)
    {
        CreatedPetResponse response = await Mediator.Send(createPetCommand);

        return Created(uri: "", response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdatePetCommand updatePetCommand)
    {
        UpdatedPetResponse response = await Mediator.Send(updatePetCommand);

        return Ok(response);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        DeletedPetResponse response = await Mediator.Send(new DeletePetCommand { Id = id });

        return Ok(response);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdPetResponse response = await Mediator.Send(new GetByIdPetQuery { Id = id });
        return Ok(response);
    }

    [HttpPost("GetList/ByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery? dynamicQuery = null)
    {
        GetListPetByDynamicModelQuery listPetByDynamicModelQuery = new() { PageRequest = pageRequest, DynamicQuery = dynamicQuery };
        GetListResponse<GetListPetByDynamicModelListItemDto> result = await Mediator.Send(listPetByDynamicModelQuery);
        return Ok(result);
    }
}
