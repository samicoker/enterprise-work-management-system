using EnterpriseWorkManagementSystem.Application.Features.Tasks.Commands.CreateTask;
using EnterpriseWorkManagementSystem.Application.Features.Tasks.Commands.DeleteTask;
using EnterpriseWorkManagementSystem.Application.Features.Tasks.Queries.GetAllTasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseWorkManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        //{
        //    var result = await _mediator.Send(new GetAllTasksQuery(), cancellationToken);

        //    if (!result.IsSuccess)
        //    {
        //        return BadRequest(result);
        //    }

        //    return Ok(result);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
        {
            var query = new GetAllTasksQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteTaskCommand { Id = id }, cancellationToken);

            return Ok(result);
        }
    }
}
