using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinema.Manage
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(MySQLServer.auth_chech(Request) && MySQLServer.admin_check(Request)))
            {
                Response.Redirect("~/", false);
            }
        }
    }
}