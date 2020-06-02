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
using System.Windows.Shapes;
using Npgsql;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for Reviews.xaml
    /// </summary>
    public partial class Reviews : Window
    {

        public class Review
        {
            public string Rdate { get; set; }
            public string Rname { get; set; }
            public int Rstars { get; set; }
            public string Rtext { get; set; }
            public int Rfunny { get; set; }
            public int Ruseful { get; set; }
            public int Rcool { get; set; }
        }
        public Reviews(string BID)
        {
            InitializeComponent();
            addColumns2AllReviews();
            addAllReviews(BID);
        }

        private void addColumns2AllReviews()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Date";
            col1.Binding = new Binding("Rdate");
            col1.Width = 100;
            AllReviewGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "User Name";
            col2.Binding = new Binding("Rname");
            col2.Width = 100;
            AllReviewGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Stars";
            col3.Binding = new Binding("Rstars");
            col3.Width = 100;
            AllReviewGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Text";
            col4.Binding = new Binding("Rtext");
            col4.Width = 100;
            AllReviewGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Funny";
            col5.Binding = new Binding("Rfunny");
            col5.Width = 100;
            AllReviewGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Useful";
            col6.Binding = new Binding("Ruseful");
            col6.Width = 100;
            AllReviewGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = "Cool";
            col7.Binding = new Binding("Rcool");
            col7.Width = 100;
            AllReviewGrid.Columns.Add(col7);
        }

        private string buildConnString()
        {
            return "Host=localhost; Username=postgres; Password=zxcvbnm; Database = milestone1db";
        }

        public void addAllReviews(string BID)
        {
            AllReviewGrid.Items.Clear();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                DateTime DT;
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select date, name, review_stars, text, funny, useful, cool from review, users where (review.user_id = users.user_id) and (business_id = '" + BID + "');";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DT = reader.GetDateTime(0).Date;
                            string datestring = DT.Year.ToString() + "-" + DT.Month.ToString() + "-" + DT.Day.ToString();

                            AllReviewGrid.Items.Add(new Review() { Rdate = datestring, Rname = reader.GetString(1), Rstars = reader.GetInt32(2), Rtext = reader.GetString(3), Rfunny = reader.GetInt32(4), Ruseful = reader.GetInt32(5), Rcool = reader.GetInt32(6) });
                        }
                    }
                }
                conn.Close();
            }
        }
    }
}
