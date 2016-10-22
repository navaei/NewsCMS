using System;
using System.Web;
using Microsoft.Practices.Unity;

namespace Mn.NewsCms.Web.WebLogic
{
    public class PerHttpRequestLifetime : LifetimeManager
    {
        // This is very important part and the reason why I believe mentioned
        // PerCallContext implementation is wrong.
        object tempContext;
        private readonly Guid _key = Guid.NewGuid();
        private HttpContext currentHttpReq;
        public override object GetValue()
        {
            if (HttpContext.Current == null)
                HttpContext.Current = currentHttpReq;
            if (HttpContext.Current != null)
                return HttpContext.Current.Items[_key];
            else
                return tempContext;
        }

        public override void SetValue(object newValue)
        {
            currentHttpReq = HttpContext.Current;
            if (currentHttpReq != null)
                HttpContext.Current.Items[_key] = newValue;
            else
                tempContext = newValue;
        }

        public override void RemoveValue()
        {
            var obj = GetValue();
            if (HttpContext.Current != null)
                HttpContext.Current.Items.Remove(obj);
            else
                tempContext = null;
        }
    }
}