using AutoMapper;
using EnterpriseWorkManagementSystem.Application.Abstractions.Infrastructure;
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
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, Result<PagedResult<TaskDto>>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        //public async Task<Result<IReadOnlyList<TaskDto>>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        //{
        //    var tasks = await _taskRepository.GetTasksWithCategoryAsync(cancellationToken);

        //    //var taskDtos = tasks.Select(task => new TaskDto
        //    //{
        //    //    Id = task.Id,
        //    //    Title = task.Title,
        //    //    Description = task.Description,
        //    //    DueDate = task.DueDate,
        //    //    Status = task.Status,
        //    //    Priority = task.Priority,
        //    //    CategoryId = task.CategoryId,
        //    //    CategoryName = task.Category?.Name ?? string.Empty
        //    //}).ToList();

        //    var taskDtos =_mapper.Map<IReadOnlyList<TaskDto>>(tasks);

        //    return Result<IReadOnlyList<TaskDto>>.Success(taskDtos, "Tasks fetched successfully.");
        //}
        public async Task<Result<PagedResult<TaskDto>>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber < 1)
                request.PageNumber = 1;

            if (request.PageSize < 1)
                request.PageSize = 10;

            if (request.PageSize > 50)
                request.PageSize = 50;

            var userId = _currentUserService.UserId;
            var isAdmin = _currentUserService.IsInRole("Admin");

            var (items, totalCount) = await _taskRepository.GetPagedTasksWithCategoryAsync(
                request.PageNumber,
                request.PageSize,
                userId,
                isAdmin,
                cancellationToken);

            var taskDtos = _mapper.Map<IReadOnlyList<TaskDto>>(items);

            var pagedResult = new PagedResult<TaskDto>
            {
                Items = taskDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return Result<PagedResult<TaskDto>>.Success(pagedResult, "Tasks fetched successfully.");
        }
    }
}
