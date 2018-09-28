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
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        SqlConnection local_connection;
        public AddUserWindow(SqlConnection connection)
        {
            InitializeComponent();
            local_connection = connection;
            GroupGrid.IsEnabled = false;
            RoleSelected.SelectionChanged += (sender, e) => {
                TextBlock ThisRole = RoleSelected.SelectedItem as TextBlock;
                if(ThisRole.Text == "Students")
                    GroupGrid.IsEnabled = true;
                else
                {
                    GroupGrid.IsEnabled = false;
                }    
            
            };
            
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                
                string Login                        = LoginText.Text;
                string password                     = PasswordText.Text;
                string Name                         = NameText.Text;
                string SecondName                   = SecondNameText.Text;
                string ThirdName                    = ThirdNameText.Text;
                TextBlock thisTextblock             = RoleSelected.SelectedItem as TextBlock;
                string ThisRole                     = thisTextblock.Text;
                string ThisTable                    = "";

                string QueryFromAddLoginUser        = "CREATE LOGIN " + Login + " WITH PASSWORD=\'" + password + "\',DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[русский], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON";
                string QueryFromAddLoginForDatabase = "CREATE USER " + Login + " FOR LOGIN " + Login + " WITH DEFAULT_SCHEMA=[dbo]";
                string QueryFromBindingUserInRole   = "ALTER ROLE " + ThisRole + " ADD MEMBER " + Login + ";";
                 if (ThisRole == "Teachers")
                {
                    ThisTable = "Teacher(Login, Name, Second_Name, Third_Name)";
                }
                else if (ThisRole == "Students")
                {
                    ThisTable = "Student1(Login, Name, Second_Name, Third_Name, ID_Group)";
                    
                }
                else if (ThisRole == "TestAdmin")
                {
                    ThisTable = "TestAutor(Login, Name, Second_Name, Third_Name)";
                }
                
                
                 string QueryFromViewToTable = "INSERT INTO " + ThisTable + " VALUES(\'" + Login + "\', \'" + Name + "\', \'" + SecondName + "\', \'" + ThirdName;
                 if (ThisRole == "Students")
                 {
                     QueryFromViewToTable += "\', " + getDataGroupName(GroupText.Text) + ");";
                 }
                 else
                 {
                     QueryFromViewToTable += "\');";
                 }
                SqlCommand CommandForAdd            = new SqlCommand(QueryFromAddLoginUser, local_connection);
                SqlCommand CommandForAddForDB       = new SqlCommand(QueryFromAddLoginForDatabase, local_connection);
                SqlCommand CommandForBind           = new SqlCommand(QueryFromBindingUserInRole, local_connection);
                SqlCommand CommandForViewToTable    = new SqlCommand(QueryFromViewToTable, local_connection);

                CommandForAdd.ExecuteNonQuery();
                CommandForAddForDB.ExecuteNonQuery();
                CommandForBind.ExecuteNonQuery();
                CommandForViewToTable.ExecuteNonQuery();

                CommandForAdd.Dispose();
                CommandForAddForDB.Dispose();
                CommandForBind.Dispose();
                CommandForViewToTable.Dispose();
                MessageBox.Show("Пользователь добавлен");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int getDataGroupName(string GroupName)
        {
            int Id = 0;
            if (GroupName != null)
            {
                SqlCommand command = new SqlCommand("Select ID from Groups where Name = \'" + GroupName + "\'", local_connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Id = (int)reader["ID"];
                }
                reader.Close();
                return Id;
            }
            else return -1;
            
        }

    }
}
