using EnterpriseWorkManagementSystem.Domain.Enums;
using TaskStatus = EnterpriseWorkManagementSystem.Domain.Enums.TaskStatus;

namespace EnterpriseWorkManagementSystem.Application.DTOs.Task
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
