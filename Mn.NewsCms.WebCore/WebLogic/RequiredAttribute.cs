using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;

namespace Mn.NewsCms.WebCore.WebLogic
{
    public class RequiredMnAttribute : RequiredAttributeAdapter
    {
        public RequiredMnAttribute(RequiredAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
            attribute.ErrorMessage = "وارد کردن مقدار {0} ضروری است";
        }
    }
}