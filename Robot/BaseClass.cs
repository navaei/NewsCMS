using System;
using System.Collections.Generic;
using Tazeyab.Common;

namespace Tazeyab.CrawlerEngine
{
    public class ServiceFactory<TSeviceBase>
    {
        private static TSeviceBase Baseservice
        {
            get
            {
                var temp = System.Web.HttpRuntime.Cache[typeof(TSeviceBase).FullName];
                if (temp != null)
                    return (TSeviceBase)temp;
                else
                    throw new Exception();
            }
            set
            {
                System.Web.HttpRuntime.Cache[typeof(TSeviceBase).FullName] = value;
            }
        }

        public static TSeviceBase Create()
        {
            if (Baseservice == null)
                throw new Exception("No set baseservice,plz first call initial service");
            return Baseservice;
        }

        public static void Initial(TSeviceBase _baseservice)
        {
            Baseservice = _baseservice;
        }

        private ServiceFactory()
        { }
    }    
}
