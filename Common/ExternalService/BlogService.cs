using System.Linq;
using Mn.NewsCms.Common.Models;
using OperationStatus = Mn.NewsCms.Common.BaseClass.OperationStatus;

namespace Mn.NewsCms.Common.ExternalService
{
    public interface IBlogService
    {
        IQueryable<RemoteRequestLog> GetListRemoteRequest();
        OperationStatus Create(RemoteRequestLog log);
        OperationStatus InsertRemoteRequestLog(string Controller, string Content);
    }
}
