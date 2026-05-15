using AutoMapper;
using EnterpriseWorkManagementSystem.Application.Abstractions.Infrastructure;
using EnterpriseWorkManagementSystem.Application.Abstractions.Persistence;
using EnterpriseWorkManagementSystem.Application.Common.Models;
using EnterpriseWorkManagementSystem.Application.DTOs.Task;
using EnterpriseWorkManagementSystem.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Application.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Result<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetTaskByIdQueryHandler(
            ITaskRepository taskRepository,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<TaskDto>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var taskItem = await _taskRepository.GetTaskWithDetailsByIdAsync(request.Id, cancellationToken);

            if (taskItem is null || taskItem.IsDeleted)
                throw new NotFoundException("Task not found.");

            var userId = _currentUserService.UserId;
            var isAdmin = _currentUserService.IsInRole("Admin");

            var hasAccess =
                isAdmin ||
                taskItem.CreatedByUserId == userId ||
                taskItem.Assignments.Any(x => x.UserId == userId);

            if (!hasAccess)
                throw new NotFoundException("Task not found.");

            var dto = _mapper.Map<TaskDto>(taskItem);

            return Result<TaskDto>.Success(dto, "Task fetched successfully.");
        }
    }
}
