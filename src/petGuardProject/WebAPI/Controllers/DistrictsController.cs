using Application.Features.Districts.Commands.Create;
using Application.Features.Districts.Commands.Delete;
using Application.Features.Districts.Commands.Update;
using Application.Features.Districts.Queries.GetAllByDynamic;
using Application.Features.Districts.Queries.GetById;
using Application.Features.Districts.Queries.GetListByDynamic;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DistrictsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateDistrictCommand createDistrictCommand)
    {
        CreatedDistrictResponse response = await Mediator.Send(createDistrictCommand);

        return Created(uri: "", response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateDistrictCommand updateDistrictCommand)
    {
        UpdatedDistrictResponse response = await Mediator.Send(updateDistrictCommand);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        DeletedDistrictResponse response = await Mediator.Send(new DeleteDistrictCommand { Id = id });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdDistrictResponse response = await Mediator.Send(new GetByIdDistrictQuery { Id = id });
        return Ok(response);
    }


    [HttpPost("GetList/ByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery? dynamicQuery = null)
    {
        GetListDistrictByDynamicModelQuery listDistrictByDynamicModelQuery = new() { PageRequest = pageRequest, DynamicQuery = dynamicQuery };
        GetListResponse<GetListDistrictByDynamicModelListItemDto> result = await Mediator.Send(listDistrictByDynamicModelQuery);
        return Ok(result);
    }

    [HttpPost("GetListWithOutPagination/ByDynamic")]
    public async Task<IActionResult> GetListWithOutPaginationByDynamic([FromBody] DynamicQuery? dynamicQuery = null)
    {
        GetAllDistrictByDynamicModelQuery listDistrictByDynamicModelQuery = new() { DynamicQuery = dynamicQuery };
        IList<GetAllDistrictByDynamicModelListItemDto> result = await Mediator.Send(listDistrictByDynamicModelQuery);
        return Ok(result);
    }
}
