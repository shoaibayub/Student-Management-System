using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace VP_Project_SYSTEM
{
    /// <summary>
    /// Interaction logic for Course.xaml
    /// </summary>
    public partial class Course : Window
    {
        // Declare SqlConnection object
        SqlConnection Sqlcon = new SqlConnection(@"Data Source=SHOAIB-VBOX\SQLEXPRESS;Initial Catalog=NUTECH;Integrated Security=True;");

        public Course()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            StdDashboard studentDashboard = new StdDashboard();
            studentDashboard.Show();
            this.Close();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            // Clear all textboxes
            ClearTb();
        }

        private void AddCourse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validationtb())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Course (CourseName, Credits) VALUES (@CourseName, @Credits)", Sqlcon);
                    cmd.Parameters.AddWithValue("@CourseName", tbCourseName.Text);
                    cmd.Parameters.AddWithValue("@Credits", Convert.ToInt32(tbCredits.Text));

                    Sqlcon.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Course added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error adding course: " + ex.Message, "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Sqlcon.Close();
                ClearTb();
            }
        }

        private void ShowCourse_Click(object sender, RoutedEventArgs e)
        {
            Loadgrid();
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

                SqlCommand cmd = new SqlCommand("SELECT CourseID, CourseName, Credits FROM Course", Sqlcon);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Bind only the selected columns to the DataGrid
                datagrid.ItemsSource = dataTable.DefaultView;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error loading courses: " + ex.Message, "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (Sqlcon.State == ConnectionState.Open)
                {
                    Sqlcon.Close();
                }
            }
        }


        public bool Validationtb()
        {
            if (string.IsNullOrEmpty(tbCourseName.Text) || string.IsNullOrWhiteSpace(tbCourseName.Text))
            {
                MessageBox.Show("Please enter a valid course name.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                tbCourseName.Focus();
                return false;
            }

            else if (string.IsNullOrEmpty(tbCredits.Text) || !int.TryParse(tbCredits.Text, out int credits) || credits <= 0)
            {
                MessageBox.Show("Please enter a valid number of credits.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                tbCredits.Focus();
                return false;
            }

            return true;
        }

        public void ClearTb()
        {
            // Clear all textboxes
            tbCourseName.Clear();
            tbCredits.Clear();
        }
    }
}
