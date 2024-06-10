using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;

namespace VP_Project_SYSTEM
{
    public partial class TeacherSignUp : Window
    {
        // Declare SqlConnection object
        SqlConnection Sqlcon = new SqlConnection(@"Data Source=SHOAIB-VBOX\SQLEXPRESS;Initial Catalog=NUTECH;Integrated Security=True;");
        DataSet ds = new DataSet();

        public TeacherSignUp()
        {
            InitializeComponent();
        }

        public void ClearTb()
        {
            tbTeacherName.Clear();
            tbEmail.Clear();
            tbAddress.Clear();
            tbDepartment.Clear();
            tbContact.Clear();
            tbPassword.Clear();
            cbAgree.IsChecked = false;
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbAgree.IsChecked == true)
                {
                    if (Validationtb())
                    {
                        SqlCommand cmd1 = new SqlCommand("SELECT * FROM TeacherSignUp WHERE Email = @Email", Sqlcon);
                        cmd1.Parameters.AddWithValue("@Email", tbEmail.Text);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd1);
                        adapter.Fill(ds);
                        int i = ds.Tables[0].Rows.Count;
                        if (i > 0)
                        {
                            MessageBox.Show("Email: " + tbEmail.Text + " already Exists!!");
                            ds.Clear();
                            ClearTb();
                        }
                        else
                        {
                            SqlCommand cmd = new SqlCommand("INSERT INTO TeacherSignUp (TeacherName, Email, Address, Department, Contact, Password) " +
                                                            "VALUES (@TeacherName, @Email, @Address, @Department, @Contact, @Password)", Sqlcon);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@TeacherName", tbTeacherName.Text);
                            cmd.Parameters.AddWithValue("@Email", tbEmail.Text);
                            cmd.Parameters.AddWithValue("@Address", tbAddress.Text);
                            cmd.Parameters.AddWithValue("@Department", tbDepartment.Text);
                            cmd.Parameters.AddWithValue("@Contact", tbContact.Text);
                            cmd.Parameters.AddWithValue("@Password", SecureData.HashString(tbPassword.Password));

                            Sqlcon.Open();
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Your Form has been Submitted Successfully", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please agree to the terms and conditions", "Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Sqlcon.Close();
                ClearTb();
            }
        }

        public bool Validationtb()
        {
            if (string.IsNullOrEmpty(tbTeacherName.Text) || string.IsNullOrWhiteSpace(tbTeacherName.Text))
            {
                MessageBox.Show("Please enter valid data for Teacher Name", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                
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
            else if (string.IsNullOrEmpty(tbDepartment.Text) || string.IsNullOrWhiteSpace(tbDepartment.Text))
            {
                MessageBox.Show("Please enter a valid Department", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                
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
            return true;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            TeacherLogin loginWindow = new TeacherLogin(); // Create an instance of Login
            loginWindow.Show(); // Show the Login window
            this.Close(); // Close the current MainWindow
        }
    }
}
