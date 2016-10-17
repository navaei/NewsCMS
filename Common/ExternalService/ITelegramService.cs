using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tazeyab.Common.ExternalService
{
    public interface ITelegramBiz
    {        
        IQueryable<TelegramMessage> GetMessages();
    }
}
