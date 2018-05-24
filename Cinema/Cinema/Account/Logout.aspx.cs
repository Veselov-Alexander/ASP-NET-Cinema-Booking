using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinema.Account
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie loginCookie = new HttpCookie("login", string.Empty);
            HttpCookie signCookie = new HttpCookie("sign", string.Empty);
            HttpCookie roleCookie = new HttpCookie("role", string.Empty);
            FormsAuthentication.SignOut();

            loginCookie.Expires = DateTime.Now.AddDays(-1);
            signCookie.Expires = DateTime.Now.AddDays(-1);
            roleCookie.Expires = DateTime.Now.AddDays(-1);

            Response.Cookies.Add(loginCookie);
            Response.Cookies.Add(signCookie);
            Response.Cookies.Add(roleCookie);

            Response.Redirect("../Default.aspx");
        }
    }
}