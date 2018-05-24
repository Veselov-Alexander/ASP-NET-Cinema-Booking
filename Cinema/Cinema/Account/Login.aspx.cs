using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinema.Account
{
    public partial class Login : System.Web.UI.Page
    {
        private MySqlConnection connection = null;
        protected async void Page_Load(object sender, EventArgs e)
        {
            connection = new MySqlConnection(MySQLServer.connectionString);
            await connection.OpenAsync();


            if (MySQLServer.auth_chech(Request))
            {
                Response.Redirect("~/Account/Profile.aspx", false);
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (connection != null && connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        [Authorize(Roles = "Admin")]
        protected async void submit_Click(object sender, EventArgs e)
        {
            if (loginBox.Value.ToUpper() == "ADMIN")
            {
                await auth(true);
            }
            else
            {
                await auth(false);
            }
        }


        async Task auth(bool is_admin)
        {
            MySQLServer.InputStatus status = await MySQLServer.login(loginBox.Value,
                                             passwordBox.Value,
                                             Response,
                                             is_admin);
            loginError.Text = "<br />";
            passwordError.Text = "<br />";
            if (status == MySQLServer.InputStatus.LoginRequired)
            {
                loginError.Text = "Login required";
            }
            if (status == MySQLServer.InputStatus.PasswordRequired)
            {
                passwordError.Text = "Password required";
            }
            if (status == MySQLServer.InputStatus.NoSuchUser)
            {
                loginError.Text = "No such user";
            }
            if (status == MySQLServer.InputStatus.IncorrectPassword)
            {
                passwordError.Text = "Incorrect password";
            }
        }

    }
}