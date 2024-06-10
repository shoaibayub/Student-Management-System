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
    /// <summary>
    /// Interaction logic for StdDashboard.xaml
    /// </summary>
    public partial class StdDashboard : Window
    {
        SqlConnection Sqlcon = new SqlConnection(@"Data Source=SHOAIB-VBOX\SQLEXPRESS;
                                                Initial Catalog=NUTECH;Integrated Security=True;");
        DataSet ds = new DataSet();

        public StdDashboard()
        {
            InitializeComponent();
        }

        public void ClearTb()
        {
            tbName.Clear();
            tbEmail.Clear();
            tbAddress.Clear();
            tbSemester.Clear();
            tbDept.Clear();
            tbContact.Clear();
            tbPassword.Clear();
            tbStudentID.Clear();
        }

        public bool Validationtb()
        {
            if (string.IsNullOrEmpty(tbName.Text) || string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Please enter valid data for Name", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(tbEmail.Text) || !Regex.IsMatch(tbEmail.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Please enter a valid Email address", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(tbAddress.Text) || string.IsNullOrWhiteSpace(tbAddress.Text))
            {
                MessageBox.Show("Please enter valid data for Address", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(tbSemester.Text) || !int.TryParse(tbSemester.Text, out int semester) || semester < 1 || semester > 8)
            {
                MessageBox.Show("Please enter a valid Semester (1-8)", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(tbDept.Text) || string.IsNullOrWhiteSpace(tbDept.Text))
            {
                MessageBox.Show("Please enter valid data for Department", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(tbContact.Text) || string.IsNullOrWhiteSpace(tbContact.Text))
            {
                MessageBox.Show("Please enter a valid Contact number", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(tbPassword.Password) || string.IsNullOrWhiteSpace(tbPassword.Password))
            {
                MessageBox.Show("Please enter valid data for Password", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(tbStudentID.Text) || string.IsNullOrWhiteSpace(tbStudentID.Text))
            {
                MessageBox.Show("Please enter a valid StudentID number", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
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

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validationtb())
                {
                    SqlCommand cmd = new SqlCommand("UPDATE StudentSignUp SET StudentName=@StudentName, Email=@Email, Address=@Address, Semester=@Semester, Department=@Dept, Contact=@Contact, Password=@Password " +
                                                    "WHERE StudentID=@StudentID", Sqlcon);
                    cmd.Parameters.AddWithValue("@StudentName", tbName.Text);
                    cmd.Parameters.AddWithValue("@Email", tbEmail.Text);
                    cmd.Parameters.AddWithValue("@Address", tbAddress.Text);
                    cmd.Parameters.AddWithValue("@Semester", Convert.ToInt32(tbSemester.Text));
                    cmd.Parameters.AddWithValue("@Dept", tbDept.Text);
                    cmd.Parameters.AddWithValue("@Contact", tbContact.Text);
                    cmd.Parameters.AddWithValue("@Password", SecureData.HashString(tbPassword.Password));
                    cmd.Parameters.AddWithValue("@StudentID", tbStudentID.Text); 

                    Sqlcon.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student record updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error updating record: " + ex.Message, "Update Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Sqlcon.Close();
                ClearTb();
                Loadgrid();
            }
        }



        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            StdLogin loginWindow = new StdLogin();
            loginWindow.Show();
            this.Close();
        }


        private void Enroll_Click(object sender, RoutedEventArgs e)
        {
            Course coursesWindow = new Course();
            coursesWindow.Show();
            this.Close();
        }

        

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            Loadgrid();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Sqlcon.Open();
                string query = "SELECT * FROM StudentSignUp WHERE StudentID LIKE @StudentID";
                SqlCommand cmd = new SqlCommand(query, Sqlcon);
                cmd.Parameters.AddWithValue("@StudentID", "%" + tbSearch.Text + "%");

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

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Sqlcon.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM StudentSignUp WHERE StudentID = @StudentID", Sqlcon);
                cmd.Parameters.AddWithValue("@StudentID", tbSearch.Text);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Teacher record has been deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No record found with the given name.", "Delete Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                Loadgrid();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error deleting teacher record: " + ex.Message, "Delete Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Sqlcon.Close();
                ClearTb();
            }
        }

    }
}
