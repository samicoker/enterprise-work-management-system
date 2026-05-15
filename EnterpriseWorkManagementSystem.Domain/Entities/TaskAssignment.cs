using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Domain.Entities
{
    public class TaskAssignment
    {
        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = null!;
    }
}
