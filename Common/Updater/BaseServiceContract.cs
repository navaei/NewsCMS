using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.Common
{
    [DataContract]
    public class FeedContract
    {
        [DataMember]
        public long Id;
        [DataMember]
        public string LastFeedItemUrl;
        [DataMember]
        public IEnumerable<string> LastFeedItems;
        [DataMember]
        public string Link;
        [DataMember]
        public bool HasNewFeed;

        [DataMember]
        public bool IsAtom;
        [DataMember]
        public bool IsNull;

        [DataMember]
        public string Cats;

        [DataMember]
        public string SiteTitle;
        [DataMember]
        public string SiteUrl;
        [DataMember]
        public long SiteId;
        public ShowContent ShowContentType { get; set; }
        [DataMember]
        public List<FeedItem> FeedItems;
    }

    //[DataContract]
    //public class FeedItemsContract
    //{
    //    [DataMember]
    //    public string Title;
    //    [DataMember]
    //    public string Link;
    //    [DataMember]
    //    public string Description;
    //    [DataMember]
    //    public Nullable<System.DateTime> PubDate;
    //}
}
