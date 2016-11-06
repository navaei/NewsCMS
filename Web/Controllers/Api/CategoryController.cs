using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.BaseClass;

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
            var result = ServiceFactory.Get<ICategoryBusiness>().GetList().ToList();
            return result.Select(c => new Category
            {
                Id = c.Id,
                Title = c.Title,
                Code = c.Code,
                ParentId = c.ParentId
            }).ToList();
        }
    }
}
