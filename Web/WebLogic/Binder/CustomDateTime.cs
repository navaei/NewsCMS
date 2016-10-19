using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mn.NewsCms.Web.WebLogic.Binder
{
    public class DateTimeModelBinder : IModelBinder
    {
        #region Fields


        private readonly string _customFormat;

        #endregion

        #region Constructors and Destructors

        public DateTimeModelBinder(string customFormat)
        {
            this._customFormat = customFormat;
        }

        #endregion

        #region Explicit Interface Methods

        object IModelBinder.BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            return DateTime.ParseExact(value.AttemptedValue, this._customFormat, CultureInfo.InvariantCulture);
        }

        #endregion
    }
}