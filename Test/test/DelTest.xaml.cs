using System;
using System.IO;
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
using System.Xml;
using System.Data.Odbc;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace test
{
    /// <summary>
    /// Логика взаимодействия для DelTest.xaml
    /// </summary>
    public partial class DelTest : Window
    {
        SqlConnection local;
        string fileName;
        public DelTest(SqlConnection inConnect)
        {
            InitializeComponent();
            local = inConnect;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand command1 = new SqlCommand("select LINK from Test where Test_Name= \'" + TestDelText.Text + "\'", local);
                SqlDataReader reader = command1.ExecuteReader();
                while (reader.Read())
                {
                    fileName = reader["LINK"].ToString();
                }
                reader.Close();
                SqlCommand command = new SqlCommand("delete from Test where Test_Name=\'" + TestDelText.Text + "\'", local);
                command.ExecuteNonQuery();

                File.Delete(fileName);

                MessageBox.Show("Тест удален");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
