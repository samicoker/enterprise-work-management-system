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
        Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPagedTasksWithCategoryAsync(int pageNumber, int pageSize,
        string? userId,
        bool isAdmin,
        string? search,
        int? status,
        int? priority,
        int? categoryId,
        string? sortBy,
        string? sortDirection,
        CancellationToken cancellationToken = default);

        Task<TaskItem?> GetTaskWithDetailsByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
