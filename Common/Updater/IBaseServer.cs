using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.Common
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBaseService" in both code and config file together.
    [ServiceContract]
    public interface IBaseServer
    {
        [OperationContract]
        bool SendFeedItems(List<FeedItem> items);
        [OperationContract]
        bool SendFeeds(List<FeedContract> items);
        [OperationContract]
        void Optimize();
        [OperationContract]
        bool UpdateFeeds(Dictionary<long, string> feeds);
        [OperationContract]
        List<FeedContract> getLatestFeeds(int MaxSize, bool? IsLocaly);
        [OperationContract]
        List<FeedContract> getLatestFeedsByDuration(string DurationCode, int MaxSize, bool IsBlog);
    }


}
