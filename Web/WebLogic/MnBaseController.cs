using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mn.NewsCms.Web.WebLogic
{
    public abstract class MnBaseController : Controller
    {
        protected string DefaultDateFormat
        {
            get
            {
                return "";//MM/dd/yyyy
            }
        }
        #region BaseMethod
        // Indicate if it is IframeMode 
        //public bool IsIframeMode
        //{
        //    get
        //    {
        //        return (Request["iframeMode"] == "true" ? true : false);
        //    }
        //}

        protected override IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, AsyncCallback callback, object state)
        {
            // ViewBag.Mode fill changing layout
            //if (requestContext.HttpContext.Request["iframeMode"] != null || requestContext.RouteData.Values["iframeMode"] != null)
            //{
            //    ViewBag.IframeMode = "true";
            //}

            //try
            //{                
            //    Session["CurrentRoleName"] = CurrentUserRole();
            //}
            //catch (Exception)
            //{

            //}

            return base.BeginExecute(requestContext, callback, state);
        }

        #endregion      

        #region Override Method
        protected JsonNetResult JsonNet(object data, JsonSerializerSettings serializerSettings, string dateFormat)
        {
            var result = new JsonNetResult
            {
                Data = data,
                ContentEncoding = System.Text.Encoding.Unicode,
                DateFormat = dateFormat,
                SerializerSettings = serializerSettings
            };
            return result;
        }
        protected JsonNetResult JsonNet(object data, string dateFormat)
        {
            var result = new JsonNetResult
            {
                Data = data,
                ContentEncoding = System.Text.Encoding.Unicode,
                DateFormat = dateFormat,
                SerializerSettings = new JsonSerializerSettings()
            };
            return result;
        }
        protected JsonNetResult JsonNet(object data, JsonSerializerSettings serializerSettings)
        {
            return JsonNet(data, serializerSettings, DefaultDateFormat);
        }
        protected JsonNetResult JsonNet(object data)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = false });
            return JsonNet(data, serializerSettings, DefaultDateFormat);
        }

        #endregion
    }
    public class JsonNetResult : ActionResult
    {
        public string DateFormat { get; set; }
        public System.Text.Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public JsonNetResult()
        {

        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType)
              ? ContentType
              : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {

                JsonTextWriter writer = new JsonTextWriter(response.Output) { Formatting = Formatting, DateFormatString = DateFormat };

                JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);

                writer.Flush();
            }
        }
    }
}