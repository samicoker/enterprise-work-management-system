using EnterpriseWorkManagementSystem.Application.Abstractions.Persistence;
using EnterpriseWorkManagementSystem.Application.Common.Models;
using EnterpriseWorkManagementSystem.Domain.Entities;
using EnterpriseWorkManagementSystem.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = EnterpriseWorkManagementSystem.Domain.Enums.TaskStatus;

namespace EnterpriseWorkManagementSystem.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<int>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTaskCommandHandler(
            ITaskRepository taskRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

            if (category is null)
            {
                return Result<int>.Failure("Category not found.");
            }

            var taskItem = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                CategoryId = request.CategoryId,
                Status = TaskStatus.Pending,
                Priority = TaskPriority.Medium
            };

            await _taskRepository.AddAsync(taskItem, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(taskItem.Id, "Task created successfully.");
        }
    }
}
