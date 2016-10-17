using Mn.Framework.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tazeyab.Common;
using Tazeyab.Common.ExternalService;

namespace Tazeyab.DomainClasses.ExternalService
{
    public class TelegramBiz : BaseBusiness<TelegramMessage, long>, ITelegramBiz
    {
        public IQueryable<TelegramMessage> GetMessages()
        {
            return base.GetList();
        }
    }
}
