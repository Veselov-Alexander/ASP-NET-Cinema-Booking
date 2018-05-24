using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinema.Account
{
    public partial class Register : System.Web.UI.Page
    {
        private MySqlConnection connection = null;

        protected async void Page_Load(object sender, EventArgs e)
        {
            connection = new MySqlConnection(MySQLServer.connectionString);
            await connection.OpenAsync();

            if (MySQLServer.auth_chech(Request))
            {
                Response.Redirect("~/Account/Profile", false);
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (connection != null && connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        protected async void submit_Click(object sender, EventArgs e)
        {

            MySQLServer.InputStatus[] status = await MySQLServer.register(loginBox.Value,
                                            passwordBox.Value,
                                            emalBox.Value,
                                            Response);
            if (status[0] == MySQLServer.InputStatus.Succeed && status[0] == status[1] && status[0] == status[2])
            {
                await MySQLServer.login(loginBox.Value,
                                            passwordBox.Value,
                                            Response,
                                            false);
            }
            else
            {
                loginError.Text = "This will be your username.";
                emailError.Text = "We'll occasionally send updates about your account to this inbox.";
                passwordError.Text = "Use at least one lowercase letter, one numeral, and seven characters.";
                loginError.CssClass = "lower-text";
                emailError.CssClass = "lower-text";
                passwordError.CssClass = "lower-text";
                if (status[0] == MySQLServer.InputStatus.LoginRequired)
                {
                    loginError.CssClass = "error-string";
                    loginError.Text = "Login required";
                }
                if (status[1] == MySQLServer.InputStatus.EmailRequired)
                {
                    emailError.CssClass = "error-string";
                    emailError.Text = "E-mail required";
                }
                if (status[2] == MySQLServer.InputStatus.PasswordRequired)
                {
                    passwordError.CssClass = "error-string";
                    passwordError.Text = "Password required";
                }
                if (status[0] == MySQLServer.InputStatus.LoginAlreadyTaken)
                {
                    loginError.CssClass = "error-string";
                    loginError.Text = "This login already taken";
                }
                if (status[1] == MySQLServer.InputStatus.EmailAlreadyTaken)
                {
                    emailError.CssClass = "error-string";
                    emailError.Text = "This e-mail already taken";
                }
                if (status[2] == MySQLServer.InputStatus.ShortPassword)
                {
                    passwordError.CssClass = "error-string";
                    passwordError.Text = "Password must be longer than 6 characters";
                }
            }
        }
    }
}