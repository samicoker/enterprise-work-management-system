using EnterpriseWorkManagementSystem.Application.Abstractions.Persistence;
using EnterpriseWorkManagementSystem.Application.Common.Models;
using EnterpriseWorkManagementSystem.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTaskCommandHandler(
            ITaskRepository taskRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var taskItem = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

            if (taskItem is null || taskItem.IsDeleted)
            {
                throw new NotFoundException("Task not found.");
            }

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

            if (category is null)
            {
                throw new NotFoundException("Category not found.");
            }

            taskItem.Title = request.Title;
            taskItem.Description = request.Description;
            taskItem.DueDate = request.DueDate;
            taskItem.Status = request.Status;
            taskItem.Priority = request.Priority;
            taskItem.CategoryId = request.CategoryId;
            taskItem.UpdatedDate = DateTime.UtcNow;

            _taskRepository.Update(taskItem);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success("Task updated successfully.");
        }
    }
}
