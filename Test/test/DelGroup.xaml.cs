using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace test
{
    /// <summary>
    /// Логика взаимодействия для DelGroup.xaml
    /// </summary>
    public partial class DelGroup : Window
    {
        SqlConnection local;
        public DelGroup(SqlConnection _local)
        {
            InitializeComponent();
            local = _local;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand command = new SqlCommand("Delete from Groups where Name = \'" + GroupText.Text + "\'", local);
                command.ExecuteNonQuery();
                MessageBox.Show("Удаление выполнено");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
