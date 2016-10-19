using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mn.NewsCms.DomainClasses.ContentManagment
{
    public class Live
    {
        static int _OnlineUser = 1;
        public static int OnlineUser
        {
            get
            {
                return _OnlineUser;
            }
            set {
                _OnlineUser = value;
            }
        }
        //public static void IncreaseUser() { 
        //return HttpContext.Current.Application["OnlineUser"]=int.Parse(return HttpContext.Current.Application["OnlineUser"]) +1;
        //}
        //public static void DecreaseUser() { 
        // HttpContext.Current.Application["OnlineUser"]=int.Parse(return HttpContext.Current.Application["OnlineUser"]) -1;
        //}
    }
}