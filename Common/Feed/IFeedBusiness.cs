﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mn.NewsCms.Common.Models;
using OperationStatus = Mn.NewsCms.Common.BaseClass.OperationStatus;

namespace Mn.NewsCms.Common
{
    public interface IFeedBusiness
    {
        Feed Get(long feedId);
        Feed GetWithSite(long feedId);
        IQueryable<Feed> GetList();
        OperationStatus CreateEdit(Feed feed);
        OperationStatus Edit(Feed feed);
        OperationStatus UpdateFeed(Feed feed);
        int GetCount();

        OperationStatus DisableTemporary(long feedId);
        void CheckForChangeDuration(Feed feed, bool hasNewFeedItem);
        void SpeedDOWN(Feed feed);
        void SpeedUP(Feed feed);
        OperationStatus CreateFeedLog(FeedLog feedLog);
        IQueryable<FeedLog> GetListLogs();

    }
}
