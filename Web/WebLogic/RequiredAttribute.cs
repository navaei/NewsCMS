using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tazeyab.Web.WebLogic
{ 
    public class RequiredMnAttribute : RequiredAttributeAdapter
    {
        public RequiredMnAttribute(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)
            : base(metadata, context, attribute)
        {           
            attribute.ErrorMessage = "وارد کردن مقدار {0} ضروری است";
        }
    }
}