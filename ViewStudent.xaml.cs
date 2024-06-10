using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace VP_Project_SYSTEM
{
    
    public partial class ViewStudent : Window
    {
        public ViewStudent()
        {
            InitializeComponent();
        }
        SqlConnection Sqlcon = new SqlConnection(@"Data Source=SHOAIB-VBOX\SQLEXPRESS;
                                                Initial Catalog=NUTECH;Integrated Security=True;");
        DataSet ds = new DataSet();

        public void ClearTb ()
        {
            tbStudentID.Clear();
        }

        public bool Validationtb()
        {
            if (string.IsNullOrEmpty(tbStudentID.Text) || string.IsNullOrWhiteSpace(tbStudentID.Text))
            {
                MessageBox.Show("Please enter valid data for ID", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearTb();
                return false;
            }
            return true;
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            TeacherDashboard teacherWindow = new TeacherDashboard();
            teacherWindow.Show();
            this.Close();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearTb();
        }

        private void Loadgrid()
        {
            try
            {
                if (Sqlcon.State != ConnectionState.Open)
                {
                    Sqlcon.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * FROM StudentSignUp", Sqlcon);
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                datagrid.ItemsSource = dt.DefaultView;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error loading grid: " + ex.Message, "Load Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Sqlcon.Close();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Sqlcon.Open();
                string query = "SELECT * FROM StudentSignUp WHERE StudentID LIKE @StudentID";
                SqlCommand cmd = new SqlCommand(query, Sqlcon);
                cmd.Parameters.AddWithValue("@StudentID", "%" + tbStudentID.Text + "%");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                datagrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                Sqlcon.Close();
            }
        }

        private void All_Students_Click(object sender, RoutedEventArgs e)
        {
            Loadgrid();
        }
    }
}
