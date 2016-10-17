using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace CrawlerEngine.Helper
{
    public class HtmlMetaParser
    {
        public enum RobotHtmlMeta { None = 0, NoIndex, NoFollow, NoIndexNoFollow };
        public static List<HtmlMeta> Parse(string htmldata)
        {
            Regex metaregex =
                new Regex(@"<meta\s*(?:(?:\b(\w|-)+\b\s*(?:=\s*(?:""[^""]*""|'" +
                          @"[^']*'|[^""'<> ]+)\s*)?)*)/?\s*>",
                          RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

            List<HtmlMeta> MetaList = new List<HtmlMeta>();
            foreach (Match metamatch in metaregex.Matches(htmldata))
            {
                HtmlMeta mymeta = new HtmlMeta();

                Regex submetaregex =
                    new Regex(@"(?<name>\b(\w|-)+\b)\s*=\s*(""(?<value>" +
                              @"[^""]*)""|'(?<value>[^']*)'|(?<value>[^""'<> ]+)\s*)+",
                              RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

                foreach (Match submetamatch in submetaregex.Matches(metamatch.Value.ToString()))
                {
                    if ("http-equiv" == submetamatch.Groups["name"].ToString().ToLower())
                        mymeta.HttpEquiv = submetamatch.Groups["value"].ToString();

                    if (("name" == submetamatch.Groups["name"].ToString().ToLower())
                        && (mymeta.HttpEquiv == String.Empty))
                    {
                        // if already set, HTTP-EQUIV overrides NAME
                        mymeta.Name = submetamatch.Groups["value"].ToString();
                    }
                    if ("content" == submetamatch.Groups["name"].ToString().ToLower())
                    {
                        mymeta.Content = submetamatch.Groups["value"].ToString();
                        MetaList.Add(mymeta);
                    }
                }
            }
            return MetaList;
        }
        public static string getOGImageMetaContent(string htmldata)
        {
            Regex metaregex =
                new Regex(@"<meta\s*(?:(?:\b(\w|-)+\b\s*(?:=\s*(?:""[^""]*""|'" +
                          @"[^']*'|[^""'<> ]+)\s*)?)*)/?\s*>",
                          RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

            List<HtmlMeta> MetaList = new List<HtmlMeta>();
            foreach (Match metamatch in metaregex.Matches(htmldata))
            {
                HtmlMeta mymeta = new HtmlMeta();

                Regex submetaregex =
                    new Regex(@"(?<name>\b(\w|-)+\b)\s*=\s*(""(?<value>" +
                              @"[^""]*)""|'(?<value>[^']*)'|(?<value>[^""'<> ]+)\s*)+",
                              RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                if (metamatch.Groups[0].Value.IndexOfX("og:image") > 0 || metamatch.Groups[0].Value.IndexOfX("image_src") > 0)
                {
                    foreach (Match submetamatch in submetaregex.Matches(metamatch.Value.ToString()))
                    {
                        if ("http-equiv" == submetamatch.Groups["name"].ToString().ToLower())
                            mymeta.HttpEquiv = submetamatch.Groups["value"].ToString();

                        if (("name" == submetamatch.Groups["name"].ToString().ToLower())
                            && (mymeta.HttpEquiv == String.Empty))
                        {
                            // if already set, HTTP-EQUIV overrides NAME
                            mymeta.Name = submetamatch.Groups["value"].ToString();
                        }
                        if ("content" == submetamatch.Groups["name"].ToString().ToLower())
                        {
                            mymeta.Content = submetamatch.Groups["value"].ToString();
                            return mymeta.Content;
                        }
                    }
                }                
            }
            //------------------FOR IMAGE_SRC -----------
            string reg = @"<link(.*)((\brel=""image_src"")|(\brel='image_src'))(.*)\bhref=(.*) />";
            Regex metaregex2 =
                new Regex(reg,
                          RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

            foreach (Match itemmatch in metaregex2.Matches(htmldata))
            {
                string href = Regex.Match(itemmatch.Groups[0].Value, "href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture).Groups[0].Value.Replace("href=", "");
                if (!string.IsNullOrEmpty(href))
                    return href;
            }

            return string.Empty;
        }
        public static RobotHtmlMeta ParseRobotMetaTags(string htmldata)
        {
            List<HtmlMeta> MetaList = HtmlMetaParser.Parse(htmldata);

            RobotHtmlMeta result = RobotHtmlMeta.None;
            foreach (HtmlMeta meta in MetaList)
            {
                if (meta.Name.ToLower().IndexOf("robots") != -1 || meta.Name.ToLower().IndexOf("robot") != -1)
                {
                    string content = meta.Content.ToLower();
                    if (content.IndexOf("noindex") != -1 && content.IndexOf("nofollow") != -1)
                    {
                        result = RobotHtmlMeta.NoIndexNoFollow;
                        break;
                    }
                    if (content.IndexOf("noindex") != -1)
                    {
                        result = RobotHtmlMeta.NoIndex;
                        break;
                    }
                    if (content.IndexOf("nofollow") != -1)
                    {
                        result = RobotHtmlMeta.NoFollow;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
