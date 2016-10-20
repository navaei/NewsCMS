using System;
using System.Linq;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.DomainClasses.Logs
{
    public class LogsBusiness : BaseBusiness<LogsBuffer>, ILogsBusiness
    {
        public LogsBusiness(IUnitOfWork dbContext) : base(dbContext)
        {
        }

        public IQueryable<LogsBuffer> GetList()
        {
            return base.GetList();
        }
        public IQueryable<LogsBuffer> GetList(TypeOfLog type)
        {
            return base.GetList().Where(log => log.Type == type);
        }

        public IQueryable<LogsBuffer> GetList(string code)
        {
            return base.GetList().Where(log => log.Code == code);
        }

        public IQueryable<LogsBuffer> GetListTop(int count)
        {
            return base.GetList().Take(count);
        }

        public new OperationStatus Create(LogsBuffer log)
        {
            return base.Create(log);
        }

        public OperationStatus Create(string value)
        {
            return this.Create(new LogsBuffer()
            {
                Value = value,
                Type = TypeOfLog.NotSet,
                Code = string.Empty
            });
        }

        public OperationStatus Create(string value, string code)
        {
            return this.Create(new LogsBuffer()
            {
                Value = value,
                Type = TypeOfLog.NotSet,
                Code = code
            });
        }

        public OperationStatus Create(string value, string code, TypeOfLog type)
        {
            return this.Create(new LogsBuffer()
            {
                Value = value,
                Type = type,
                Code = code
            });
        }

        public OperationStatus DeleteAll()
        {
            return base.DeleteWhere(t => 1 == 1);
        }

        public OperationStatus DeleteUntilToday()
        {
            return base.DeleteWhere(log => log.CreationDate < DateTime.Today);
        }
       
    }
}
