using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinema.Manage
{
    public partial class AddShow : System.Web.UI.Page
    {
        List<string> films_id;
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
            List<string> items = MySQLServer.get_films();
            films_id = new List<string>();
            foreach (string item in items)
            {
                films.Items.Add(new ListItem(item.ToString().Split('&')[1]));
                films_id.Add(item.ToString().Split('&')[0]);
            }
        }

        protected void Check(object sender, EventArgs e)
        {
            time.Items.Clear();
            string date = Calendar1.SelectedDate.ToShortDateString();
            string mySQLDate = date.Split('.')[2] +
                               '-' + date.Split('.')[1] +
                               '-' + date.Split('.')[0];
            Debug.WriteLine(mySQLDate);

            List<string> items = MySQLServer.get_times(mySQLDate);
            bool[] used = new bool[12];
            foreach (string item in items)
            {
                int begin =int.Parse(item.Split(':')[0]);


                for(int hour = begin-1; hour < begin+2; ++hour)
                {
                    if (hour - 10 >= 0)
                    {
                        used[hour - 10] = true;
                    }
                }

            }
            for (int hour = 10; hour < 22; ++hour)
            {
                if (!used[hour - 10])
                {
                    string h = hour.ToString() + ":00:00";
                    time.Items.Add(new ListItem(h));
                }
            }
        }

        protected async void submit_Click(object sender, EventArgs e)
        {
            string date = Calendar1.SelectedDate.ToShortDateString();
            string mySQLDate = date.Split('.')[2] +
                   '-' + date.Split('.')[1] +
                   '-' + date.Split('.')[0] +
                   " " +
                   time.SelectedValue;
            await MySQLServer.add_session(films_id[films.SelectedIndex], mySQLDate, cost.SelectedValue);
            Response.Redirect("~/Shows/", false);
        }
    }
}