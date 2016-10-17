using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tazeyab.Common
{
    public interface IMembershipBusiness
    {
        bool IsUserFlow(string EntityCode, string UserName, string Content);       
    }
}
