using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Xml;
using System.Data.Odbc;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace test
{
	/// <summary>
	/// Interaction logic for UserWindow.xaml
	/// </summary>
	public partial class UserWindow : Window
	{
        string local_user_connection;
        string nameUser;
        string roleUser;
        SqlConnection connect;
		public UserWindow(string connection, string UserName)
		{
			this.InitializeComponent();
            #region others initializing
            local_user_connection = connection;
            nameUser = UserName;
            try
            {
                connect = new SqlConnection(local_user_connection);
                connect.Open();

                GetDataCurrentUser();
                MainGrid.Visibility = Visibility.Visible;
                StatisticsGrid.Visibility = Visibility.Hidden;
                TestsGrid.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
		}
        #region get data connecting user
        private void GetDataCurrentUser()
        {
            try
            {
                roleUser = validateRole();
                string queryActivateDataUser = "";
                int id_group = 0;
                #region validate role
                if (roleUser == "TestAdmin")
                {
                    queryActivateDataUser = "select * from TestAutor where Login = \'" + nameUser + "\';";
                }
                else if (roleUser == "Students")
                {
                    queryActivateDataUser = "select * from Student1 where Login = \'" + nameUser + "\';";
                } 
                else if(roleUser == "Teachers") 
                {
                    queryActivateDataUser = "select * from Teacher where Login = \'" + nameUser + "\';";
                }
                #endregion
                SqlCommand commandFromExecuteTableDB = new SqlCommand(queryActivateDataUser, connect);
                SqlDataReader reader = commandFromExecuteTableDB.ExecuteReader();
                #region read data
                while (reader.Read())
                {
                    LoginText.Text = reader["Login"].ToString();
                    if (roleUser == "Students")
                    id_group = (int)reader["ID_Group"];
                    IDText.Text = reader["ID"].ToString();
                    SecondNameText.Text = reader["Second_Name"].ToString();
                    NameText.Text = reader["Name"].ToString();
                    ThirdNameText.Text = reader["Third_Name"].ToString();
                }
                #endregion
                reader.Close();
                GroupText.Text = GetGroupName(id_group);
                if (getPositionGroup() == -1)
                {
                    TestCompleteCount1.Text = "0";
                }
                else
                {
                    TestCompleteCount1.Text = getPositionGroup().ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int getPositionGroup()
        {
            string query = @"select Student1.Login, 
Log_Complete_Test.percentComplete,
Groups.Name as GroupName
from Student1, Log_Complete_Test, Groups
where Student1.ID_Group = Groups.ID and
Log_Complete_Test.ID_Student = Student1.ID and
Groups.Name = '" + GroupText.Text + "\' order by Log_Complete_Test.percentComplete";

            SqlCommand command = new SqlCommand(query, connect);
            SqlDataReader reader = command.ExecuteReader();
            int i = 1;
            while (reader.Read())
            {
                if (LoginText.Text == reader["Login"].ToString())
                {
                    reader.Close();
                    return i;
                }
                else
                {
                    i++;
                }
            }
            reader.Close();
            return -1;
        }

        private string GetGroupName(int p)
        {
            string thisGroupName = "";
            SqlCommand command = new SqlCommand("select Name from Groups where ID = " + p + "", connect);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                thisGroupName = reader["Name"].ToString();
            }
            reader.Close();
            return thisGroupName;
        }
        #endregion
        #region initialize role connect
        private string validateRole()
        {
            
            SqlCommand command = new SqlCommand("sp_helpuser", connect);
            SqlDataReader readerRoleUser = command.ExecuteReader();
            while (readerRoleUser.Read())
            {
                if (nameUser == readerRoleUser["UserName"].ToString())
                {
                    string thisRole = readerRoleUser["RoleName"].ToString();
                    readerRoleUser.Close();
                    return thisRole;
                   
                }
            }
            readerRoleUser.Close();
            return null;
        }
        #endregion
        #region events menu
        //exiting
        private void Window_Closed(object sender, EventArgs e)
        {
            connect.Close();
            this.Hide();
            Application.Current.Shutdown();
            Environment.Exit(0);
        }
        //main
        private void dockPanel_Copy_MouseUp(object sender, MouseButtonEventArgs e)
        {
            #region switching grids
            TopBar.Children.Clear();
            MainGrid.Visibility = Visibility.Visible;
            TestsGrid.Visibility = Visibility.Hidden;
            StatisticsGrid.Visibility = Visibility.Hidden;
            UpdateData();
            #endregion
        }
        //statistics
        private void dockPanel_Copy2_MouseUp(object sender, MouseButtonEventArgs e)
        { 
            #region switching grids
            TopBar.Children.Clear();
            StatisticsGrid.Visibility = Visibility.Visible;
            MainGrid.Visibility = Visibility.Hidden;
            TestsGrid.Visibility = Visibility.Hidden;
            #endregion
            try
            {
                UpdateData();
                string query = "select Count(*) as AllRightTest from Log_Complete_Test where ID_Student = " + Convert.ToInt32(IDText.Text) + " and Complete = 'да'";
                SqlCommand command = new SqlCommand(query, connect);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    RightTestsText.Text = reader["AllRightTest"].ToString();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private  void UpdateData()
        {
           
                int percentComplete;
                UserText.Text = nameUser;
                string query = "select COUNT(*) as AllTests, AVG(Log_Complete_Test.percentComplete) as AVGTestComplete from Student1, Log_Complete_Test, Test where Log_Complete_Test.ID_Student=Student1.ID and Log_Complete_Test.ID_Test=Test.ID and Student1.Login=\'" + nameUser + "\'";
                SqlCommand command2 = new SqlCommand(query, connect);
                SqlDataReader reader1 =  command2.ExecuteReader();
                while (reader1.Read())
                {
                try
                {
                    allTests.Text = reader1["AllTests"].ToString();
                    TestCompleteCount.Text = allTests.Text;
                    percentComplete = (int)reader1["AVGTestComplete"];
                    ValueN.Text = (percentComplete / 10).ToString();
                    perComplete.Text = percentComplete.ToString() + " %";
                } catch(InvalidCastException)
                {
                    TestCompleteCount.Text = "0";
                    ValueN.Text = "0";
                }

                };
                reader1.Close();

                if (getPositionGroup() == -1)
                {
                    PositionText.Text = "Без места";
                    TestCompleteCount1.Text = "0";

                }
                else
                {
                    PositionText.Text = getPositionGroup().ToString();
                    TestCompleteCount1.Text = getPositionGroup().ToString();
                }
            
        }

        //tests
        private void dockPanel_Copy1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            #region clear panels
            ClearPanels();
            #endregion
            #region switching grids
            StatisticsGrid.Visibility = Visibility.Hidden;
            MainGrid.Visibility = Visibility.Hidden;
            TestsGrid.Visibility = Visibility.Visible;
            #endregion
            #region initialize and execute command to subject
            GetDataSubject();
            #region generate buttons to Add and deleting test if connect Test Autor
            if (roleUser == "TestAdmin")
            {
                generateControrsToTopBar();
            }
            #endregion
            #endregion
        }

        private void generateControrsToTopBar()
        {
            TopBar.Children.Add(CreateMenuItemCommandBar("AddTest", 200, "Добавить тест", 30));
            TopBar.Children[0].MouseUp += UserWindow_MouseUp;

            TopBar.Children.Add(CreateMenuItemCommandBar("DelTest", 200, "Удалить тест", 10));
            TopBar.Children[1].MouseUp += UserWindow_Del_MouseUp;
        }

        private void ClearPanels()
        {
            TopBar.Children.Clear();
            ObjectsToCurrentSubjectStackPanel.Children.Clear();
            SubjectStackPanel.Children.Clear();
        }

        private void GetDataSubject()
        {
            try
            {
                SqlCommand commandSubject = new SqlCommand("select Subject from Test group by Subject", connect);
                SqlDataReader readerSubject = commandSubject.ExecuteReader();
                while (readerSubject.Read())
                {
                    DockPanel Subject = GenerateSubjectItem(readerSubject["Subject"].ToString());
                    SubjectStackPanel.Children.Add(Subject);
                }
                readerSubject.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        #endregion
        #region generate controls
        private DockPanel CreateMenuItemCommandBar(string name, int width, string ContextItem, int MarginLeft)
        {
            DockPanel MenuItemCommandBar = new DockPanel();
            MenuItemCommandBar.Width = width;
            MenuItemCommandBar.Height = TopBar.Height - 13;
            MenuItemCommandBar.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            MenuItemCommandBar.Background = new SolidColorBrush(Color.FromArgb(255, 48, 50, 57));
            DropShadowEffect DSE = new DropShadowEffect();
            DSE.BlurRadius = 6;
            DSE.ShadowDepth = 2;
            DSE.Direction = 360;
            MenuItemCommandBar.Effect = DSE;
            MenuItemCommandBar.Name = name;
            MenuItemCommandBar.Margin = new Thickness(MarginLeft, 0, 10, 0);
            MenuItemCommandBar.MouseEnter += MenuItemCommandBar_MouseEnter;
            MenuItemCommandBar.MouseLeave += MenuItemCommandBar_MouseLeave;
            TextBlock Content = new TextBlock();
            Content.Text = ContextItem;
            Content.FontFamily = new System.Windows.Media.FontFamily("Segoe UI Light");
            Content.FontSize = 24;
            Content.Foreground = Brushes.White;
            Content.Padding = new Thickness(10, 0, 0, 0);
            Content.TextAlignment = TextAlignment.Center;
            Content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Content.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            MenuItemCommandBar.Children.Add(Content);
            return MenuItemCommandBar;
        }
        private DockPanel GenerateSubjectItem(string SubjectText)
        {
            DockPanel subject = new DockPanel();
            subject.Height = 80;
            subject.Margin = new Thickness(0, 3, 0, 0);
            subject.Background = new SolidColorBrush(Color.FromArgb(255, 83, 84, 90));
            DropShadowEffect DSE = new DropShadowEffect();
            DSE.Direction = 270;
            DSE.ShadowDepth = 1;
            subject.Effect = DSE;
            TextBlock SubjectName = new TextBlock();
            SubjectName.Height = 80;
            SubjectName.TextWrapping = TextWrapping.Wrap;
            SubjectName.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            SubjectName.Width = 468;
            SubjectName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            SubjectName.FontSize = 26;
            SubjectName.Foreground = new SolidColorBrush(Colors.White);
            SubjectName.Margin = new Thickness(0, 0, 0, 0);
            SubjectName.Padding = new Thickness(20, 20, 0, 0);
            DropShadowEffect TextDSE = new DropShadowEffect();
            TextDSE.BlurRadius = 13;
            TextDSE.ShadowDepth = 6;
            TextDSE.RenderingBias = RenderingBias.Quality;
            SubjectName.Effect = TextDSE;
            SubjectName.Text = SubjectText;
            subject.Children.Add(SubjectName);

            subject.MouseEnter += subject_MouseEnter;
            subject.MouseLeave += subject_MouseLeave;
            subject.MouseUp += (sender, e) =>
            {
                try
                {
                    ObjectsToCurrentSubjectStackPanel.Children.Clear();
                    SqlCommand command = new SqlCommand("select Test.Test_Name, TestAutor.Login from Test, TestAutor where Test.ID_TestAutor=TestAutor.ID and Subject = \'" + SubjectName.Text + "\';", connect);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        StackPanel TestItem = generateTestItem(reader["Test_Name"].ToString(),"Автор: " + reader["Login"].ToString(), ObjectsToCurrentSubjectStackPanel.Width);
                        ObjectsToCurrentSubjectStackPanel.Children.Add(TestItem);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
       
            };
            return subject;
        }
        private StackPanel generateTestItem(string TestName, string Autor, double _width)
        {
            StackPanel subject = new StackPanel();
            subject.Height = 80;
            subject.Margin = new Thickness(0, 3, 0, 0);
            subject.Width = _width;
            subject.Background = new SolidColorBrush(Color.FromArgb(255, 83, 84, 90));
            subject.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DropShadowEffect DSE = new DropShadowEffect();
            DSE.Direction = 270;
            DSE.ShadowDepth = 1;
            subject.Effect = DSE;

            TextBlock SubjectName = new TextBlock();
            SubjectName.Height = 40;
            SubjectName.TextWrapping = TextWrapping.Wrap;
            SubjectName.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            SubjectName.Width = 453;
            SubjectName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            SubjectName.FontSize = 26;
            SubjectName.Foreground = new SolidColorBrush(Colors.White);
            SubjectName.Margin = new Thickness(0, 0, 0, 0);
            SubjectName.Padding = new Thickness(20, 0, 0, 0);
            DropShadowEffect TextDSE = new DropShadowEffect();
            TextDSE.BlurRadius = 13;
            TextDSE.ShadowDepth = 6;
            TextDSE.RenderingBias = RenderingBias.Quality;
            SubjectName.Effect = TextDSE;
            SubjectName.Text = TestName;
            subject.Children.Add(SubjectName);

            subject.MouseUp += (sender, e) =>
            {
                if (roleUser == "Students")
                {
                    passingTest PT = new passingTest(TestName, connect, Convert.ToInt32(IDText.Text));
                    PT.ShowDialog();
                    UpdateData();
                }
                else
                {
                    MessageBox.Show("Только студенты могут выполнять тесты");
                }
            };
            TextBlock AutorName = new TextBlock();
            AutorName.Height = 40;
            AutorName.TextWrapping = TextWrapping.Wrap;
            AutorName.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            AutorName.Width = 453;
            AutorName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            AutorName.FontSize = 20;
            AutorName.Foreground = new SolidColorBrush(Colors.White);
            AutorName.Margin = new Thickness(0, 0, 0, 0);
            AutorName.Padding = new Thickness(30, 0, 0, 0);
            DropShadowEffect TextDSE1 = new DropShadowEffect();
            TextDSE.BlurRadius = 13;
            TextDSE.ShadowDepth = 6;
            TextDSE.RenderingBias = RenderingBias.Quality;
            AutorName.Effect = TextDSE1;
            AutorName.Text = Autor;
            subject.Children.Add(AutorName);
            subject.MouseEnter += (sender, e) => 
            { 
            subject.Background = new SolidColorBrush(Color.FromArgb(255, 103, 104, 110));
            };
            subject.MouseLeave += (sender, e) =>
            {
                subject.Background = new SolidColorBrush(Color.FromArgb(255, 83, 84, 90));
            };

            return subject;
        }
        #endregion
        #region events controls
        void UserWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddTest AT = new AddTest(connect, Convert.ToInt32(IDText.Text));
            AT.ShowDialog();
            ClearPanels();
            generateControrsToTopBar();
            GetDataSubject();
        }
        private void UserWindow_Del_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DelTest thisDelTest = new DelTest(connect);
            thisDelTest.ShowDialog();
            ClearPanels();
            generateControrsToTopBar();
            GetDataSubject();
        }
        void MenuItemCommandBar_MouseLeave(object sender, MouseEventArgs e)
        {
            DockPanel CommandItem = sender as DockPanel;
            CommandItem.Background = new SolidColorBrush(Color.FromArgb(255, 48, 50, 57));
        }
        void MenuItemCommandBar_MouseEnter(object sender, MouseEventArgs e)
        {
            DockPanel CommandItem = sender as DockPanel;
            CommandItem.Background = new SolidColorBrush(Color.FromArgb(255, 55, 57, 64));
        }
        void subject_MouseLeave(object sender, MouseEventArgs e)
        {
            DockPanel CommandItem = sender as DockPanel; 
            CommandItem.Background = new SolidColorBrush(Color.FromArgb(255, 83, 84, 90));
        }
        void subject_MouseEnter(object sender, MouseEventArgs e)
        {
            DockPanel CommandItem = sender as DockPanel;
            CommandItem.Background = new SolidColorBrush(Color.FromArgb(255, 103, 104, 110));
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
            UpdateLastTest();
        }

        private  void UpdateLastTest()
        {
            LastTests.Children.Clear();
            SqlCommand command1 = new SqlCommand("select Subject, Test_Name from Test", connect);
            SqlDataReader reader = command1.ExecuteReader();
            while(reader.Read())
            {
                LastTests.Children.Add(generateTestItem(reader["Test_Name"].ToString(), "Предмет: " + reader["Subject"].ToString(), LastTests.Width));
            }
            reader.Close();
        }

        private void dockPanel_Copy4_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}