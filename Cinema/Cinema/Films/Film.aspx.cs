using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinema.Films
{
    public partial class Film : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            String URL = Request.FilePath.Split('/').Last();
            if (await MySQLServer.URL_check(URL))
            {
                List<String> list = await MySQLServer.get_film_information(URL);
                title.Text = list[1];
                description.Text = list[2];
                durability.Text = list[3];
                producer.Text = list[4];
                year.Text = list[5];
                country.Text = list[6];
                genre.Text = list[7];
                poster.ImageUrl = "~/Posters/" + URL + ".jpg";
            }
            else
            {
                Response.Redirect("~/", false);
            }
        }
    }
}