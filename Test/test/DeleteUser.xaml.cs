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
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.Odbc;

namespace test
{
   
    /// <summary>
    /// Исключение, которое возникает, когда будет предпринята попытка удалить SUPER_USER
    /// </summary>
    [Serializable]
    public class DeleteSuperUserException : Exception
    {
        public DeleteSuperUserException() {
            
        }
        public DeleteSuperUserException(string message) : base(message) { }
        public DeleteSuperUserException(string message, Exception inner) : base(message, inner) { }
        protected DeleteSuperUserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Логика взаимодействия для DeleteUser.xaml
    /// </summary>
    /// 
    public partial class DeleteUser : Window
    {
        SqlConnection Local_connection;
        List<User> Cur_Table;
        string thisRole;
        string RoleData;
        string thisTable;
        public DeleteUser(SqlConnection connect, List<User> currentTable)
        {
            InitializeComponent();
            Cur_Table = currentTable;
            Local_connection = connect;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Login = LoginText.Text;
                if (Login == "SUPER_USER")
                {
                    throw new DeleteSuperUserException();
                }
                for (int i = 0; i < Cur_Table.Count; i++)
                {
                    if (Cur_Table[i].Login == Login)
                    {
                        thisRole = Cur_Table[i].Role;
                    }
                }
                if (thisRole == "teacher")
                {
                    thisTable = "Teacher";
                    RoleData = "Teachers";
                }
                else if (thisRole == "student")
                {
                    thisTable = "Student1";
                    RoleData = "Students";
                }
                else
                {
                    thisTable = "TestAutor";
                    RoleData = "TestAdmin";
                }
                SqlCommand DelThisTable = new SqlCommand("DELETE FROM " + thisTable + " WHERE Login = \'" +Login+"\' ;", Local_connection);
                SqlCommand DisBind = new SqlCommand("ALTER ROLE "+ RoleData + " DROP MEMBER "+ Login +";",Local_connection);
                SqlCommand DropLogin = new SqlCommand("DROP LOGIN "+ Login + ";", Local_connection);
                SqlCommand DropUser = new SqlCommand("DROP USER " + Login + ";", Local_connection);

                DelThisTable.ExecuteNonQuery();
                DisBind.ExecuteNonQuery();
                DropLogin.ExecuteNonQuery();
                DropUser.ExecuteNonQuery();

                DelThisTable.Dispose();
                DisBind.Dispose();
                DropLogin.Dispose();
                DropUser.Dispose();
                MessageBox.Show("Пользователь удален");
                this.Close();
            }

            catch(DeleteSuperUserException)
            {
                MessageBox.Show("Недопустимо удалять пользователя с именем SUPER_USER");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
