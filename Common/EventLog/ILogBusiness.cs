using System.Linq;
using OperationStatus = Mn.NewsCms.Common.BaseClass.OperationStatus;

namespace Mn.NewsCms.Common.Models
{
    public interface ILogsBusiness
    {
        IQueryable<LogsBuffer> GetList();
        IQueryable<LogsBuffer> GetList(TypeOfLog type);
        IQueryable<LogsBuffer> GetList(string code);
        IQueryable<LogsBuffer> GetListTop(int count);        
        OperationStatus Create(LogsBuffer log);
        OperationStatus Create(string value);
        OperationStatus Create(string value, string code);
        OperationStatus Create(string value, string code, TypeOfLog type);
        OperationStatus DeleteAll();
        OperationStatus DeleteUntilToday();
    }
}
