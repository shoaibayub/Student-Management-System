using Microsoft.VisualBasic.Logging;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace VP_Project_SYSTEM
{
    /// <summary>
    /// Interaction logic for TeacherDashboard.xaml
    /// </summary>
    public partial class TeacherDashboard : Window
    {
        public TeacherDashboard()
        {
            InitializeComponent();
        }

        SqlConnection Sqlcon = new SqlConnection(@"Data Source=SHOAIB-VBOX\SQLEXPRESS;Initial Catalog=NUTECH;Integrated Security=True;");

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearTb();
        }

        public void ClearTb()
        {
            tbTeacherName.Clear();
            tbEmail.Clear();
            tbAddress.Clear();
            tbDepartmentID.Clear();
            tbContact.Clear();
            tbPassword.Clear();
            tbSearch.Clear();
        }

        private bool Validationtb()
        {
            if (string.IsNullOrEmpty(tbTeacherName.Text) || string.IsNullOrWhiteSpace(tbTeacherName.Text))
            {
                MessageBox.Show("Please enter valid data for Name", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearTb();
                return false;
            }
            else if (string.IsNullOrEmpty(tbEmail.Text) || !Regex.IsMatch(tbEmail.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Please enter a valid Email address", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearTb();
                return false;
            }
            else if (string.IsNullOrEmpty(tbAddress.Text) || string.IsNullOrWhiteSpace(tbAddress.Text))
            {
                MessageBox.Show("Please enter valid data for Address", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearTb();
                return false;
            }
            else if (string.IsNullOrEmpty(tbDepartmentID.Text) || string.IsNullOrWhiteSpace(tbDepartmentID.Text))
            {
                MessageBox.Show("Please enter valid data for Department ID", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearTb();
                return false;
            }
            else if (string.IsNullOrEmpty(tbContact.Text) || string.IsNullOrWhiteSpace(tbContact.Text))
            {
                MessageBox.Show("Please enter valid data for Contact", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearTb();
                return false;
            }
            else if (string.IsNullOrEmpty(tbPassword.Password) || string.IsNullOrWhiteSpace(tbPassword.Password))
            {
                MessageBox.Show("Please enter a valid Password", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearTb();
                return false;
            }
            return true;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validationtb())
                {
                    string query = "UPDATE TeacherSignUp SET TeacherName=@TeacherName, Email=@Email, Address=@Address, Department=@Department, Contact=@Contact, Password=@Password WHERE TeacherName=@TeacherName";
                    SqlCommand cmd = new SqlCommand(query, Sqlcon);

                    cmd.Parameters.AddWithValue("@TeacherName", tbTeacherName.Text);
                    cmd.Parameters.AddWithValue("@Email", tbEmail.Text);
                    cmd.Parameters.AddWithValue("@Address", tbAddress.Text);
                    cmd.Parameters.AddWithValue("@Department", tbDepartmentID.Text);
                    cmd.Parameters.AddWithValue("@Contact", tbContact.Text);
                    cmd.Parameters.AddWithValue("@Password", tbPassword.Password);

                    Sqlcon.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Profile updated successfully!", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("No record found with the given name.", "Update Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    Loadgrid();
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
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Sqlcon.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM TeacherSignUp WHERE TeacherName = @TeacherName", Sqlcon);
                cmd.Parameters.AddWithValue("@TeacherName", tbSearch.Text);

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

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Sqlcon.Open();
                string query = "SELECT * FROM TeacherSignUp WHERE TeacherName LIKE @TeacherName";
                SqlCommand cmd = new SqlCommand(query, Sqlcon);
                cmd.Parameters.AddWithValue("@TeacherName", "%" + tbSearch.Text + "%");

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

        private void Loadgrid()
        {
            try
            {
                if (Sqlcon.State != ConnectionState.Open)
                {
                    Sqlcon.Open();
                }
                
                SqlCommand cmd = new SqlCommand("SELECT * FROM TeacherSignUp", Sqlcon);
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

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            TeacherLogin loginWindow = new TeacherLogin();
            loginWindow.Show();
            this.Close();
        }

        private void TeacherProfile_Click(object sender, RoutedEventArgs e)
        {
            Loadgrid();
        }

        private void View_StudentClick(object sender, RoutedEventArgs e)
        {
            ViewStudent viewStudentWindow = new ViewStudent();
            viewStudentWindow.Show();
            this.Close();
        }

    }
}
