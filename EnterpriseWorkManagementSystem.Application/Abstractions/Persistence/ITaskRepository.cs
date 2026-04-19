using EnterpriseWorkManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Application.Abstractions.Persistence
{
    public interface ITaskRepository : IGenericRepository<TaskItem>
    {
        Task<IReadOnlyList<TaskItem>> GetTasksWithCategoryAsync(CancellationToken cancellationToken = default);
    }
}
