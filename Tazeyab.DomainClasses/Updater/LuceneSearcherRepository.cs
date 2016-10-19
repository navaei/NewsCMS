using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.DomainClasses.UpdaterBusiness
{
    public class LuceneSearcherRepository : LuceneBase, IRepositorySearcher
    {

        #region Search
        public static List<FeedItem> VisitedItems = new List<FeedItem>();
        public List<Guid> Search(string searchQuery, int PageSize, int PageIndex, LuceneSearcherType SearchType, LuceneSortField SortField, bool hasPhoto)
        {
            try
            {
                var photoQuery = new TermQuery(new Term("HasPhoto", hasPhoto.ToString()));
                var results = new List<ScoreDoc>();
                var res = new List<Guid>();
                // validation
                if (string.IsNullOrEmpty(searchQuery) || string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", "")))
                    return new List<Guid>();
                var Content = searchQuery.Trim();

                if (SortField != LuceneSortField.PubDate)
                    sort = new Sort(new SortField(SortField.ToString(), System.Globalization.CultureInfo.CurrentCulture, true));

                // set up lucene searcher
                //using (var _searcher = new IndexSearcher(_directory, false))
                //{
                var analyzer = new StandardAnalyzer(Currentversion);

                if (SearchType == LuceneSearcherType.Key)
                {
                    if (Content.Contains("|"))
                    {
                        var fields = new[] { "Title", "Description" };
                        var queryParser = new MultiFieldQueryParser(Currentversion, fields, analyzer);
                        //var query = queryParser.Parse(Content.Replace('|', ' '));
                        var query = queryParser.Parse(Content.Replace(" ", "+").Replace('|', ' ') + (hasPhoto ? ",HasPhoto:True" : ""));
                        BooleanQuery innerExpr = new BooleanQuery();
                        innerExpr.Add(query, Occur.MUST);
                        if (hasPhoto)
                            innerExpr.Add(photoQuery, Occur.MUST);

                        results = _searcher.Search(innerExpr, null, (PageIndex + 1) * PageSize, sort).ScoreDocs.ToList();
                        if (results.Count < PageSize)
                        {
                            QueryParser oParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Title", analyzer);
                            string desc = "", finalQuery = "";
                            desc = " OR (Description:" + Content + ")";
                            finalQuery = "(" + Content + desc + ")";
                            query = oParser.Parse(finalQuery);
                            results = _searcher.Search(query, null, (PageIndex + 1) * PageSize, Sort.RELEVANCE).ScoreDocs.ToList();
                        }
                    }
                    else if (Content.Contains(" "))
                    {
                        var fields = new[] { "Title", "Description" };
                        var queryParser = new MultiFieldQueryParser(Currentversion, fields, analyzer);
                        //var query = queryParser.Parse("\"" + Content + "\"");
                        var query = queryParser.Parse("\"" + Content + "\"");
                        BooleanQuery innerExpr = new BooleanQuery();
                        innerExpr.Add(query, Occur.MUST);
                        if (hasPhoto)
                            innerExpr.Add(photoQuery, Occur.MUST);

                        results = _searcher.Search(innerExpr, null, (PageIndex + 1) * PageSize, sort).ScoreDocs.ToList();
                        if (results.Count < PageSize)
                        {
                            QueryParser oParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Title", analyzer);
                            string desc = "", finalQuery = "";
                            desc = " OR (Description:" + Content + ")";
                            finalQuery = "(" + Content + desc + ")";
                            query = oParser.Parse(finalQuery);
                            results = _searcher.Search(query, null, (PageIndex + 1) * PageSize, Sort.RELEVANCE).ScoreDocs.ToList();
                        }
                    }
                    else
                    {
                        var query = new PrefixQuery(new Term("Title", Content.ToString()));
                        BooleanQuery innerExpr = new BooleanQuery();
                        innerExpr.Add(query, Occur.MUST);
                        if (hasPhoto)
                            innerExpr.Add(photoQuery, Occur.MUST);

                        results = _searcher.Search(innerExpr, null, (PageIndex + 1) * PageSize, sort).ScoreDocs.ToList();
                        //if (results.Count < (PageIndex + 1) * PageSize)
                        //    results = _searcher.Search(new PrefixQuery(new Term("Description", Content.ToString())), null, (PageIndex + 1) * PageSize, sort).ScoreDocs.ToList();
                    }
                }
                var resultsId = results.Skip(PageIndex * PageSize).Take(PageSize);
                foreach (var doc in resultsId)
                {
                    var tempdoc = _searcher.Doc(doc.Doc);
                    res.Add(Guid.Parse(tempdoc.Get("FeedItemId")));
                }

                return res;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("write.lock"))
                {
                    try
                    {
                        var lockFilePath = Path.Combine(LuceneDir, "write.lock");
                        if (File.Exists(lockFilePath))
                            File.Delete(lockFilePath);
                    }
                    catch { }
                }
                return new List<Guid>();
            }
        }
        public FeedItem GetItem(string FeedItemId, long? ItemId)
        {
            return GetItem(FeedItemId, ItemId, false);
        }
        public FeedItem GetItem(string FeedItemId, long? ItemId, bool DirectRequest)
        {

            using (IndexSearcher _searcher = new IndexSearcher(_directory, false))
            {
                var item = new FeedItem();
                TermQuery TermQuery;
                TermQuery = new TermQuery(new Term("FeedItemId", FeedItemId));
                var doc = _searcher.Search(TermQuery, 1).ScoreDocs.FirstOrDefault();
                if (doc != null)
                {

                    var tempdoc = _searcher.Doc(doc.Doc);
                    item.Id = Guid.Parse(tempdoc.Get("FeedItemId"));
                    //long temp = 0;
                    //if (long.TryParse(tempdoc.Get("ItemId"), out temp))
                    //    item.ItemId = temp;
                    item.Title = tempdoc.Get("Title");
                    //item.Description = tempdoc.Get("Description");
                    item.SiteTitle = tempdoc.Get("SiteTitle");
                    item.SiteUrl = tempdoc.Get("SiteUrl");
                    //item.SiteId = long.Parse(tempdoc.Get("SiteId"));
                    item.PubDate = DateTime.ParseExact(tempdoc.Get("PubDate"), "yyyyMMddHHmm", System.Globalization.CultureInfo.InvariantCulture);
                    item.Link = tempdoc.Get("Link");
                    item.ShowContentType = !string.IsNullOrEmpty(tempdoc.Get("ShowContentType")) ? (ShowContent)Enum.Parse(typeof(ShowContent), tempdoc.Get("ShowContentType")) : ShowContent.Inner;
                    //try
                    //{
                    //    if (!string.IsNullOrEmpty(tempdoc.Get("Cats")))
                    //        item.Cats = tempdoc.Get("Cats").Split(' ').Select(c => int.Parse(c)).ToList();
                    //}
                    //catch
                    //{
                    //    if (!string.IsNullOrEmpty(tempdoc.Get("Cats")))
                    //        item.Cats = tempdoc.Get("Cats").Split(',').Select(c => int.Parse(c)).ToList();
                    //}
                }
                //else
                //{
                //    TazehaContext context = new TazehaContext();
                //    var items = context.ItemVisiteds.Where(i => i.FeedItemId == FeedItemId && !string.IsNullOrEmpty(i.Link)).
                //        Select(x => new FeedItem { FeedItemId = x.FeedItemId, Title = x.Title, Description = x.Title, Link = x.Link });
                //    if (items.Any())
                //        item = items.First();
                //    if (string.IsNullOrEmpty(item.SiteUrl))
                //        item.SiteUrl = "v";
                //}

                //if (DirectRequest && item != null)
                //if (!string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent) && !HttpContext.Current.Request.UserAgent.Contains("bot"))
                //    IncreaseVisitCount(new ItemVisited()
                //    {
                //        CreationDate = item.CreateDate.Year > 1000 ? item.CreateDate : (item.PubDate.HasValue ? item.PubDate.Value : DateTime.Now),
                //        EntityCode = string.Empty,
                //        EntityRef = 0,
                //        FeedItemId = item.FeedItemId,
                //        Title = item.Title,
                //        VisitCount = 1,
                //        PubDate = item.PubDate,
                //        Link = item.Link,
                //        SiteId = item.SiteId,
                //        FeedId = item.FeedId,
                //    });
                return item;
            }
        }

        //private static void UpdateVisitedItems(List<ItemVisited> visitedItems)
        //{
        //    using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["TazehaContext"].ConnectionString))
        //    {
        //        con.Open();
        //        using (SqlCommand cmd = new SqlCommand("exec sp_ItemVisitUpdate @list", con))
        //        {
        //            var table = new DataTable();
        //            table.Columns.Add("FeedItemId", typeof(string));
        //            table.Columns.Add("Title", typeof(string));
        //            table.Columns.Add("Link", typeof(string));
        //            table.Columns.Add("VisitCount", typeof(int));
        //            table.Columns.Add("EntityCode", typeof(string));
        //            table.Columns.Add("EntityRef", typeof(int));
        //            table.Columns.Add("PubDate", typeof(DateTime));
        //            foreach (var item in visitedItems)
        //                table.Rows.Add(item.FeedItemId,
        //                    item.Title,
        //                    item.Link,
        //                    item.VisitCount,
        //                    item.EntityCode,
        //                    item.EntityRef,
        //                    item.PubDate); //table.Rows.Add(item.FeedItemId, item.VisitsCount, item.Cats);

        //            var pList = new SqlParameter("@list", SqlDbType.Structured);
        //            pList.TypeName = "dbo.ItemDictionary";
        //            pList.Value = table;
        //            cmd.Parameters.Add(pList);
        //            cmd.ExecuteNonQuery();
        //        }
        //        con.Close();
        //    }
        //}
        #endregion

    }
}