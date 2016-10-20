using System.Collections.Generic;
using OperationStatus = Mn.NewsCms.Common.BaseClass.OperationStatus;

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
