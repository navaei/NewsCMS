using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mn.NewsCms.Web.WebLogic.Binder
{
    public class PersianDateModelBinder2 : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                throw new Exception("ffgfgg");

                var parts = valueResult.AttemptedValue.Split('/'); //ex. 1391/1/19
                if (parts.Length != 3) return null;



                int year = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                int day = int.Parse(parts[2]);
                actualValue = new DateTime(year, month, day, new PersianCalendar());
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }

    }
    public class PersianDateModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
        //public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //{
        //    var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        //    if (valueResult == null)
        //        return null;

        //    var modelState = new ModelState { Value = valueResult };
        //    object actualValue = null;
        //    try
        //    {               
        //        var parts = valueResult.AttemptedValue.Split('/'); //ex. 1391/1/19
        //        if (parts.Length != 3) return null;

        //        int year = int.Parse(parts[0]);
        //        int month = int.Parse(parts[1]);
        //        int day = int.Parse(parts[2]);
        //        actualValue = new DateTime(year, month, day, new PersianCalendar());
        //    }
        //    catch (FormatException e)
        //    {
        //        modelState.Errors.Add(e);
        //    }

        //    bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
        //    return actualValue;
        //}
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.PropertyType == typeof(DateTime) || propertyDescriptor.PropertyType == typeof(Nullable<DateTime>))
            {
                var val = propertyDescriptor.GetValue(bindingContext.Model);
                if (val != null)
                {
                    var parts = val.ToString().Split('/');
                    if (parts.Length == 3)
                    {
                        int year = int.Parse(parts[0]);
                        int month = int.Parse(parts[1]);
                        int day = int.Parse(parts[2]);
                        var actualValue = new DateTime(year, month, day, new PersianCalendar());
                        propertyDescriptor.SetValue(bindingContext.Model, actualValue);
                    }
                }
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
            // bind the other properties here
        }
    }
}