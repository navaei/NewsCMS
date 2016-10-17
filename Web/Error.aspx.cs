using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Tazeyab.Web
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Write(Request.QueryString["aspxerrorpath"]);
                Response.Write("raw: " + Request.RawUrl.ToString());
            }
            catch { }
            try
            {
                Response.Write(Request.UrlReferrer.ToString());
            }
            catch { }
        }
    }
}