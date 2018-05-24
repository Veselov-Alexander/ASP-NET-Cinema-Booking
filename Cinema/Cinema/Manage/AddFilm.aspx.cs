using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinema.Manage
{
    public partial class AddFilm : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        { 
            if (!(MySQLServer.auth_chech(Request) && MySQLServer.admin_check(Request)))
            {
                Response.Redirect("~/", false);
            }


            if (category.Items.Count == 0)
            {
                loadMeta();
            }
        }

        private void loadMeta()
        {

            List<string> items = MySQLServer.get_categories();
            foreach (string item in items)
            {
                category.Items.Add(new ListItem(item.ToString()));
            }
            items = MySQLServer.get_coutries();
            foreach (string item in items)
            {
                country.Items.Add(item);
            }
        }

        protected async void submit_Click(object sender, EventArgs e)
        {
            MySQLServer.InputStatus[] status = await MySQLServer.add_film(title.Text,
                                            description.Text,
                                            durability.Text,
                                            poster,
                                            URL.Text,
                                            year.Text,
                                            producer.Value,
                                            country.SelectedValue,
                                            category,
                                            Response,
                                            Server);
            if (!(status[0] == MySQLServer.InputStatus.Succeed &&
                status[0] == status[1] && status[0] == status[2] &&
                status[2] == status[3] && status[3] == status[4]))
            {
                title_error.Text = "Enter film title.";
                description_error.Text = "Fill out a short description of the film.";
                image_error.Text = "Select an image for the poster.";
                durability_error.Text = "Enter the length of the movie (in minutes).";
                category_error.Text = "Select at least one category.";
                producer_error.Text = "Producer's name.";
                year_error.Text = "Enter year of film.";
                URL_error.Text = "Enter the URL (must be unique).";
                poster_path.Value = "";
     

                category_error.CssClass = "lower-text";
                producer_error.CssClass = "lower-text";
                durability_error.CssClass = "lower-text";
                title_error.CssClass = "lower-text";
                description_error.CssClass = "lower-text";
                image_error.CssClass = "lower-text";
                year_error.CssClass = "lower-text";
                URL_error.CssClass = "lower-text";

                if (status[0] == MySQLServer.InputStatus.EmptyString)
                {
                    title_error.CssClass = "error-string";
                    title_error.Text = "Title required";
                }
                if (status[1] == MySQLServer.InputStatus.EmptyString)
                {
                    description_error.CssClass = "error-string";
                    description_error.Text = "Description required";
                }
                if (status[2] == MySQLServer.InputStatus.EmptyString)
                {
                    durability_error.CssClass = "error-string";
                    durability_error.Text = "Durability required";
                }
                else
                if (status[2] == MySQLServer.InputStatus.IncorrectNumber)
                {
                    durability_error.CssClass = "error-string";
                    durability_error.Text = "Must be a positive number not greater than 500";
                }

                if (status[3] == MySQLServer.InputStatus.WrongPosterExp)
                {
                    image_error.CssClass = "error-string";
                    image_error.Text = "File must be in '*.jpg' format.";
                }

                if (status[4] == MySQLServer.InputStatus.NoCategoriesSelected)
                {
                    category_error.CssClass = "error-string";
                    category_error.Text = "No categories selected";
                }

                if (status[5] == MySQLServer.InputStatus.EmptyString)
                {
                    producer_error.CssClass = "error-string";
                    producer_error.Text = "Producer required.";
                }

                if (status[6] == MySQLServer.InputStatus.IncorrectNumber)
                {
                    year_error.CssClass = "error-string";
                    year_error.Text = "Wrong year.";
                }

                if (status[7] != MySQLServer.InputStatus.Succeed)
                {
                    URL_error.CssClass = "error-string";
                    URL_error.Text = "Wrong URL or already exist.";
                }

            }
            else
            {
                Response.Redirect("~/Films/" + URL.Text, false);
            }
        }
    }
}