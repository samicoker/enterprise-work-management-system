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
            return await _context.Tasks.Where(x => !x.IsDeleted)
                .Include(x => x.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPagedTasksWithCategoryAsync(
            int pageNumber,
            int pageSize,
            string? userId,
            bool isAdmin,
            string? search,
            int? status,
            int? priority,
            int? categoryId,
            string? sortBy,
            string? sortDirection,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Tasks.Where(x => !x.IsDeleted)
                .Include(x => x.Category)
                .Include(x => x.Assignments)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(x =>
                    x.CreatedByUserId == userId ||
                    x.Assignments.Any(a => a.UserId == userId));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Title.Contains(search) ||
                    x.Description.Contains(search));
            }

            if (status.HasValue)
            {
                query = query.Where(x => (int)x.Status == status.Value);
            }

            if (priority.HasValue)
            {
                query = query.Where(x => (int)x.Priority == priority.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            query = sortBy?.ToLower() switch
            {
                "duedate" => sortDirection?.ToLower() == "asc"
                    ? query.OrderBy(x => x.DueDate)
                    : query.OrderByDescending(x => x.DueDate),

                "priority" => sortDirection?.ToLower() == "asc"
                    ? query.OrderBy(x => x.Priority)
                    : query.OrderByDescending(x => x.Priority),

                "status" => sortDirection?.ToLower() == "asc"
                    ? query.OrderBy(x => x.Status)
                    : query.OrderByDescending(x => x.Status),

                _ => query.OrderByDescending(x => x.CreatedDate)
            };


            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<TaskItem?> GetTaskWithDetailsByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Include(x => x.Category)
                .Include(x => x.Assignments)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
