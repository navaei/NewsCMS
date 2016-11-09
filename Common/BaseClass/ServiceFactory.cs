using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Mn.NewsCms.Common.BaseClass
{
    public class ServiceFactory
    {
        private static IServiceProvider _serviceProvider;
        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T Get<T>() where T : class
        {
            return _serviceProvider.GetService(typeof(T)) as T;
        }
    }
}
