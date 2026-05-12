using EnterpriseWorkManagementSystem.Domain.Common;
using EnterpriseWorkManagementSystem.Domain.Enums;
using TaskStatus = EnterpriseWorkManagementSystem.Domain.Enums.TaskStatus;


namespace EnterpriseWorkManagementSystem.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Pending;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public string? CreatedByUserId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
