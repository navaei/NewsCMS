using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tazeyab.Common.Models;

namespace Tazeyab.Common
{
    public interface IRwpBiz
    {
        RemoteWebPart Get(string code, bool tryNow = false);
        IQueryable<RemoteWebPart> GetList();
        OperationStatus CreateEdit(RemoteWebPart rwp); 
        IQueryable<RemoteWebPart> GetByKeyword(string keyword);
        IQueryable<RemoteWebPart> GetByCats(List<long> catIds);
        IQueryable<RemoteWebPart> GetByTag(long tagId);
    }
}
