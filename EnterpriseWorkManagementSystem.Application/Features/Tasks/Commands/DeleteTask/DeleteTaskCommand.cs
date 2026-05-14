using EnterpriseWorkManagementSystem.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}
