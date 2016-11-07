using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Practices.Unity;
using Microsoft.AspNetCore.Http.Extensions;
using Mn.NewsCms.Common.BaseClass;

namespace Mn.NewsCms.WebCore.WebLogic
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
            return ServiceFactory.Get<ICacheManager>().Get<object>(_key.ToString());
        }

        public override void SetValue(object newValue)
        {
            ServiceFactory.Get<ICacheManager>().Set(_key.ToString(), newValue, 5);
        }

        public override void RemoveValue()
        {
            ServiceFactory.Get<ICacheManager>().Remove(_key.ToString());
        }
    }
}