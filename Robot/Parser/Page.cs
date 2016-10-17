using System;
using System.Text.RegularExpressions;
using Tazeyab.Common.EventsLog;

namespace Tazeyab.CrawlerEngine
{
    public class Page
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Page() { }
        public Page(string PageText)
        {
            this.Content = PageText;
            setTitle();
            setMetaTags();
            setDesc();
            setLogo();
        }
        #endregion
        #region Private Instance Fields

        private int _size;
        private string _text;
        private string _url;
        private int _viewstateSize;
        private string _title;
        private string _fulltitle;
        private string _desc;
        private string _keyWord;
        private string _siteLogo;
        #endregion
        #region Public Properties

        public int Size
        {
            get { return _size; }
        }

        public string Content
        {
            get { return _text; }
            set
            {
                _text = value;
                _size = value.Length;
            }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value.SubstringX(0, 200); }
        }

        public int ViewstateSize
        {
            get { return _viewstateSize; }
            set { _viewstateSize = value; }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
            }
        }
        public string FullTitle
        {
            get { return _fulltitle; }
            set
            {
                _fulltitle = value;
            }
        }
        public string Description
        {
            get { return _desc; }
        }
        public string KeyWord
        {
            get { return _keyWord; }
        }
        public string SiteLogo
        {
            get { return _siteLogo; }
        }
        #endregion

        public void CalculateViewstateSize()
        {
            int startingIndex = Content.IndexOf("id=\"__VIEWSTATE\"");
            if (startingIndex > -1)
            {
                int indexOfViewstateValueNode = Content.IndexOf("value=\"", startingIndex);
                int indexOfClosingQuotationMark = Content.IndexOf("\"", indexOfViewstateValueNode + 7);
                string viewstateValue = Content.Substring(indexOfViewstateValueNode + 7, indexOfClosingQuotationMark - (indexOfViewstateValueNode + 7));

                ViewstateSize = viewstateValue.Length;
            }
        }
        private void setTitle()
        {
            try
            {
                int startingIndex = Content.IndexOfX("<Title>");
                if (startingIndex > -1)
                {
                    int indexOfClosingQuotationMark = Content.IndexOfX("</Title>");
                    _fulltitle = Content.Substring(startingIndex + 7, indexOfClosingQuotationMark - (startingIndex + 7));
                    _fulltitle = Helper.HtmlRemoval.StripTagsRegex(_fulltitle);
                    char[] splitor = { '_', '*', '-', '/', '\\', '&', '@', '#', '~', '(', ')', '+', '=', ':' };
                    string[] arr = _fulltitle.Split(splitor);
                    _title = arr[0].SubstringX(0, 150).Length > 5 ? arr[0].SubstringX(0, 150) : _fulltitle.SubstringX(0, 150);
                }
            }
            catch(Exception ex) {
                GeneralLogs.WriteLog(ex.Message);
            }
        }
        private void setDesc()
        {
            try
            {
                int startingIndex = Content.IndexOfX("id=\"description\"");
                if (startingIndex == -1)
                    startingIndex = Content.IndexOfX("name=\"description\"");
                if (startingIndex > -1)
                {
                    int indexOfViewstateValueNode = Content.IndexOf("content=\"", startingIndex);
                    int indexOfClosingQuotationMark = Content.IndexOf("\"", indexOfViewstateValueNode + 9);
                    _desc = Content.Substring(indexOfViewstateValueNode + 9, indexOfClosingQuotationMark - (indexOfViewstateValueNode + 9)).SubstringX(0, 300);
                    _desc = Helper.HtmlRemoval.StripTagsRegex(_desc);
                }
            }
            catch(Exception ex) {
                GeneralLogs.WriteLog(ex.Message);
            }
        }
        private void setMetaTags()
        {
            int startingIndex = Content.IndexOfX("id=\"keywords\"");
            if (startingIndex == -1)
                startingIndex = Content.IndexOfX("name=\"keywords\"");
            if (startingIndex > -1)
            {
                int indexOfViewstateValueNode = Content.IndexOf("content=\"", startingIndex);
                int indexOfClosingQuotationMark = Content.IndexOf("\"", indexOfViewstateValueNode + 9);
                _keyWord = Content.Substring(indexOfViewstateValueNode + 9, indexOfClosingQuotationMark - (indexOfViewstateValueNode + 9)).SubstringX(0, 300);
            }
        }
        private void setLogo()
        {
            try
            {
                foreach (Match match in Regex.Matches(Content, "<link .*? href=\"(.*?.ico)\""))
                {
                    _siteLogo = match.Groups[1].Value;
                    break;

                }
            }
            catch { }
        }
    }
}
