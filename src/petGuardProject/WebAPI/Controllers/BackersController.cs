using Application.Features.Backers.Commands.Create;
using Application.Features.Backers.Commands.Delete;
using Application.Features.Backers.Commands.Update;
using Application.Features.Backers.Queries.GetById;
using Application.Features.Backers.Queries.GetListByDynamic;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BackersController : BaseController
{

    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromBody] BackerForRegisterDto backerForRegisterDto)
    {
        CreateBackerCommand createBackerCommand = new() { BackerForRegisterDto = backerForRegisterDto, IpAddress = getIpAddress() };
        CreatedBackerResponse response = await Mediator.Send(createBackerCommand);

        return Created(uri: "", response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateBackerCommand updateBackerCommand)
    {
        UpdatedBackerResponse response = await Mediator.Send(updateBackerCommand);

        return Ok(response);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        DeletedBackerResponse response = await Mediator.Send(new DeleteBackerCommand { Id = id });

        return Ok(response);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdBackerResponse response = await Mediator.Send(new GetByIdBackerQuery { Id = id });
        return Ok(response);
    }

    [HttpPost("GetList/ByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery? dynamicQuery = null)
    {
        
        GetListBackerByDynamicModelQuery listBackerByDynamicModelQuery = new() { PageRequest = pageRequest, DynamicQuery = dynamicQuery };
        GetListResponse<GetListBackerByDynamicModelListItemDto> result = await Mediator.Send(listBackerByDynamicModelQuery);
        return Ok(result);
    }
}
