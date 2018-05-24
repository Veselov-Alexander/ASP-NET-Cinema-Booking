using iTextSharp.text;
using iTextSharp.text.pdf;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinema.Account
{
    public partial class Profile : System.Web.UI.Page
    {
        private string name;
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie loginCookie = Request.Cookies["login"];
            HttpCookie signCookie = Request.Cookies["sign"];

            if (!MySQLServer.auth_chech(Request))
            {
                Response.Redirect("../Accounts/Login.aspx");
                return;
            }
            name = loginCookie.Value.ToUpper();
            login.Text = "<h1>" + name + "</h1>";

            loadMeta();


            //QRCode();
        }

        private void loadMeta()
        {
            List<string> items = MySQLServer.get_orders_information(name);
            orders.Text += String.Format("<tr><th>Film title</th><th>Date and time</th><th>Places</th><th>Cost</th><th>Status</th><th>Get PDF</th></tr>");
            for (int i = 0; i < items.Count / 7; ++i)
            {
                string id = items[i * 7 + 0];
                string user = items[i * 7 + 1];
                string film = items[i * 7 + 2];
                string date = items[i * 7 + 3];
                string status = items[i * 7 + 4];
                string cost = items[i * 7 + 5];
                string places = items[i * 7 + 6];
                string PDF = "<a runat=\"server\" class = \"pdf\" href=\"Ticket.ashx?id=" + id + "\">Download</a>";
                if (status != "confirmed")
                {
                    PDF = "";
                }
                orders.Text += String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{4}</td><td class=\"flex\">{3}</td><td>{5}<td></tr>", film, date, places, status, cost, PDF);
            }
            orders.Text = "<table  cellspacing=\"0\">" + orders.Text + "</table>";
        }
    }
}