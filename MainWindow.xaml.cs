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

namespace VP_Project_SYSTEM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Teacher_Click (object sender, RoutedEventArgs e)
        {
            TeacherLogin stdobj = new TeacherLogin();
            stdobj.Show();
            this.Close();
        }
        private void Student_Click (object sender, RoutedEventArgs e)
        {
            StdLogin stdobj = new StdLogin ();
            stdobj.Show();
            this.Close();
        }
    }
}

