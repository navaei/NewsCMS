using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tazeyab.Common.Share
{
    public enum ContentType
    {
        Cat = 1,
        Tag = 2,
        Site = 3,
        Key = 4
    }

    public enum ViewMode
    {
        [Description("عدم‌نمایش")]
        NotShow = 0,
        [Description("منو")]
        Menu = 1,
        [Description("صفحه‌اصلی")]
        Index = 2,
        [Description("منو-صفحه‌اصلی")]
        MenuIndex = 12,
    }
    public enum SmtpServers
    {
        Gmail,
        Hotmail,
        Outlook,
        Yahoo,
        SendGrid,
        Tazeyab,
        ModernNet
    }
    [Flags]
    public enum UpdaterList : byte
    {
        None = 0,
        UpdaterServer = 1,
        UpdaterClient = 2,
        UpdaterClassic = 4,

        AsServiceUpdater = UpdaterServer | UpdaterClient,
        AllUpdater = UpdaterServer | UpdaterClient | UpdaterClassic
    }

    public enum FeedType
    {
        Rss = 0,
        Atom = 1
    }
    public enum DeleteStatus
    {
        Active = 0,
        Spam = 1,
        Filter = 2,
        NotWork = 3,
        ReRepeat = 5,
        Other = 6,
        Temporary = 7,
        NoAccepted = 10,
        NotFound = 404,
        RequestTimeOut = 408,
        Forbidden = 403,
    }
}
