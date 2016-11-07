using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Mn.NewsCms.WebCore.WebLogic.Binder
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

      

        #endregion

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            return
                new Task<DateTime>(
                    () => DateTime.ParseExact(value.Values, this._customFormat, CultureInfo.InvariantCulture));
        }
    }
}