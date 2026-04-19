using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Application.Abstractions.Infrastructure
{
    public interface ITokenService
    {
        string CreateToken(string userId, string email, IList<string> roles);
    }
}
