using EnterpriseWorkManagementSystem.Application.Abstractions.Persistence;
using EnterpriseWorkManagementSystem.Application.Common.Models;
using EnterpriseWorkManagementSystem.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTaskCommandHandler(
            ITaskRepository taskRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var taskItem = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

            if (taskItem is null || taskItem.IsDeleted)
            {
                throw new NotFoundException("Task not found.");
            }

            taskItem.IsDeleted = true;
            taskItem.UpdatedDate = DateTime.UtcNow;

            _taskRepository.Update(taskItem);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success("Task deleted successfully.");
        }
    }
}
