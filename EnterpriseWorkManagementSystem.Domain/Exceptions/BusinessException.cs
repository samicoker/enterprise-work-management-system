using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Domain.Exceptions
{
    public class BusinessException : BaseException
    {
        public BusinessException(string message)
            : base(message, (int)HttpStatusCode.BadRequest)
        {
        }
    }
}
