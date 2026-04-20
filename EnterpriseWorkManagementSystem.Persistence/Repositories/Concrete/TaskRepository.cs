using EnterpriseWorkManagementSystem.Application.Abstractions.Persistence;
using EnterpriseWorkManagementSystem.Domain.Entities;
using EnterpriseWorkManagementSystem.Persistence.Context;
using EnterpriseWorkManagementSystem.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;


namespace EnterpriseWorkManagementSystem.Persistence.Repositories.Concrete
{
    public class TaskRepository : GenericRepository<TaskItem>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<TaskItem>> GetTasksWithCategoryAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Include(x => x.Category)
                .ToListAsync(cancellationToken);
        }
    }
}
