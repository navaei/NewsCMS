using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Tazeyab.Web.WebLogic
{
    public class ExternalJavaScriptFileAttribute : ActionFilterAttribute
    {

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

            var response = filterContext.HttpContext.Response;

            response.Filter = new StripEnclosingScriptTagsFilter(response.Filter);

            response.ContentType = "application/javascript";

        }



        private class StripEnclosingScriptTagsFilter : MemoryStream
        {

            private readonly Stream _responseStream;

            private static readonly Regex _stripScriptTagsRegex;



            static StripEnclosingScriptTagsFilter()
            {

                _stripScriptTagsRegex = new Regex(@"^\s*<script[^>]*>(.*)</script>\s*$",

                    RegexOptions.Singleline);

            }



            public StripEnclosingScriptTagsFilter(Stream responseStream)
            {

                _responseStream = responseStream;

            }



            public override void Write(byte[] buffer, int offset, int count)
            {

                string response = Encoding.UTF8.GetString(buffer);



                response = _stripScriptTagsRegex.Replace(response, GetScriptTagContent);



                byte[] outdata = Encoding.UTF8.GetBytes(response);

                _responseStream.Write(outdata, 0, outdata.Length);

            }



            private static string GetScriptTagContent(Match match)
            {

                return match.Groups[1].Value.Trim();

            }

        }

    }
}