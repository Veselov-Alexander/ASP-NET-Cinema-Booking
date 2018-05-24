using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinema.Shows
{
    public partial class Default : System.Web.UI.Page
    {
        public class DayShow
        {
            public class Film
            {
                public String film_name;
                public String film_id;
                public String URL;
                public String country;
                public String continuance;
                public List<String> time = new List<string>();
            }
            public String date;
            public List<Film> film_list = new List<Film>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> Shows = MySQLServer.get_shows();
            List<DayShow> Days = new List<DayShow>();
            String first_date = String.Empty;
            String last_show_date = String.Empty;
            Tabs.Text = string.Empty;
            for (int show = 0; show < Shows.Count / 6; show++)
            {
                String film_id = Shows[show * 6 + 0];
                String film_name = Shows[show * 6 + 1];
                String URL = Shows[show * 6 + 2];
                String country = Shows[show * 6 + 3];
                String continuance = Shows[show * 6 + 4];
                String show_date = Shows[show * 6 + 5];
                if (first_date == String.Empty)
                {
                    first_date = show_date.Split(' ')[0];
                }
                if (show_date.Split(' ').First() != last_show_date.Split(' ').First())
                {
                    last_show_date = show_date;

                    Days.Add(new DayShow());

                    Days.Last().film_list.Add(new DayShow.Film());
                    Days.Last().film_list.Last().film_id = film_id;
                    Days.Last().film_list.Last().film_name = film_name;
                    Days.Last().film_list.Last().URL = URL;
                    Days.Last().film_list.Last().country = country;
                    Days.Last().film_list.Last().continuance = continuance;
                    Days.Last().film_list.Last().time.Add(show_date.Split(' ').Last());
                    Days.Last().date = show_date.Split(' ').First();
                }
                else
                {
                    int film_is_found = -1;
                    for (int film = 0; film < Days.Last().film_list.Count; ++film)
                    {
                        if (Days.Last().film_list[film].film_name == film_name)
                        {
                            film_is_found = film;
                            break;
                        }
                    }
                    if (film_is_found != -1)
                    {
                        Days.Last().film_list[film_is_found].time.Add(show_date.Split(' ').Last());
                    }
                    else
                    {
                        Days.Last().film_list.Add(new DayShow.Film());
                        Days.Last().film_list.Last().film_id = film_id;
                        Days.Last().film_list.Last().film_name = film_name;
                        Days.Last().film_list.Last().URL = URL;
                        Days.Last().film_list.Last().country = country;
                        Days.Last().film_list.Last().continuance = continuance;
                        Days.Last().film_list.Last().time.Add(show_date.Split(' ').Last());
                    }
                }
            }

            //**DEBUG**//
            /*
            for (int i = 0; i < Days.Count; ++i)
            {
                Debug.WriteLine("Date: " + Days[i].date);
                Debug.WriteLine("Hall: " + Days[i].hall_name);
                Debug.WriteLine("Films:");
                for (int j = 0; j < Days[i].film_list.Count; ++j)
                {
                    Debug.WriteLine(Days[i].film_list[j].film_name + " " + Days[i].film_list[j].film_id + ":");
                    for (int k = 0; k < Days[i].film_list[j].time.Count; ++k)
                    {
                        Debug.WriteLine(Days[i].film_list[j].time[k]);
                    }
                }
                Debug.WriteLine("");
            }
            */

            for (int i = 0; i < Days.Count; ++i)
            {
                string tabclass = "tablinks";
                if (int.Parse(selectedTab.Value) == i)
                {
                    tabclass = "tablinks active";
                }
                Tabs.Text += String.Format("<button id=\"T"+i+"\" class=\""+ tabclass + "\" onclick=\"openTab(event,this, '{0}')\">{0}</button>", Days[i].date);
                String content = String.Empty;
                for (int j = 0; j < Days[i].film_list.Count; ++j)
                {
                    String sessions = String.Empty;
                    for (int k = 0; k < Days[i].film_list[j].time.Count; ++k)
                    {
                        string[] d = Days[i].date.Split('.').Reverse().ToArray();
                        string date = string.Empty;
                        foreach (string s in d)
                        {
                            date += s + "-";
                        }
                        sessions += String.Format("<a class=\"link\" href=\"{0}/{1}/{2}\">{3}</a>",
                             date.Substring(0, date.Length - 1),
                             Days[i].film_list[j].time[k].Split(':').First(),
                             Days[i].film_list[j].film_id,
                             Days[i].film_list[j].time[k]);
                    }
                    string genres = MySQLServer.get_films_genres(Days[i].film_list[j].film_id);
                    content += String.Format("<ul class=\"list\"><li><img src=\"../Posters/{0}.jpg\" alt=\"\" /><a class=\"link\"href=\"../Films/{0}\">{1}</a><br />Country: {2}<br/>Genres: {3}<br/>Durability: {4} <br/>Sessions: {5} <br/></li></ul>",
                         Days[i].film_list[j].URL,
                         Days[i].film_list[j].film_name,
                         Days[i].film_list[j].country,
                         genres,
                         Days[i].film_list[j].continuance,
                         sessions);

                }
                generateInfo.Text += String.Format("<div id = \"{0}\" class=\"tabcontent\"><div class=\"content\">{1}</div></div>", Days[i].date, content);
            }
            
            Page.ClientScript.RegisterStartupScript(this.GetType(), "selectFirst", "selectFirst('"+first_date+"')", true);
        }
    }
}