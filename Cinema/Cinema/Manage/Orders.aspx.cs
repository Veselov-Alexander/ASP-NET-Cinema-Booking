using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinema.Manage
{
    public partial class Orders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(MySQLServer.auth_chech(Request) && MySQLServer.admin_check(Request)))
            {
                Response.Redirect("~/", false);
            }
            loadMeta();
        }

        private void loadMeta()
        {
            List<string> items = MySQLServer.get_orders_information();
            orders.Text += String.Format("<tr><th>User</th><th>Film title</th><th>Date and time</th><th>Places</th><th>Cost</th><th>Status</th></tr>");
            for (int i = 0; i < items.Count/7; ++i)
            {
                string id = items[i * 7 + 0];
                string user = items[i * 7 + 1];
                string film = items[i * 7 + 2];
                string date = items[i * 7 + 3];
                string status = items[i * 7 + 4];
                string cost = items[i * 7 + 5];
                string places = items[i * 7 + 6];
                if (status == "pending confirmation")
                {
                    orders.Text += String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{5}</td><td class=\"flex\"><input class=\"sub-button\" onClick=\"approve(this)\" id=\"a{4}\" type = \"submit\" value=\"Accept\"/><input class=\"dec-button\" onClick=\"approve(this)\" id=\"d{4}\" type = \"submit\" value=\"Decline\"/></td></tr>", user, film, date, places, id, cost);
                }
                else
                {
                    orders.Text += String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{5}</td><td class=\"flex\">{4}</td></tr>", user, film, date, places, status, cost);
                }
            }
            orders.Text = "<table  cellspacing=\"0\">" + orders.Text + "</table>";
        }

        protected void HiddenField1_ValueChanged(object sender, EventArgs e)
        {
            string id = HiddenField1.Value;
            if (id[0] == 'a')
            {
                MySQLServer.confirm(id.Substring(1));
            }
            else
            {
                MySQLServer.decline(id.Substring(1));
            }
            Response.Redirect("~/Manage/Orders");   
        }
    }
}