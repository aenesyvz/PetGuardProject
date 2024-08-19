using Application.Features.JobApplications.Commands.Create;
using Application.Features.JobApplications.Commands.Delete;
using Application.Features.JobApplications.Commands.Update;
using Application.Features.JobApplications.Queries.GetById;
using Application.Features.JobApplications.Queries.GetListByDynamic;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobApplicationsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateJobApplicationCommand createJobApplicationCommand)
    {
        CreatedJobApplicationResponse response = await Mediator.Send(createJobApplicationCommand);

        return Created(uri: "", response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateJobApplicationCommand updateJobApplicationCommand)
    {
        UpdatedJobApplicationResponse response = await Mediator.Send(updateJobApplicationCommand);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        DeletedJobApplicationResponse response = await Mediator.Send(new DeleteJobApplicationCommand { Id = id });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdJobApplicationResponse response = await Mediator.Send(new GetByIdJobApplicationQuery { Id = id });
        return Ok(response);
    }


    [HttpPost("GetList/ByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery? dynamicQuery = null)
    {
        GetListJobApplicationByDynamicModelQuery listJobApplicationByDynamicModelQuery = new() { PageRequest = pageRequest, DynamicQuery = dynamicQuery };
        GetListResponse<GetListJobApplicationByDynamicModelListItemDto> result = await Mediator.Send(listJobApplicationByDynamicModelQuery);
        return Ok(result);
    }
}
