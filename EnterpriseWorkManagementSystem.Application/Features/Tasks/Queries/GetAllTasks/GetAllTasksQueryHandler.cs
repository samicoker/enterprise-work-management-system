using EnterpriseWorkManagementSystem.Application.Abstractions.Persistence;
using EnterpriseWorkManagementSystem.Application.Common.Models;
using EnterpriseWorkManagementSystem.Application.DTOs.Task;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Application.Features.Tasks.Queries.GetAllTasks
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, Result<IReadOnlyList<TaskDto>>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Result<IReadOnlyList<TaskDto>>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetTasksWithCategoryAsync(cancellationToken);

            var taskDtos = tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                CategoryId = task.CategoryId,
                CategoryName = task.Category?.Name ?? string.Empty
            }).ToList();

            return Result<IReadOnlyList<TaskDto>>.Success(taskDtos, "Tasks fetched successfully.");
        }
    }
}
