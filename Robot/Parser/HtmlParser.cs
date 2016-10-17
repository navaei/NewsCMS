using System;
using System.Text;
using HtmlAgilityPack;
using Tazeyab.Common.EventsLog;

namespace Tazeyab.CrawlerEngine.Parser
{
    public class HtmlParser
    {
        public static string GetTagAttribute(string Content, string Pattern, string AttributeName)
        {

            HtmlDocument doc = new HtmlDocument()
            {
                OptionCheckSyntax = true,
                OptionFixNestedTags = true,
                OptionAutoCloseOnEnd = true,
                OptionDefaultStreamEncoding = Encoding.UTF8
            };
            if (Content != null)
                doc.Load(Content, Encoding.UTF8);
            return GetTagAttribute(doc, Pattern, AttributeName);
        }
        public static string GetTagAttribute(HtmlDocument doc, string Pattern, string AttributeName)
        {
            try
            {
                HtmlNode Tag = doc.DocumentNode.SelectSingleNode("//" + Pattern);
                if (Tag.GetAttributeValue(AttributeName, string.Empty) != null)
                {
                    return Tag.GetAttributeValue(AttributeName, string.Empty);
                }
            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLog("Error GetImageStreamByPattern" + ex.Message);
            }
            return null;
        }
        public static HtmlNodeCollection GetTags(string content, string Pattern)
        {
            try
            {
                var doc = new HtmlDocument();
                //{
                //    OptionCheckSyntax = true,
                //    OptionFixNestedTags = true,
                //    OptionAutoCloseOnEnd = true,
                //    OptionDefaultStreamEncoding = Encoding.UTF8
                //};
                if (content != null)
                    doc.LoadHtml(content);

                return doc.DocumentNode.SelectNodes(Pattern);
            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLog("GetImageStreamByPattern" + ex.Message, Common.Models.TypeOfLog.Error, typeof(HtmlParser));
            }
            return null;
        }
    }
}
