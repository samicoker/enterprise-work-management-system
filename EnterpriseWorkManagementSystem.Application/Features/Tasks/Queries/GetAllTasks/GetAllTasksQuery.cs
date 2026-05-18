using EnterpriseWorkManagementSystem.Application.Common.Models;
using EnterpriseWorkManagementSystem.Application.DTOs.Task;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Application.Features.Tasks.Queries.GetAllTasks
{
    public class GetAllTasksQuery : IRequest<Result<PagedResult<TaskDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }
        public int? Status { get; set; }
        public int? Priority { get; set; }
        public int? CategoryId { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
