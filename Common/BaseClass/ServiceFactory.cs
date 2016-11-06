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
        static IUnityContainer _unityContainer;

        public static void Initialize(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public static T Get<T>()
        {
            return _unityContainer.Resolve<T>();
        }
    }
}
