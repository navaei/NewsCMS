using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Mn.NewsCms.Common;

namespace Mn.NewsCms.Web.Controllers.Api
{
    public class CategoryController : ApiController
    {
        private readonly ICategoryBusiness _categoryBusiness;
        public CategoryController()
        {

        }
        public CategoryController(ICategoryBusiness categoryBusiness)
        {
            _categoryBusiness = categoryBusiness;
        }

        public List<Category> Get()
        {
            var result = _categoryBusiness.GetList().ToList();
            return result;

        }
    }
}
