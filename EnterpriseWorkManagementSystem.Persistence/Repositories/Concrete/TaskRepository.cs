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
            return await _context.Tasks.Where(x=>!x.IsDeleted)
                .Include(x => x.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPagedTasksWithCategoryAsync(
    int pageNumber,
    int pageSize,
    string? userId,
    bool isAdmin,
    CancellationToken cancellationToken = default)
        {
            var query = _context.Tasks.Where(x=>!x.IsDeleted)
                .Include(x => x.Category)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(x => x.CreatedByUserId == userId);
            }

            query = query.OrderByDescending(x => x.CreatedDate);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
