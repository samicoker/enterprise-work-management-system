using EnterpriseWorkManagementSystem.Application.Features.Tasks.Commands.CreateTask;
using EnterpriseWorkManagementSystem.Application.Features.Tasks.Commands.DeleteTask;
using EnterpriseWorkManagementSystem.Application.Features.Tasks.Commands.UpdateTask;
using EnterpriseWorkManagementSystem.Application.Features.Tasks.Queries.GetAllTasks;
using EnterpriseWorkManagementSystem.Application.Features.Tasks.Queries.GetTaskById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseWorkManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            [FromQuery] string? search = null,
            [FromQuery] int? status = null,
            [FromQuery] int? priority = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = null,
            CancellationToken cancellationToken = default)
        {
            var query = new GetAllTasksQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                Status = status,
                Priority = priority,
                CategoryId = categoryId,
                SortBy = sortBy,
                SortDirection = sortDirection
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetTaskByIdQuery { Id = id }, cancellationToken);

            return Ok(result);
        }
    }
}
