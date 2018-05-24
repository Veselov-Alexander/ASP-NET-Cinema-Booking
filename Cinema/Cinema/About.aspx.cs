using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinema
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<List<string>> items = MySQLServer.custom_query("select Film, count(Film) 'Booking amount', round(avg(cost)) 'Average order cost' from order_information group by Film");
            orders.Text += "<tr>";
            int column_count = items[0].Count;
            for (int i=0; i< column_count; ++i)
            {
                orders.Text += "<th>"+ items[0][i] + "</th>";
            }
            orders.Text += "</tr>";       
            for (int i = 1; i < items.Count; ++i)
            {
                orders.Text += "<tr>";
                for (int j = 0; j < column_count; ++j)
                {
                    orders.Text += "<td>" + items[i][j] + "</td>";
                }
                orders.Text += "</tr>";
            }
            orders.Text = "<table  cellspacing=\"0\">" + orders.Text + "</table>";
        }
    }
}