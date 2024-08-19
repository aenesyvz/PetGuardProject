using Application.Features.PetAds.Commands.Create;
using Application.Features.PetAds.Commands.Delete;
using Application.Features.PetAds.Commands.Update;
using Application.Features.PetAds.Queries.GetById;
using Application.Features.PetAds.Queries.GetListByDynamic;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PetAdsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreatePetAdCommand createPetAdCommand)
    {
        CreatedPetAdResponse response = await Mediator.Send(createPetAdCommand);

        return Created(uri: "", response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePetAdCommand updatePetAdCommand)
    {
        UpdatedPetAdResponse response = await Mediator.Send(updatePetAdCommand);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        DeletedPetAdResponse response = await Mediator.Send(new DeletePetAdCommand { Id = id });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdPetAdResponse response = await Mediator.Send(new GetByIdPetAdQuery { Id = id });
        return Ok(response);
    }


    [HttpPost("GetList/ByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery? dynamicQuery = null)
    {
        GetListPetAdByDynamicModelQuery listPetAdByDynamicModelQuery = new() { PageRequest = pageRequest, DynamicQuery = dynamicQuery };
        GetListResponse<GetListPetAdByDynamicModelListItemDto> result = await Mediator.Send(listPetAdByDynamicModelQuery);
        return Ok(result);
    }
}
