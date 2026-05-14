using EnterpriseWorkManagementSystem.Application.Common.Models;
using EnterpriseWorkManagementSystem.Domain.Enums;
using MediatR;

namespace EnterpriseWorkManagementSystem.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public int CategoryId { get; set; }
    }
}
