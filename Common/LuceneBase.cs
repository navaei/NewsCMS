using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.EventsLog;
using Microsoft.Win32;
using Mn.NewsCms.Common.Config;

namespace Mn.NewsCms.Common
{

    public abstract class LuceneBase
    {
        public enum LuceneSearcherType
        {
            All,
            Key,
            Site,
            Cat,
            Time,
            FeedId,
            SiteUrl,
        }
        public enum LuceneSortField
        {
            PubDate,
            Relative,
        }
        #region fields
        static StandardAnalyzer analyzer = new StandardAnalyzer(Currentversion);
        private static string _lucenedir;
        private static FSDirectory _directoryTemp;
        protected static readonly Lucene.Net.Util.Version Currentversion = Lucene.Net.Util.Version.LUCENE_30;

        protected IndexSearcher _searcher = new IndexSearcher(_directory, false);
        protected Sort sort = new Sort(new SortField("PubDate", System.Globalization.CultureInfo.CurrentCulture, true));
        public static int TodayItemsCount = 0;
        #endregion

        #region peoperties
        public static string LuceneDir
        {
            get
            {
                if (string.IsNullOrEmpty(_lucenedir))
                    _lucenedir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\LuceneIndex");

                //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\LuceneIndex");
                return _lucenedir;
            }
            set
            {
                _lucenedir = value;
            }
        }
        protected static FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null)
                    _directoryTemp = FSDirectory.Open(new DirectoryInfo(LuceneDir));
                if (IndexWriter.IsLocked(_directoryTemp))
                    IndexWriter.Unlock(_directoryTemp);

                try
                {
                    var lockFilePath = Path.Combine(LuceneDir, "write.lock");
                    if (File.Exists(lockFilePath))
                        File.Delete(lockFilePath);
                }
                catch { }
                return _directoryTemp;
            }
        }
        static IndexWriter _writer;
        static IndexWriter writer
        {
            get
            {
                if (_writer == null)
                {
                    try
                    {
                        _writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
                    }

                    catch (LockObtainFailedException ex)
                    {
                        DirectoryInfo indexDirInfo = new DirectoryInfo(LuceneDir);
                        FSDirectory indexFSDir = FSDirectory.Open(indexDirInfo, new Lucene.Net.Store.SimpleFSLockFactory(indexDirInfo));
                        IndexWriter.Unlock(indexFSDir);
                        _writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
                    }
                }
                return _writer;
            }
        }
        #endregion

        public LuceneBase()
        {

        }

        #region Insert
        private Document FeedItemToDocument(FeedItem item)
        {
            Document doc = new Document();

            var feeditemid = new Field("FeedItemId", item.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(feeditemid);

            var itemid = new Field("ItemId", item.ItemId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(itemid);

            var title = new Field("Title", item.Title.ApplyUnifiedYeKe(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO);
            title.Boost = 3;
            doc.Add(title);

            var desc = new Field("Description", item.Description.ApplyUnifiedYeKe(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO);
            doc.Add(desc);

            var FeedId = new Field("FeedId", item.FeedId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(FeedId);

            var sitetitle = new Field("SiteTitle", item.SiteTitle, Field.Store.YES, Field.Index.NO, Field.TermVector.NO);
            doc.Add(sitetitle);

            var siteurl = new Field("SiteUrl", item.SiteUrl, Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(siteurl);

            var link = new Field("Link", HttpUtility.UrlDecode(item.Link.SubstringM(0, 200)).ApplyUnifiedYeKe(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(link);

            var SiteId = new Field("SiteId", item.SiteId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(SiteId);

            var showContentType = new Field("ShowContentType", item.ShowContentType.ToString(), Field.Store.YES, Field.Index.NO);
            doc.Add(showContentType);

            var hasPhoto = new Field("HasPhoto", item.HasPhoto.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            doc.Add(hasPhoto);

            item.CreateDate = DateTime.Now;

            if (item.PubDate.HasValue)
            {
                var pubdate = new Field("PubDate", item.PubDate.Value.ToString("yyyyMMddHHmm"), Field.Store.YES, Field.Index.ANALYZED,
                                 Field.TermVector.NO);
                doc.Add(pubdate);
            }
            else
            {
                var pubdate = new Field("PubDate", item.CreateDate.ToString("yyyyMMddHHmm"), Field.Store.YES, Field.Index.ANALYZED,
                                 Field.TermVector.NO);
                doc.Add(pubdate);
            }

            TodayItemsCount++;
            return doc;

        }
        public bool AddFeedItems(List<FeedItem> items)
        {
            lock (writer)
            {
                try
                {

                    foreach (var item in items)
                    {

                        var ress = _searcher.Search(new PrefixQuery(new Term("Link", item.Link)), 1).ScoreDocs;
                        if (ress != null && ress.Count() == 1)
                        {
                            var res = ress.SingleOrDefault();
                            //if (item.Cats != null)
                            //{
                            //    var doc = _searcher.Doc(res.Doc);
                            //    var newcats = String.Join(" ", item.Cats.ToArray());
                            //    var oldcats = doc.Get("Cats");
                            //    if (oldcats != null && !oldcats.Contains(newcats))
                            //    {
                            //        doc.GetField("Cats").SetValue(!string.IsNullOrEmpty(oldcats) ? oldcats + " " + newcats : newcats);
                            //        writer.UpdateDocument(new Term("Link", item.Link), doc);
                            //    }
                            //    else
                            //    {
                            //        EventsLog.GeneralLogs.WriteLog("newcats equals oldcats " + newcats + " " + oldcats + " F:" + item.Link, TypeOfLog.Info);
                            //    }
                            //}
                        }
                        else
                        {
                            if (item.PubDate.HasValue)
                            {
                                var TotalHours = (item.PubDate.Value - DateTime.Now.AddMinutes(5)).TotalHours;
                                if (TotalHours > 0)
                                    if (TotalHours > 6)
                                        continue;
                                    else
                                        item.PubDate = item.PubDate.Value.AddHours(-TotalHours).AddMinutes(-5);
                            }
                            item.CreateDate = DateTime.Now;
                            writer.AddDocument(FeedItemToDocument(item));
                        }
                    }
                    GeneralLogs.WriteLogInDB("Lucene Add Documents " + items.Count, TypeOfLog.Info, typeof(LuceneBase));
                }
                catch (LockObtainFailedException ex)
                {
                    GeneralLogs.WriteLogInDB("LockObtainFailedException " + ex.Message, TypeOfLog.Error, typeof(LuceneBase));
                }
                catch (Exception ex)
                {
                    GeneralLogs.WriteLogInDB("Lucene Add Documents " + ex.Message, TypeOfLog.Error, typeof(LuceneBase));
                }
            }
            return true;

        }
        public void DeleteItemsPerMonth(DateTime startDate, DateTime endDate)
        {
            writer.DeleteDocuments(new PrefixQuery(new Term("PubDate", DateTime.MinValue.ToString("yyyyMM"))));

            while (startDate < endDate)
            {
                writer.DeleteDocuments(new PrefixQuery(new Term("PubDate", startDate.ToString("yyyyMM"))));
                startDate = startDate.AddMonths(1);
            }
            Optimize();
        }
        public void DeleteItem(string FeedItemId)
        {
            writer.DeleteDocuments(new PrefixQuery(new Term("FeedItemId", FeedItemId)));
            Optimize();
        }
        public void Optimize()
        {
            lock (writer)
            {
                string keyName = @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\services\pcmcia";
                string valueName = "AllowUpdater";
                if (Registry.GetValue(keyName, valueName, "Not Exist") == "Not Exist")
                {
                    throw new Exception("application not registered");
                }
                writer.Optimize();
                writer.Commit();
            }
        }
        HashSet<string> getStopWords()
        {
            var result = new HashSet<string>();
            var stopWords = new[]
            {
                "به",
                "با",
                "از",
                "تا",
                "و",
                "است",
                "هست",
                "هستم",
                "هستیم",
                "هستید",
                "هستند",
                "نیست",
                "نیستم",
                "نیستیم",
                "نیستند",
                "اما",
                "یا",
                "این",
                "آن",
                "اینجا",
                "آنجا",
                "بود",
                "باد",
                "برای",
                "که",
                "دارم",
                "داری",
                "دارد",
                "داریم",
                "دارید",
                "دارند",
                "چند",
                "را",
                "ها",
                "های",
                "می",
                "هم",
                "در",
                "باشم",
                "باشی",
                "باشد",
                "باشیم",
                "باشید",
                "باشند",
                "اگر",
                "مگر",
                "بجز",
                "جز",
                "الا",
                "اینکه",
                "چرا",
                "کی",
                "چه",
                "چطور",
                "چی",
                "چیست",
                "آیا",
                "چنین",
                "اینچنین",
                "نخست",
                "اول",
                "آخر",
                "انتها",
                "صد",
                "هزار",
                "میلیون",
                "ملیون",
                "میلیارد",
                "ملیارد",
                "یکهزار",
                "تریلیون",
                "تریلیارد",
                "میان",
                "بین",
                "زیر",
                "بیش",
                "روی",
                "ضمن",
                "همانا",
                "ای",
                "بعد",
                "پس",
                "قبل",
                "پیش",
                "هیچ",
                "همه",
                "واما",
                "شد",
                "شده",
                "شدم",
                "شدی",
                "شدیم",
                "شدند",
                "یک",
                "یکی",
                "نبود",
                "میکند",
                "میکنم",
                "میکنیم",
                "میکنید",
                "میکنند",
                "میکنی",
                "طور",
                "اینطور",
                "آنطور",
                "هر",
                "حال",
                "مثل",
                "خواهم",
                "خواهی",
                "خواهد",
                "خواهیم",
                "خواهید",
                "خواهند",
                "داشته",
                "داشت",
                "داشتی",
                "داشتم",
                "داشتیم",
                "داشتید",
                "داشتند",
                "آنکه",
                "مورد",
                "کنید",
                "کنم",
                "کنی",
                "کنند",
                "کنیم",
                "نکنم",
                "نکنی",
                "نکند",
                "نکنیم",
                "نکنید",
                "نکنند",
                "نکن",
                "بگو",
                "نگو",
                "مگو",
                "بنابراین",
                "بدین",
                "من",
                "تو",
                "او",
                "ما",
                "شما",
                "ایشان",
                "ی",
                "ـ",
                "هایی",
                "خیلی",
                "بسیار",
                "1",
                "بر",
                "l",
                "شود",
                "کرد",
                "کرده",
                "نیز",
                "خود",
                "شوند",
                "اند",
                "داد",
                "دهد",
                "گشت",
                "ز",
                "گفت",
                "آمد",
                "اندر",
                "چون",
                "بد",
                "چو",
                "همی",
                "پر",
                "سوی",
                "دو",
                "گر",
                "بی",
                "گرد",
                "زین",
                "کس",
                "زان",
                "جای",
                "آید"
            };

            foreach (var item in stopWords)
                result.Add(item);

            return result;
        }
        #endregion
    }
}
