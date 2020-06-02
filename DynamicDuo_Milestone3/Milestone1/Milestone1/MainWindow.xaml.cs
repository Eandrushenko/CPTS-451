using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using System.Device.Location; //used for coordinate distance calculation

//For Sam's beautiful eyes only

//select distinct on(R.user_id) R.user_id, R.business_id, R.text from review as R
//   INNER JOIN
//   (
//   select distinct(user_id), Min(date) as date from review where user_id IN(select friend_id from friend where user_id = 'om5ZiponkpRqUNa3pVPiRg') group by user_id
//	)tb2 ON R.user_id = tb2.user_id AND R.date = tb2.date
//select* from friend where user_id = 'om5ZiponkpRqUNa3pVPiRg'

//FOR SAMS FINISHED BEAUTIFUL EYES ONLY

//select distinct on(R.user_id) R.user_id, R.business_id, R.text, B.name, U.name from review as R
//   INNER JOIN
//   (
//   select distinct(user_id), Min(date) as date from review where user_id IN(select friend_id from friend where user_id = 'om5ZiponkpRqUNa3pVPiRg') group by user_id
//   )tb2 ON R.user_id = tb2.user_id AND R.date = tb2.date
//   INNER JOIN business as B on B.business_id = R.business_id
//   INNER JOIN users as U on U.user_id = R.user_id

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string G_UID = "BfcNxKpnF9z5wJLXY7elRg"; //Global variable for user ID
        public class Business
        {
            public string name { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public double distance { get; set; }
            public int count { get; set; }
            public double rating { get; set; }
            public int checkins { get; set; }
            public string zipcode { get; set; }
            public string bid { get; set; }

        }

        public class Review
        {
            public string username { get; set; }
            public string date { get; set; }
            public string text { get; set; }
        }

        public class Friend
        {
            public string friendname { get; set; }
            public string friendstars { get; set; }
            public string frienddate { get; set; }
        }

        public class Favorite
        {
            public string favename { get; set; }
            public double favestars { get; set; }
            public string favecity { get; set; }
            public string favezip { get; set; }
            public string faveaddress { get; set; }
            public string favebid { get; set; }
        }

        public class Latest
        {
            public string latename { get; set; }
            public string latebusiness { get; set; }
            public string latecity { get; set; }
            public string latetext { get; set; }
        }


        public MainWindow()
        {
            InitializeComponent();
            addStates();
            addColumns2Grid();
            addColumns2Review();
            addColumns2Friend();
            addColumns2Favorite();
            addColumns2Latest();
        }

        private string buildConnString()
        {
            return "Host=localhost; Username=postgres; Password=zxcvbnm; Database = milestone1db";
        }

        public void addStates()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY state;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            statelist.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }

        public void addCities()
        {
            citylist.Items.Clear();
            if (statelist.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT distinct city FROM business WHERE state = '"+ statelist.SelectedItem.ToString() +"' ORDER BY city;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                citylist.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }

        public void addZipCodes()
        {
            ziplist.Items.Clear();
            if (citylist.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT distinct zipcode FROM business WHERE city = '" + citylist.SelectedItem.ToString() + "' ORDER BY zipcode;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ziplist.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }

        public void addCats()
        {
            catlist.Items.Clear();
            if (ziplist.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select distinct category from business, categories where business.business_id = categories.business_id and zipcode = '" + ziplist.SelectedItem.ToString() + "' ORDER BY category;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                catlist.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }

        public void addLatest()
        {
            LatestGrid.Items.Clear();
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    DateTime DT;
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        string UID = G_UID;
                        cmd.Connection = conn;
                        cmd.CommandText = "select distinct on(R.user_id) b.city, R.text, B.name, U.name from review as R INNER JOIN (select distinct(user_id), Max(date) as date from review where user_id IN(select friend_id from friend where user_id = '" + UID + "') group by user_id)tb2 ON R.user_id = tb2.user_id AND R.date = tb2.date INNER JOIN business as B on B.business_id = R.business_id INNER JOIN users as U on U.user_id = R.user_id;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                LatestGrid.Items.Add(new Latest() { latecity = reader.GetString(0), latetext = reader.GetString(1), latebusiness = reader.GetString(2), latename = reader.GetString(3) });
                            }
                        }
                    }
                    conn.Close();
            }
        }


        public void addReviews()
        {
            reviewlist.Items.Clear();
            if (businessGrid.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    DateTime DT;
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        string UID = G_UID;
                        cmd.Connection = conn;
                        // cmd.CommandText = "select review_stars from business, review where(business.business_id = review.business_id) and business.business_id = '" + ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString() + "';";
                        cmd.CommandText = "select name, date, text from Review natural join Users where business_id = '" + ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString() + "' AND user_id IN (select friend_id from friend as f where f.user_id = '" + UID + "')";
                        //var h1 = ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            { 
                                DT = reader.GetDateTime(1).Date;
                                string datestring = DT.Year.ToString() + "-" + DT.Month.ToString() + "-" + DT.Day.ToString();
                                //businessGrid.Items.Add(new Business() { name = reader.GetString(0), city = reader.GetString(1), state = reader.GetString(2), zipcode = reader.GetString(3), bid = reader.GetString(4) });
                                reviewlist.Items.Add(new Review() { username = reader.GetString(0), date = datestring, text = reader.GetString(2) });
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }

        public void addDOW()
        {
            string open;
            string close;
            DateTime day = DateTime.Now;
            string businessDOW = day.DayOfWeek.ToString();
            if (businessGrid.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select open, close from Hours where business_id = '" + ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString() + "' and day = '" + businessDOW + "';";
                        //var h1 = ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString();
                        using (var reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            try
                            {
                                open = reader.GetString(0).ToString();
                                close = reader.GetString(1).ToString();
                            }
                            catch(InvalidOperationException)
                            {
                                open = close = "N/A";
                            }
                            dow.Text = "Today (" + businessDOW + ")\n  Opens: " + open + "\n  Closes: " + close;

                        }
                    }
                    conn.Close();
                }
            }
        }

        public void addCats2Display()
        {
            CatDisplay.Clear();
            string category = "";
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select distinct category from categories where business_id = '" + ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString() + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            category += reader.GetString(0) + "\n";
                        }
                        CatDisplay.Text = category;
                    }
                }
                conn.Close();
            }
            
        }

        public void addUID(string Username)
        {
            UseridBox.Items.Clear();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT user_id FROM users where name like '" + Username + "%'";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UseridBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }

        public void AddUserInfo()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select name, average_stars, fans, yelping_since, funny, cool, useful, lat, long from users where user_id='" + G_UID + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime DT = reader.GetDateTime(3).Date;
                            string datestring = DT.Year.ToString() + "-" + DT.Month.ToString() + "-" + DT.Day.ToString();

                            UIDnameBox.Text = reader.GetString(0);
                            UIDstarBox.Text = reader.GetDouble(1).ToString();
                            UIDfansBox.Text = reader.GetInt32(2).ToString();
                            UIDdateBox.Text = datestring;
                            UIDfunnyBox.Text = reader.GetInt32(4).ToString();
                            UIDcoolBox.Text = reader.GetInt32(5).ToString();
                            UIDusefulBox.Text = reader.GetInt32(6).ToString();
                            try
                            {
                                UIDlatBox.Text = reader.GetDouble(7).ToString();
                                UIDlongBox.Text = reader.GetDouble(8).ToString();
                            }
                            catch(InvalidCastException)
                            {
                                UIDlatBox.Text = "";
                                UIDlongBox.Text = "";
                            }
                     

                        }
                    }
                }
                conn.Close();
            }
        }


        public string generatestrings()
        {
            string usables = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = "";
            int j = 0;
            Random random = new Random();
            for (int i = 0; i < 8; i++)
            {
                j = random.Next(0, usables.Length - 1);
                result += usables[j].ToString();
            }
            return result;
        }

        public void submitReviews(string comment, string stars)
        {
            string RID = generatestrings();
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        string UID = G_UID;
                        DateTime DT = DateTime.Today;
                        string datestring = DT.Month.ToString() + "/" + DT.Day.ToString() + "/" + DT.Year.ToString();
                        var h1 = ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString();
                        cmd.Connection = conn;
                        cmd.CommandText = "insert into review(business_id, user_id, review_id, review_stars, text, date) values ('" + h1 +"', '" + UID + "', '" + RID + "', '" + stars + "', '" + comment + "', '" + datestring + "');";
                        using (var reader = cmd.ExecuteReader())
                        {
                            //while (reader.Read())
                            //{
                            //    reviewlist.Items.Add(reader.GetString(0));
                            //}
                        }
                    }
                    conn.Close();
                }
            }

        public void submitCheckins()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    DateTime day = DateTime.Now;
                    string DOW = day.DayOfWeek.ToString();

                    cmd.Connection = conn;
                    cmd.CommandText = "insert into checkins (business_id, day, total) values ('" + ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString() + "', '" + DOW + "', 1);";
                    try
                    {
                        using (var reader = cmd.ExecuteReader())
                        {

                        }
                    }
                    catch(Npgsql.PostgresException)
                    {
                        cmd.CommandText = "update Checkins SET total = total + 1 where business_id = '" + ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString() + "' AND day = '" + DOW + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            //while (reader.Read())
                            //{
                            //    reviewlist.Items.Add(reader.GetString(0));
                            //}
                        }
                    }
                }
                conn.Close();
            }
        }

        private void addFriends()
        {
            FriendGrid.Items.Clear();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select name, average_stars, yelping_since from friend, users where (friend.friend_id = users.user_id) and (friend.user_id='" + G_UID + "');";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime DT = reader.GetDateTime(2).Date;
                            string datestring = DT.Year.ToString() + "-" + DT.Month.ToString() + "-" + DT.Day.ToString();

                            FriendGrid.Items.Add(new Friend() { friendname = reader.GetString(0), friendstars = reader.GetDouble(1).ToString(), frienddate = datestring});
                        }
                    }
                }
                conn.Close();
            }
        }

        private void addFavorites()
        {
            FavoriteGrid.Items.Clear();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    string UID = G_UID;
                    cmd.CommandText = "select name, reviewrating, city, zipcode, address, business.business_id from favorites, business where (favorites.business_id = business.business_id) and user_id = '"+ UID + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FavoriteGrid.Items.Add(new Favorite() { favename = reader.GetString(0), favestars = reader.GetDouble(1), favecity = reader.GetString(2), favezip = reader.GetString(3), faveaddress = reader.GetString(4), favebid = reader.GetString(5)});
                        }
                    }
                }
                    conn.Close();
            }
        }

        private void SubmitFavorites()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    string UID = G_UID;
                    cmd.CommandText = "insert into favorites (business_id, user_id) values ('" + ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString() + "' , '" + UID + "');";
                    using (var reader = cmd.ExecuteReader())
                    {
                        //while (reader.Read())
                        //{
                        //    FavoriteGrid.Items.Add(new Favorite() { favename = reader.GetString(0), favestars = reader.GetDouble(1), favecity = reader.GetString(2), favezip = reader.GetString(3), faveaddress = reader.GetString(4) });
                        //}
                    }
                }
                conn.Close();
            }
            addFavorites();
        }

        private void UpdateCoordinates()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    string UID = G_UID;
                    cmd.CommandText = "update users set (lat, long) = ('" + UIDlatBox.Text + "', '" + UIDlongBox.Text +"') where user_id = '"+ UID +"';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        //while (reader.Read())
                        //{
                        //    FavoriteGrid.Items.Add(new Favorite() { favename = reader.GetString(0), favestars = reader.GetDouble(1), favecity = reader.GetString(2), favezip = reader.GetString(3), faveaddress = reader.GetString(4) });
                        //}
                    }
                }
                conn.Close();
            }
        }


        private void RemoveFavorites()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    string UID = G_UID;
                    cmd.CommandText = "delete from favorites where user_id = '" + UID + "' and business_id = '" + ((Milestone1.MainWindow.Favorite)FavoriteGrid.SelectedValue).favebid.ToString() + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        //while (reader.Read())
                        //{
                        //    FavoriteGrid.Items.Add(new Favorite() { favename = reader.GetString(0), favestars = reader.GetDouble(1), favecity = reader.GetString(2), favezip = reader.GetString(3), faveaddress = reader.GetString(4) });
                        //}
                    }
                }
                conn.Close();
            }
            addFavorites();

        }

        public double getMeridian(string ulatitude, string ulongitude, double blatitude, double blongitude)
        {
            //https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates/44703178#44703178
            if(ulatitude == "" || ulongitude == "")
            {
                return -1;
            }
            var sCoord = new GeoCoordinate(Convert.ToDouble(ulatitude), Convert.ToDouble(ulongitude));
            var eCoord = new GeoCoordinate(blatitude, blongitude);
            return Math.Round(sCoord.GetDistanceTo(eCoord) * 0.000621371, 2); //convert meters to miles and round to the nearest hundredth
        }

        public void addColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Business Name";
            col1.Binding = new Binding("name");
            col1.Width = 255;
            businessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Address";
            col2.Binding = new Binding("address");
            businessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            businessGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "State";
            col4.Binding = new Binding("state");
            businessGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Distance (Miles)";
            col5.Binding = new Binding("distance");
            businessGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "# of reviews";
            col6.Binding = new Binding("count");
            businessGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = "Avg. Review Rating";
            col7.Binding = new Binding("rating");
            businessGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Header = "Total Checkins";
            col8.Binding = new Binding("checkins");
            businessGrid.Columns.Add(col8);



            //DataGridTextColumn col5 = new DataGridTextColumn();
            //col5.Header = "bid";
            //col5.Binding = new Binding("business_id");
            //businessGrid.Columns.Add(col5);
        }

        public void addColumns2Review()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "User Name";
            col1.Binding = new Binding("username");
            col1.Width = 100;
            reviewlist.Columns.Add(col1);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Date";
            col3.Binding = new Binding("date");
            col3.Width = 100;
            reviewlist.Columns.Add(col3);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Text";
            col2.Binding = new Binding("text");
            col2.Width = 20000;
            reviewlist.Columns.Add(col2);
        }

        public void addColumns2Friend()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Name";
            col1.Binding = new Binding("friendname");
            col1.Width = 100;
            FriendGrid.Columns.Add(col1);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Average Stars";
            col3.Binding = new Binding("friendstars");
            col3.Width = 100;
            FriendGrid.Columns.Add(col3);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Yelping Since";
            col2.Binding = new Binding("frienddate");
            col2.Width = 200;
            FriendGrid.Columns.Add(col2);
        }

        public void addColumns2Favorite()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Business Name";
            col1.Binding = new Binding("favename");
            col1.Width = 100;
            FavoriteGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Stars";
            col2.Binding = new Binding("favestars");
            col2.Width = 100;
            FavoriteGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("favecity");
            col3.Width = 100;
            FavoriteGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Zipcode";
            col4.Binding = new Binding("favezip");
            col4.Width = 100;
            FavoriteGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Address";
            col5.Binding = new Binding("faveaddress");
            col5.Width = 200;
            FavoriteGrid.Columns.Add(col5);
        }

        public void addColumns2Latest()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "User Name";
            col1.Binding = new Binding("latename");
            col1.Width = 100;
            LatestGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Business";
            col2.Binding = new Binding("latebusiness");
            col2.Width = 100;
            LatestGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("latecity");
            col3.Width = 100;
            LatestGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Text";
            col4.Binding = new Binding("latetext");
            col4.Width = 20000;
            LatestGrid.Columns.Add(col4);
        }

        private void statelist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            addCities();
            if (statelist.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name, address, city, state, reviewrating, review_count, num_checkins, business_id, zipcode, latitude, longitude FROM business WHERE state = '" + statelist.SelectedItem.ToString() + "' ;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                double latitude = reader.GetDouble(9);
                                double longitude = reader.GetDouble(10);
                                double debug = reader.GetDouble(4);

                                businessGrid.Items.Add(new Business() { name = reader.GetString(0), address = reader.GetString(1), city = reader.GetString(2), state = reader.GetString(3), distance = getMeridian(UIDlatBox.Text, UIDlongBox.Text, reader.GetDouble(9), reader.GetDouble(10)),  rating = reader.GetDouble(4), count = reader.GetInt32(5), checkins = reader.GetInt32(6), bid = reader.GetString(7), zipcode = reader.GetString(8)});
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }

        private void citylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            addZipCodes();
            if ((citylist.SelectedIndex) > -1 && (statelist.SelectedIndex > -1))
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name, address, city, state, reviewrating, review_count, num_checkins, business_id, zipcode, latitude, longitude FROM business WHERE city = '" + citylist.SelectedItem.ToString() + "' and state = '" +statelist.SelectedItem.ToString()+ "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessGrid.Items.Add(new Business() { name = reader.GetString(0), address = reader.GetString(1), city = reader.GetString(2), state = reader.GetString(3), distance = getMeridian(UIDlatBox.Text, UIDlongBox.Text, reader.GetDouble(9), reader.GetDouble(10)), rating = reader.GetDouble(4), count = reader.GetInt32(5), checkins = reader.GetInt32(6), bid = reader.GetString(7), zipcode = reader.GetString(8) });
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }

        private void ziplist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            addCats();
            if ((ziplist.SelectedIndex) > -1 && (citylist.SelectedIndex > -1) && (statelist.SelectedIndex > -1))
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name, address, city, state, reviewrating, review_count, num_checkins, business_id, zipcode, latitude, longitude FROM business WHERE zipcode = '" + ziplist.SelectedItem.ToString() + "' and city = '" + citylist.SelectedItem.ToString() + "' and state = '" + statelist.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                double latitude = reader.GetDouble(9);
                                double longitude = reader.GetDouble(10);

                                businessGrid.Items.Add(new Business() { name = reader.GetString(0), address = reader.GetString(1), city = reader.GetString(2), state = reader.GetString(3), distance = getMeridian(UIDlatBox.Text, UIDlongBox.Text, reader.GetDouble(9), reader.GetDouble(10)), rating = reader.GetDouble(4), count = reader.GetInt32(5), checkins = reader.GetInt32(6), bid = reader.GetString(7), zipcode = reader.GetString(8) });
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }

        private void catlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            if ((ziplist.SelectedIndex) > -1 && (citylist.SelectedIndex > -1) && (statelist.SelectedIndex > -1) && (catlist.SelectedIndex > -1))
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;

                        cmd.CommandText = "SELECT distinct business.business_id, address, city, state, reviewrating, review_count, num_checkins, name, zipcode, latitude, longitude FROM business, categories WHERE business.business_id = categories.business_id and zipcode = '" + ziplist.SelectedItem.ToString() + "' and city = '" + citylist.SelectedItem.ToString() + "' and state = '" + statelist.SelectedItem.ToString() + "'";
                        foreach(var item in catlist.SelectedItems)
                        {
                            cmd.CommandText += " AND business.business_id IN (SELECT business_id FROM categories where category = '"
                                + item + "')";
                        }
                        cmd.CommandText += ";";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                double latitude = reader.GetDouble(9);
                                double longitude = reader.GetDouble(10);

                                businessGrid.Items.Add(new Business() { bid = reader.GetString(0), address = reader.GetString(1), city = reader.GetString(2), state = reader.GetString(3), distance = getMeridian(UIDlatBox.Text, UIDlongBox.Text, reader.GetDouble(9), reader.GetDouble(10)), rating = reader.GetDouble(4), count = reader.GetInt32(5), checkins = reader.GetInt32(6), name = reader.GetString(7), zipcode = reader.GetString(8) });
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }

        private void businessGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addReviews();
            addDOW();
            string BN = "Business Name";
            string BA = "Address";
            try
            {
                BN = ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).name.ToString();
                BA = ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).address.ToString();
                addCats2Display();
            }
            catch(NullReferenceException)
            {
                BN = "Business Name";
                BA = "Address";
                CatDisplay.Text = "";
            }
            SelectedBusiness.Text = BN;
            SelectedAddress.Text = BA;

        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            string comment = commentbox.Text;
            string rating = ratingbox.Text;
            int f = 0;
            //MessageBox.Show(comment);
            if (businessGrid.SelectedIndex > -1)
            {
                if (comment == "")
                {
                    MessageBox.Show("Please leave a comment when submitting a review.");
                }
                else if ((int.TryParse(rating, out f)) && (f >= 0) && (f <= 5))
                {
                    submitReviews(comment, rating);
                    commentbox.Clear();
                    ratingbox.Clear();
                }
                else
                {
                    MessageBox.Show("Please also leave a rating when submitting a review.");
                }
            }
            else
            {
                MessageBox.Show("Please Select a Business when submitting a review.");
            }
        }

        private void UsernameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //MessageBox.Show(UsernameBox.Text);
            addUID(UsernameBox.Text);
        }

        private void UseridBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                G_UID = UseridBox.SelectedItem.ToString();
            }
            catch (NullReferenceException)
            {
            }
            AddUserInfo();
            addFriends();
            addFavorites();
            addLatest();
            //BfcNxKpnF9z5wJLXY7elRg
        }

        private void editbutton_Click(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/6808739/how-to-convert-color-code-into-media-brush
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#ffffff");

            UIDlatBox.Background = brush;
            UIDlatBox.IsReadOnly = false;
            UIDlongBox.Background = brush;
            UIDlongBox.IsReadOnly = false;
        }

        private void updatebutton_Click(object sender, RoutedEventArgs e)
        {
            //Same as editbutton_Click function
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#e1e4ed");

            UIDlatBox.Background = brush;
            UIDlatBox.IsReadOnly = true;
            UIDlongBox.Background = brush;
            UIDlongBox.IsReadOnly = true;


            double f;
            double g;
            if ((double.TryParse(UIDlatBox.Text, out f)) && (double.TryParse(UIDlongBox.Text, out g)))
            {
                if (f > 90 || f < -90 || g > 90 || g < -90)
                {
                    MessageBox.Show("Latitudes and Longitudes must be less than or equal to 90 or greater than or equal to -90");
                    UIDlatBox.Text = "";
                    UIDlongBox.Text = "";
                }
                else
                {
                    UpdateCoordinates();
                }
            }
            else
            {
                MessageBox.Show("Please Enter Valid Coordinates");
                UIDlatBox.Text = "";
                UIDlongBox.Text = "";
            }
        }

        private void Checkinbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                submitCheckins();
                MessageBox.Show("Check-in Successful");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("You must select a Business first");
            }
        }

        private void ShowCheckinbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Graph myGraph = new Graph(((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString());
                myGraph.Show();
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("You must select a Business first");
            }
        }

        private void AddFavesbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show(((Milestone1.MainWindow.Business)businessGrid.SelectedValue).name.ToString() + " has been added as a favorite");
                SubmitFavorites();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("You must select a Business first");
            }
            catch (Npgsql.PostgresException)
            {
            }
        }

        private void ShowReviewsbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Reviews AllReviews = new Reviews(((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString());
                AllReviews.Show();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("You must select a business first");
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveFavorites();
        }
    }
}
