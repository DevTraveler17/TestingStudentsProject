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
using System.Data.Odbc;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool admin = false;
        string userName;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void EnterBut_Click(object sender, RoutedEventArgs e)
        {
            string connect_string;
            if (checkBox1.IsChecked == true)
            {
                connect_string = "User=" + LoginText.Text + ";Password=" + PasswordText.Password + ";Initial Catalog=DB_Prozorov;server=" + ServerText.Text + ";";
            }
            else
            {
                connect_string = "User=" + LoginText.Text + ";Password=" + PasswordText.Password + ";Initial Catalog=DB_Prozorov;server=DESKTOP-COH8BQV\\SQLEXPRESS;";
            }
            try
            {
                #region Mysql - подключение
               // MySqlConnection myConnection = new MySqlConnection(Connect);
               // myConnection.Open(); //Устанавливаем соединение с базой данных.
               // if (LoginText.Text == "root")
               // {
               //     admin = true;
               //     userName = "Admin";
               // }
               // else
               // {
               //     admin = false;
               //     userName = LoginText.Text;
               // }
               // myConnection.Close(); //Обязательно закрываем соединение!
               // MessageBox.Show("Соединение установлено");
               //DBView d = new DBView(admin, userName, Connect, PasswordText.Text);
               //d.Show();
               //this.Hide();
                #endregion
                #region MSSQL подключение
                SqlConnection connect = new SqlConnection(connect_string);
                connect.Open();
                if (LoginText.Text == "sa")
                     {
                         admin = true;
                         userName = "Admin";
                         MessageBox.Show("Соединение установлено");
                         DBView_User d = new DBView_User(admin, userName, connect_string, PasswordText.Password);
                         d.Show();
                         this.Hide();
                     }
                     else
                     {
                         admin = false;
                         userName = LoginText.Text;
                         MessageBox.Show("Соединение установлено");
                         UserWindow d = new UserWindow(connect_string, userName);
                         d.Show();
                         this.Hide();
                     }
                connect.Close();
               
                #endregion
            }
            catch (Exception ex) { MessageBox.Show("Ошибка установки соединения\nПопробуйте ещё раз\n" + ex.Message); }
         
            
        }
    }
}
