using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinema.Shows
{
    public partial class Book : System.Web.UI.Page
    {
        String id, time, date;
        String session_id, cost, title;

        protected void Page_Load(object sender, EventArgs e)
        {
            String[] info = Request.FilePath.Split('/');
            if (info.Length<3)
            {
                Response.Redirect("~/");
            }
            id = info[info.Length - 1];
            time = info[info.Length - 2];
            date = info[info.Length - 3];
            List<string> show = MySQLServer.get_show_information(id, date + " " + time + ":00:00");
            if (show.Count == 0)
            {
                Response.Redirect("~/");
            }
            session_id = show[0];
            title = show[1];
            cost = show[2];
            film_cost.Text = "<h3 style=\"header-text\">Film cost: " + cost + "</h3>";
            film.Text = "<h1 style=\"header-text\">" + title + "</h1>";
            session_date.Text = "<h3>Date: " + date + "</h3>";
            session_time.Text = "<h3>Time: " + time + ":00</h3>";
            seats.Text = "";
            loadSeats();

            if (Request.Cookies["login"] != null)
            {
                move.Text = "<input class=\"sub-button\" type=\"submit\" value=\"Book\" style=\"width: 290px\" runat=\"server\" onclick=\"get()\" />";
            } 
            else
            {
                move.Text = "<p class=\"lower-text\">New to Cinema?<a href = \"../../../Account/Register\"> Create an account.</a><p/>";
            }
        }

        private void loadSeats()
        {
            for (int row = 1; row <= 5; ++row)
            {
                string places = string.Empty;
                for (int column = 1; column <= 6; ++column)
                {
                    int place = (row - 1) * 6 + column;
                    if (!MySQLServer.check_seat(session_id, place.ToString()))
                    {
                        places += String.Format("<li class=\"seat\"><input type=\"checkbox\" id=\"{0}\" onchange=\"update_cost(this)\" /><label for=\"{0}\">{0}</label></li>", place);
                    }
                    else
                    {
                        places += String.Format("<li class=\"seat\"><input type=\"checkbox\" disabled id=\"{0}\" /><label for=\"{0}\">Occupied</label></li>", place);
                    }
                }
                places = "<ol class=\"seats\" type=\"A\">"+ places + "</ol>";
                seats.Text += String.Format("<li class=\"row row--{0}\">{1}</li>", row.ToString(), places);
            }
        }

        protected void HiddenField1_ValueChanged(object sender, EventArgs e)
        {
            string[] list = HiddenField1.Value.Split(',');
            List<string> places = new List<string>();
            for(int i =0; i<list.Length; ++i)
            {
                if (list[i] == "true")
                {
                    places.Add((i + 1).ToString());
                }
            }
            MySQLServer.add_order(Request.Cookies["login"].Value, session_id, places);
            Response.Redirect("~/Account/Profile");
        }


    }
}