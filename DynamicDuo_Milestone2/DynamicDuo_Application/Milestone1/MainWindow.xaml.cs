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

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Business
        {
            public string name { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zipcode { get; set; }
            public string bid { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            addStates();
            addColumns2Grid();
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

        public void addReviews()
        {
            reviewlist.Items.Clear();
            if (businessGrid.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select review_stars from business, review where(business.business_id = review.business_id) and business.business_id = '" + ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString() + "';";
                        //var h1 = ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reviewlist.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                    conn.Close();
                }
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
                        string UID = "om5ZiponkpRqUNa3pVPiRg";
                        var h1 = ((Milestone1.MainWindow.Business)businessGrid.SelectedValue).bid.ToString();
                        cmd.Connection = conn;
                        cmd.CommandText = "insert into review(business_id, user_id, review_id, review_stars, text) values ('" + h1 +"', '" + UID + "', '" + RID + "', '" + stars + "', '" + comment + "');";
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

        public void addColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Business Name";
            col1.Binding = new Binding("name");
            col1.Width = 255;
            businessGrid.Columns.Add(col1);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City Name";
            col3.Binding = new Binding("city");
            businessGrid.Columns.Add(col3);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "State Name";
            col2.Binding = new Binding("state");
            businessGrid.Columns.Add(col2);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "ZipCode";
            col4.Binding = new Binding("zipcode");
            businessGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "bid";
            col5.Binding = new Binding("business_id");
            businessGrid.Columns.Add(col5);
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
                        cmd.CommandText = "SELECT name, city, state, zipcode, business_id FROM business WHERE state = '" + statelist.SelectedItem.ToString() + "' ;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessGrid.Items.Add(new Business() { name = reader.GetString(0), city = reader.GetString(1), state = reader.GetString(2), zipcode = reader.GetString(3), bid = reader.GetString(4)});
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
                        cmd.CommandText = "SELECT name, city, state, zipcode, business_id FROM business WHERE city = '" + citylist.SelectedItem.ToString() + "' and state = '" +statelist.SelectedItem.ToString()+ "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessGrid.Items.Add(new Business() { name = reader.GetString(0), city = reader.GetString(1), state = reader.GetString(2), zipcode = reader.GetString(3), bid = reader.GetString(4) });
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
                        cmd.CommandText = "SELECT name, city, state, zipcode, business_id FROM business WHERE zipcode = '" + ziplist.SelectedItem.ToString() + "' and city = '" + citylist.SelectedItem.ToString() + "' and state = '" + statelist.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessGrid.Items.Add(new Business() { name = reader.GetString(0), city = reader.GetString(1), state = reader.GetString(2), zipcode = reader.GetString(3), bid = reader.GetString(4) });
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

                        cmd.CommandText = "SELECT distinct name, city, state, zipcode, business.business_id FROM business, categories WHERE business.business_id = categories.business_id and zipcode = '" + ziplist.SelectedItem.ToString() + "' and city = '" + citylist.SelectedItem.ToString() + "' and state = '" + statelist.SelectedItem.ToString() + "'";
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
                                businessGrid.Items.Add(new Business() { name = reader.GetString(0), city = reader.GetString(1), state = reader.GetString(2), zipcode = reader.GetString(3), bid = reader.GetString(4) });
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
    }
}
