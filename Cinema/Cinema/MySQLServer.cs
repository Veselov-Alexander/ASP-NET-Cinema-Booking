using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;


namespace Cinema
{
    public static class MySQLServer
    {

        public static string connectionString = "server=localhost;user id=root;password=1234567;database=cinema";

        public static MySqlConnection connection = new MySqlConnection(MySQLServer.connectionString);

        public enum InputStatus
        {
            Succeed,
            IncorrectPassword,
            NoSuchUser,
            LoginRequired,
            PasswordRequired,
            EmailRequired,
            ShortPassword,
            LoginAlreadyTaken,
            EmailAlreadyTaken,
            EmptyString,
            IncorrectNumber,
            NoCategoriesSelected,
            WrongPosterSize,
            WrongPosterExp
        }

        public static bool auth_chech(HttpRequest Request)
        {
            HttpCookie loginCookie = Request.Cookies["login"];
            HttpCookie signCookie = Request.Cookies["sign"];

            if (loginCookie != null && signCookie != null)
            {
                if (signCookie.Value == SignGenerator.GetSign(loginCookie.Value))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool admin_check(HttpRequest Request)
        {
            HttpCookie roleCookie = Request.Cookies["role"];

            if (roleCookie != null)
            {
                if (roleCookie.Value == "admin")
                {
                    return true;
                }
            }
            return false;
        }



        public static async Task<InputStatus> login(string login,
                                                   string password,
                                                   HttpResponse Response,
                                                   bool is_admin)
        {
            if (login == string.Empty)
            {
                return InputStatus.LoginRequired;
            }
            if (password == string.Empty)
            {
                return InputStatus.PasswordRequired;
            }
            MySqlCommand cmd = new MySqlCommand("SELECT login, password, role FROM User WHERE login like('"+ login + "')", connection);

            string queryLogin = "", queryPassword= "", queryRole="";
            DbDataReader reader = null;
            try
            {
                reader = await cmd.ExecuteReaderAsync();
                await reader.ReadAsync();
                if (!reader.HasRows)
                {
                    return InputStatus.NoSuchUser;
                }
                queryLogin = reader["login"].ToString();
                queryPassword = reader["password"].ToString();
                queryRole = reader["role"].ToString();
            }
            catch {} finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            
            if (password == queryPassword)
            {
                HttpCookie loginCookie = new HttpCookie("login", login);
                HttpCookie signCookie = new HttpCookie("sign", SignGenerator.GetSign(login));
                HttpCookie roleCookie = new HttpCookie("role", queryRole);

                Response.Cookies.Add(loginCookie);
                Response.Cookies.Add(signCookie);
                Response.Cookies.Add(roleCookie);
                if (is_admin)
                {
                    string roles = "Admin,User";
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                      1,
                      login,  //user id
                      DateTime.Now,
                      DateTime.MaxValue,
                      true,
                      roles,
                      "/");
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                                       FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(login, true);
                }
                

                Response.Redirect("../Default.aspx", false);
                return InputStatus.Succeed;
            }
            
            return InputStatus.IncorrectPassword;
        }

        public static async Task<InputStatus[]> register(string login,
                                                      string password,
                                                      string email,
                                                      HttpResponse Response)
        {
            InputStatus[] errors = new InputStatus[3];
            errors[0] = InputStatus.Succeed;
            errors[1] = InputStatus.Succeed;
            errors[2] = InputStatus.Succeed;

            if (login == string.Empty)
            {
                errors[0] = InputStatus.LoginRequired;
            }
            if (email == string.Empty)
            {
                errors[1] = InputStatus.EmailRequired;
            }
            if (password == string.Empty)
            {
                errors[2] = InputStatus.PasswordRequired;
            }
            if (password.Length < 7)
            {
                if (errors[2] == InputStatus.Succeed)
                {
                    errors[2] = InputStatus.ShortPassword;
                }
            }
            if (find_tag(login.ToUpper(), "login").Result)
            {
                if (errors[0] == InputStatus.Succeed)
                {
                    errors[0] = InputStatus.LoginAlreadyTaken;
                }
            }
            if (find_tag(email, "e-mail").Result)
            {
                if (errors[1] == InputStatus.Succeed)
                {
                    errors[1] =  InputStatus.EmailAlreadyTaken;
                }
            }
            if (errors[0] != InputStatus.Succeed || 
                errors[1] != InputStatus.Succeed || 
                errors[2] != InputStatus.Succeed)
            {
                return errors;
            }
            MySqlCommand myCommand = new MySqlCommand(@"INSERT INTO user (`login`, `password`, `role`, `e-mail`)
                                                      VALUES(@login, @password, @role, @email)", connection);
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@login",
                MySqlDbType = MySqlDbType.VarChar,
                Value = login
            });
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@password",
                MySqlDbType = MySqlDbType.VarChar,
                Value = password
            });
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@role",
                MySqlDbType = MySqlDbType.Enum,
                Value = "user"
            });
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@email",
                MySqlDbType = MySqlDbType.VarChar,
                Value = email
            });
            await myCommand.ExecuteNonQueryAsync();
            return errors;
        }

        private static async Task<bool> find_tag(string key, string tag)
        {
            MySqlCommand cmd = new MySqlCommand("select * from user where `"+tag+"` like('"+key+"')", connection);
            DbDataReader reader = null;
            bool result = false;
            try
            {
                reader = await cmd.ExecuteReaderAsync();
            }
            catch { } finally
            {
                
                if (reader != null)
                {
                    result = reader.HasRows;
                    reader.Close();
                }
            }
            return result;

        }

        public static List<string> get_categories()
        {
            MySqlCommand cmd = new MySqlCommand("select title from category", connection);
            DbDataReader reader = null;
            List<string> list = new List<string>();
            try
            {
                reader =  cmd.ExecuteReader();
                while ( reader.Read())
                {
                    list.Add(reader["title"].ToString());
                }
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public static List<string> get_coutries()
        {
            MySqlCommand cmd = new MySqlCommand("select country_name from country", connection);
            DbDataReader reader = null;
            List<string> list = new List<string>();
            try
            {
                reader =  cmd.ExecuteReader();
                while ( reader.Read())
                {
                    list.Add(reader["country_name"].ToString());
                }
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public static List<string> get_films()
        {
            MySqlCommand cmd = new MySqlCommand("select film_id, title from Film", connection);
            DbDataReader reader = null;
            List<string> list = new List<string>();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader["film_id"].ToString()+"&  "+reader["title"].ToString());
                }
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public static List<string> get_shows()
        {
            MySqlCommand cmd = new MySqlCommand("select film_id, title, URL, country_name, durability, date from film, session, country where film.film_id = session.Film_film_id and country_id = film.Country_country_id and date > now() order by date", connection);
            DbDataReader reader = null;
            List<string> list = new List<string>();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader["film_id"].ToString());
                    list.Add(reader["title"].ToString());
                    list.Add(reader["URL"].ToString());
                    list.Add(reader["country_name"].ToString());
                    list.Add(reader["durability"].ToString());
                    list.Add(reader["date"].ToString());
                }
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public static List<string> get_times(string date)
        {
            MySqlCommand cmd = new MySqlCommand("select time(date) 'begin' from session, film where date like('"+date+"%') and film_id = Film_film_id", connection);
            DbDataReader reader = null;
            List<string> list = new List<string>();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader["begin"].ToString());
                }
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public static async Task<int> get_id_by_country(string country)
        {
           
            MySqlCommand cmd = new MySqlCommand("select country_id from country where country_name like(\""+country+"\")", connection);
            int result = -1;
            DbDataReader reader = null;
            try
            {
                reader = await cmd.ExecuteReaderAsync();
                await reader.ReadAsync();
            }
            catch { }
            finally
            {

                if (reader != null)
                {
                   
                    result = int.Parse(reader["country_id"].ToString());
                    reader.Close();
                }
            }
            return result;
        }

        public static async Task<int> get_id_by_category(string title)
        {
            MySqlCommand cmd = new MySqlCommand("select category_id from category where title like(\"" + title + "\")", connection);
            int result = -1;
            DbDataReader reader = null;
            try
            {
                reader = await cmd.ExecuteReaderAsync();
                await reader.ReadAsync();
            }
            catch { }
            finally
            {

                if (reader != null)
                {
                    result = int.Parse(reader["category_id"].ToString());
                    reader.Close();
                }
            }
            return result;
        }

        public static async Task<int> get_id_by_URL(string URL)
        {

            MySqlCommand cmd = new MySqlCommand("select film_id from film where URL like(\"" + URL + "\")", connection);
            int result = -1;
            DbDataReader reader = null;
            try
            {
                reader = await cmd.ExecuteReaderAsync();
                await reader.ReadAsync();
            }
            catch { }
            finally
            {

                if (reader != null)
                {
                    result = int.Parse(reader["film_id"].ToString());
                    reader.Close();
                }
            }
            return result;
        }

        public static int get_id_by_Login(string login)
        {
            MySqlCommand cmd = new MySqlCommand("select user.user_id from user where user.login = '"+login+"'", connection);
            int result = -1;
            DbDataReader reader = null;
            try
            {
                reader =  cmd.ExecuteReader();
                reader.Read();
            }
            catch { }
            finally
            {

                if (reader != null)
                {
                    result = int.Parse(reader["user_id"].ToString());
                    reader.Close();
                }
            }
            return result;
        }

        public static async Task<bool> URL_check(string URL)
        {
            MySqlCommand cmd = new MySqlCommand("select film_id from film where URL like(\"" + URL + "\")", connection);
            bool result = false;
            DbDataReader reader = null;
            try
            {
                reader = await cmd.ExecuteReaderAsync();
                await reader.ReadAsync();
            }
            catch { }
            finally
            {

                if (reader != null)
                {
                    result = reader.HasRows;
                    reader.Close();
                }
            }
            return result;
        }


        public static async Task<InputStatus[]> add_film(string title,
                                                       string description,
                                                       string durability,
                                                       FileUpload poster,
                                                       string URL,
                                                       string year,
                                                       string producer,
                                                       string country,
                                                       CheckBoxList categories,
                                                       HttpResponse Response,
                                                       HttpServerUtility Server)
        {
            InputStatus[] errors = new InputStatus[8];
            errors[0] = InputStatus.Succeed;
            errors[1] = InputStatus.Succeed;
            errors[2] = InputStatus.Succeed;
            errors[3] = InputStatus.Succeed;
            errors[4] = InputStatus.Succeed;
            errors[5] = InputStatus.Succeed;
            errors[6] = InputStatus.Succeed;
            errors[7] = InputStatus.Succeed;

            if (title == string.Empty)
            {
                errors[0] = InputStatus.EmptyString;
            }
            if (description == string.Empty)
            {
                errors[1] = InputStatus.EmptyString;
            }
            if (durability == string.Empty)
            {
                errors[2] = InputStatus.EmptyString;
            }
            if (producer == string.Empty)
            {
                errors[5] = InputStatus.EmptyString;
            }
            if (URL == string.Empty)
            {
                errors[7] = InputStatus.EmptyString;
            }

            int durabilityNumber = 0;
            if (!int.TryParse(durability, out durabilityNumber))
            {
                errors[2] = InputStatus.IncorrectNumber;
            }

            int yearNumber = 0;
            if (!int.TryParse(year, out yearNumber))
            {
                errors[6] = InputStatus.IncorrectNumber;
            }
            if (yearNumber > 3000 || yearNumber < 1000)
            {
                errors[6] = InputStatus.IncorrectNumber;
            }

            if (await URL_check(URL))
            {
                errors[7] = InputStatus.IncorrectNumber;
            }

            if (durabilityNumber < 0 || durabilityNumber > 500)
            {
                errors[2] = InputStatus.IncorrectNumber;
            }

            List<ListItem> selected = new List<ListItem>();
            foreach(ListItem item in categories.Items)
            {
                if (item.Selected)
                {
                    selected.Add(item);
                }
            }

            if (selected.Count == 0)
            {
                errors[4] = InputStatus.NoCategoriesSelected;
            }

            if (poster.FileName.Split('.').Last() != "jpg" )
            {
                errors[3] = InputStatus.WrongPosterExp;
            }



            if (errors[0] != InputStatus.Succeed ||
                errors[1] != InputStatus.Succeed ||
                errors[2] != InputStatus.Succeed ||
                errors[3] != InputStatus.Succeed ||
                errors[4] != InputStatus.Succeed ||
                errors[5] != InputStatus.Succeed ||
                errors[6] != InputStatus.Succeed ||
                errors[7] != InputStatus.Succeed) 
            {
                return errors;
            }

            poster.SaveAs(Server.MapPath("~/Posters/" + URL + ".jpg"));


            MySqlCommand myCommand = new MySqlCommand(@"INSERT INTO film (`title`, `description`, `durability`, `Country_country_id`, `producer`, `URL`, `year`)
                                                      VALUES(@title, @description, @durability, @country_id, @producer, @URL, @year)", connection);
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@title",
                MySqlDbType = MySqlDbType.VarChar,
                Value = title
            });
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@description",
                MySqlDbType = MySqlDbType.VarChar,
                Value = description
            });
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@durability",
                MySqlDbType = MySqlDbType.Int32,
                Value = durabilityNumber
            });
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@country_id",
                MySqlDbType = MySqlDbType.Int32,
                Value = get_id_by_country(country).Result
            });
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@producer",
                MySqlDbType = MySqlDbType.VarChar,
                Value = producer
            });
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@URL",
                MySqlDbType = MySqlDbType.VarChar,
                Value = URL
            });
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@year",
                MySqlDbType = MySqlDbType.Int32,
                Value = yearNumber
            });
            await myCommand.ExecuteNonQueryAsync();


            foreach(ListItem item in selected)
            {
                MySqlCommand cmd = new MySqlCommand(@"INSERT INTO film_has_a_category (`Film_film_id`, `Category_category_id`)
                                                      VALUES(@Film_film_id, @Category_category_id)", connection);

                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Film_film_id",
                    MySqlDbType = MySqlDbType.Int32,
                    Value = get_id_by_URL(URL).Result
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Category_category_id",
                    MySqlDbType = MySqlDbType.Int32,
                    Value = get_id_by_category(item.Text).Result
                });
                await cmd.ExecuteNonQueryAsync();
            }

            return errors;
        }

        public static async Task add_session(string film_id,
                                                        string date,
                                                        string cost)
        {
            MySqlCommand myCommand = new MySqlCommand(@"INSERT INTO session (`Film_film_id`, `date`, `cost`)
                                                      VALUES(@Film_film_id, @date, @cost)", connection);
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Film_film_id",
                MySqlDbType = MySqlDbType.Int32,
                Value = film_id
            });
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@date",
                MySqlDbType = MySqlDbType.DateTime,
                Value = date
            });
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@cost",
                MySqlDbType = MySqlDbType.Int32,
                Value = cost
            });
           
            await myCommand.ExecuteNonQueryAsync();
        }

        public static async Task<List<String>> get_film_information(String URL)
        {
            int ID = await get_id_by_URL(URL);
            MySqlCommand cmd = new MySqlCommand("select * from film_information where `Film id` like(" + ID.ToString() +")", connection);
            DbDataReader reader = null;
            List<string> list = new List<string>();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    int fieldCount = reader.GetValues(values);
                    for (int i = 0; i < fieldCount; i++)
                    {
                        list.Add(values[i].ToString());
                    }
                }
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public static List<String> get_show_information(String id, String date)
        {
            MySqlCommand cmd = new MySqlCommand("select session_id, film.title, session.cost from session, film where session.Film_film_id = film.film_id and Film_film_id = " + id +" and date = \"" + date + "\"and date > curdate()", connection);
            Debug.WriteLine(cmd.CommandText);
            DbDataReader reader = null;
            List<string> list = new List<string>();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    int fieldCount = reader.GetValues(values);
                    for (int i = 0; i < fieldCount; i++)
                    {
                        list.Add(values[i].ToString());
                    }
                }
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public static List<String> get_orders_information()
        {
            MySqlCommand cmd = new MySqlCommand("select * from order_information order by Date", connection);
            Debug.WriteLine(cmd.CommandText);
            DbDataReader reader = null;
            List<string> list = new List<string>();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    int fieldCount = reader.GetValues(values);
                    for (int i = 0; i < fieldCount; i++)
                    {
                        list.Add(values[i].ToString());
                    }
                }
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public static List<List<string>> custom_query(String command)
        {
            MySqlCommand cmd = new MySqlCommand(command, connection);
            Debug.WriteLine(cmd.CommandText);
            DbDataReader reader = null;
            List<List<string>> list = new List<List<string>>();
            try
            {
                reader = cmd.ExecuteReader();
                
                List<string> attributes = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    attributes.Add(reader.GetName(i));
                }
                list.Add(attributes);
                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    List<string> row = new List<string>();

                    int fieldCount = reader.GetValues(values);
                    for (int i = 0; i < fieldCount; i++)
                    {
                        row.Add(values[i].ToString());
                    }
                    list.Add(row);
                }
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public static List<String> get_orders_information(string name)
        {
            MySqlCommand cmd = new MySqlCommand("select * from order_information where user = \"" + name + "\" order by Date", connection);
            Debug.WriteLine(cmd.CommandText);
            DbDataReader reader = null;
            List<string> list = new List<string>();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    int fieldCount = reader.GetValues(values);
                    for (int i = 0; i < fieldCount; i++)
                    {
                        list.Add(values[i].ToString());
                    }
                }
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public static List<String> get_orders_information(int id)
        {
            MySqlCommand cmd = new MySqlCommand("select * from order_information where ID = \"" + id + "\" order by Date", connection);
            Debug.WriteLine(cmd.CommandText);
            DbDataReader reader = null;
            List<string> list = new List<string>();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    int fieldCount = reader.GetValues(values);
                    for (int i = 0; i < fieldCount; i++)
                    {
                        list.Add(values[i].ToString());
                    }
                }
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public static void confirm(string id)
        {
            MySqlCommand cmd = new MySqlCommand("update `order` set status = \"confirmed\" where order_id = " + id, connection);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand("call update_orders("+id+")", connection);
            cmd.ExecuteNonQuery();
        }

        public static void decline(string id)
        {
            MySqlCommand cmd = new MySqlCommand("update `order` set status = \"rejected\" where order_id = " + id, connection);
            cmd.ExecuteNonQuery();
        }

        public static String get_films_genres(String id)
        {
            MySqlCommand cmd = new MySqlCommand("select Genres from film_information where `film id` = " + id, connection);
            Debug.WriteLine(cmd.CommandText);
            DbDataReader reader = null;
            string list = string.Empty;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
                list = reader["Genres"].ToString();
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public static bool check_seat(String id, String place)
        {
            MySqlCommand cmd = new MySqlCommand("select place.place_id from order_has_a_place, place, session, `order` where place.place_id = order_has_a_place.Place_place_id and order_has_a_place.Order_order_id = `order`.order_id and `order`.status = \"confirmed\" and `order`.Session_session_id = session.session_id and session.session_id ="+ id+" and place.place_id = " + place, connection);
            bool result = false;
            DbDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
            }
            catch { }
            finally
            {

                if (reader != null)
                {
                    result = reader.HasRows;
                    reader.Close();
                }
            }
            return result;
        }

        public static void add_order(string login, string session_id, List<string> places)
        {
            MySqlCommand myCommand = new MySqlCommand(@"INSERT INTO `order` (`User_user_id`, `status`, `Session_session_id`)
                                                      VALUES(@User_user_id, @status, @Session_session_id)", connection);
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@User_user_id",
                MySqlDbType = MySqlDbType.Int32,
                Value = get_id_by_Login(login)
            });
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@status",
                MySqlDbType = MySqlDbType.Enum,
                Value = "pending confirmation"
            });
            myCommand.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Session_session_id",
                MySqlDbType = MySqlDbType.Int32,
                Value = session_id
            });

            myCommand.ExecuteNonQuery();

            MySqlCommand cmd = new MySqlCommand("select max(`order_id`) 'id' from `order`", connection);
            DbDataReader reader = null;
            string order_id = string.Empty;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
                order_id = reader["id"].ToString();
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            foreach (string place in places)
            {
                myCommand = new MySqlCommand(@"INSERT INTO order_has_a_place (`Order_order_id`, `Place_place_id`)
                                                      VALUES(@order_id, @place_id)", connection);
                myCommand.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@order_id",
                    MySqlDbType = MySqlDbType.Int32,
                    Value = order_id
                });
                myCommand.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@place_id",
                    MySqlDbType = MySqlDbType.Int32,
                    Value = place
                });
                myCommand.ExecuteNonQuery();
            }


        }

    }
}