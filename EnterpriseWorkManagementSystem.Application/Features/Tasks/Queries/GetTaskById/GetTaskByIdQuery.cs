using EnterpriseWorkManagementSystem.Application.Common.Models;
using EnterpriseWorkManagementSystem.Application.DTOs.Task;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Application.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQuery : IRequest<Result<TaskDto>>
    {
        public int Id { get; set; }
    }
}
