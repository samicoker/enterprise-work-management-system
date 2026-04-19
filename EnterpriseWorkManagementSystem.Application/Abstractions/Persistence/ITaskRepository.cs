using EnterpriseWorkManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Application.Abstractions.Persistence
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
        void Update(TaskItem taskItem);
        void Delete(TaskItem taskItem);
    }
}
