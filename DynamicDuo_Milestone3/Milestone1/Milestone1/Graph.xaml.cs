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
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class Graph : Window
    {
        public Graph(string BID)
        {
            InitializeComponent();
            ColumnChart(BID);
        }

        private string buildConnString()
        {
            return "Host=localhost; Username=postgres; Password=zxcvbnm; Database = milestone1db";
        }

        private void ColumnChart(string BID)
        {
            string[] dow = new string[7] {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            int[] count = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            List<KeyValuePair<string, int>> myChartData = new List<KeyValuePair<string, int>>();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select day, total from checkins where business_id = '"+ BID +"';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(0) == "Monday")
                            {
                                count[0] = reader.GetInt32(1);
                            }
                            else if (reader.GetString(0) == "Tuesday")
                            {
                                count[1] = reader.GetInt32(1);
                            }
                            else if (reader.GetString(0) == "Wednesday")
                            {
                                count[2] = reader.GetInt32(1);
                            }
                            else if (reader.GetString(0) == "Thursday")
                            {
                                count[3] = reader.GetInt32(1);
                            }
                            else if (reader.GetString(0) == "Friday")
                            {
                                count[4] = reader.GetInt32(1);
                            }
                            else if (reader.GetString(0) == "Saturday")
                            {
                                count[5] = reader.GetInt32(1);
                            }
                            else if (reader.GetString(0) == "Sunday")
                            {
                                count[6] = reader.GetInt32(1);
                            }
                            else
                            {
                                MessageBox.Show("ERROR!");
                            }
                        }
                    }
                }
                conn.Close();
            }
            int i = 0;
            while (i < 7)
            {
                myChartData.Add(new KeyValuePair<string, int>(dow[i], count[i]));
                i++;
            }
            MyChart.DataContext = myChartData;
        }
    }
}
