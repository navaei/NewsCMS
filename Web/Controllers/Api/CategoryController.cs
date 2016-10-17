using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tazeyab.Common;

namespace Tazeyab.Web.Controllers.Api
{
    public class CategoryController : ApiController
    {
        public List<Category> Get()
        {
            var result = ServiceFactory.Get<ICategoryBusiness>().GetList().ToList();
            return result;

        }
    }
}
