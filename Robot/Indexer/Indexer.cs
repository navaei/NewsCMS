using System;
using System.Collections.Generic;
using System.Linq;
using CrawlerEngine;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common;

namespace Mn.NewsCms.Robot.Indexer
{
    class Indexer
    {

        public static List<Tag> TagsTable = new List<Tag>();            
        public static void FirstIndexing(FeedItem feedItem)
        {
            //--------condition for alternate items------
            if (feedItem.ItemId == 0)
                return;
            var entiti = new TazehaContext();
            if (TagsTable.Count == 0)
                TagsTable = entiti.Tags.ToList<Tag>();
            int InsertCount = 0;
            foreach (var tag in TagsTable)
            {
                if (feedItem.Title.ContainsX(tag.Value) || feedItem.Description.ContainsX(tag.Value))
                {                    
                    tag.RepeatCount = tag.RepeatCount == null ? 1 : tag.RepeatCount + 1;
                    InsertCount++;
                }
            }
            if (InsertCount > 0)
                try
                {
                    entiti.SaveChanges();
                }
                catch (Exception ex)
                {

                }
        }
        public static void TagsTableSaveChanges()
        {
            try
            {
                var entiti = new TazehaContext();
                var TagsTableMain = entiti.Tags.ToList<Tag>();
                TagsTable.ForEach(x => x.RepeatCount = x.RepeatCount == null ? 0 : x.RepeatCount);
                TagsTableMain.ForEach(x => x.RepeatCount = x.RepeatCount == null ? 0 : x.RepeatCount);

                int NumberOfChanges = 0;
                for (int i = 0; i < TagsTableMain.Count; i++)
                {
                    if (TagsTableMain[i].RepeatCount < TagsTable[i].RepeatCount)
                    {
                        TagsTableMain[i].RepeatCount = TagsTable[i].RepeatCount;
                        NumberOfChanges++;
                    }
                }
                entiti.SaveChanges();
                GeneralLogs.WriteLogInDB(">OK TagsTableSaveChanges NumberOfChanges:" + NumberOfChanges);
            }
            catch
            {
                GeneralLogs.WriteLogInDB(">Error TagsTableSaveChanges ");
            }
        }

    }

}
