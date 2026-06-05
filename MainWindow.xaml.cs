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
using MySqlConnector;
using System.Data;

namespace Mozi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FilmekLoad();
        }

        string connstr = "server=localhost;database=mozi;user=root;password=;";

        private void FilmekLoad()
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();

                string sql = "SELECT * FROM filmek";

                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);

                DataTable dt = new DataTable();

                adapter.Fill(dt);

                dGrid.ItemsSource = dt.DefaultView;

                conn.Close();
            } 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void jegyekSzama_Click(object sender, RoutedEventArgs e)
        {
            if (dGrid.SelectedItem == null)
            {
                MessageBox.Show("Nincs kijelölt film!");
                return;
            }

            DataRowView sor = (DataRowView)dGrid.SelectedItem;

            int id = Convert.ToInt32(sor["id"]);

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();

                string sql = @"SELECT SUM(darab) FROM jegyek WHERE film_id = @id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", id);

                object result = cmd.ExecuteScalar();

                int db = 0;

                if (result != DBNull.Value)
                {
                    db = Convert.ToInt32(result);
                }

                MessageBox.Show($"Összes eladott jegy: {db}");

                conn.Close();
            }
        }

        private void lgDragabb_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();

                string sql = @"SELECT cim FROM filmek ORDER BY ar DESC LIMIT 1";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                string cim = cmd.ExecuteScalar().ToString();

                MessageBox.Show( $"Legdrágább film: {cim}");

                conn.Close();
            }
        }
    }
}
