using EnterpriseWorkManagementSystem.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : IRequest<Result<int>>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public int CategoryId { get; set; }
    }
}
