using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.Common
{
    public interface IUpdaterDurationBusiness
    {
        List<UpdateDuration> GetList();
        UpdateDuration GetLast(string Code, int CountOfFeed);
        OperationStatus Edit(UpdateDuration duration);
        OperationStatus Edit(int id, int feedsCount, int startIndex);
        void PokeClients();
    }
}
