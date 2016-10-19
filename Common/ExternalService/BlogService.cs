using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.Common.ExternalService
{
    public interface IBlogService
    {
        IQueryable<RemoteRequestLog> GetListRemoteRequest();
        OperationStatus Create(RemoteRequestLog log);
        OperationStatus InsertRemoteRequestLog(string Controller, string Content);
    }
}
