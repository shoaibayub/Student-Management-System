using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Xml.Linq;

namespace VP_Project_SYSTEM
{
    /// <summary>
    /// Interaction logic for TeacherLogin.xaml
    /// </summary>
    public partial class TeacherLogin : Window
    {
        public TeacherLogin()
        {
            InitializeComponent();
        }

        private void SignUp_Button_Click(object sender, RoutedEventArgs e)
        {
            TeacherSignUp objMain = new TeacherSignUp();
            objMain.Show();
            this.Close();
        }

        public void ClearTb()
        {
            tbEmail.Clear();
            tbPassword.Clear();
        }

        SqlConnection Sqlcon = new SqlConnection(@"Data Source=SHOAIB-VBOX\SQLEXPRESS;Initial Catalog=NUTECH;Integrated Security=True;");

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Sqlcon.State == ConnectionState.Closed)
                    Sqlcon.Open();

                string query = "SELECT COUNT(1) FROM TeacherSignUp WHERE Email = @Email AND Password = @Password";
                SqlCommand sqlcmd = new SqlCommand(query, Sqlcon);
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.Parameters.AddWithValue("@Email", tbEmail.Text);
                sqlcmd.Parameters.AddWithValue("@Password", SecureData.HashString(tbPassword.Password));

                int count = Convert.ToInt32(sqlcmd.ExecuteScalar());

                if (count == 1)
                {
                    MessageBox.Show("Login Successful.", "Authorized", MessageBoxButton.OK, MessageBoxImage.Information);
                    TeacherDashboard objhome = new TeacherDashboard();
                    objhome.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid Email or Password.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    ClearTb();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                Sqlcon.Close();
            }
        }

        private void Home(object sender, RoutedEventArgs e)
        {
            MainWindow objmain = new MainWindow();
            objmain.Show();
            this.Close();
        }
    }
}
