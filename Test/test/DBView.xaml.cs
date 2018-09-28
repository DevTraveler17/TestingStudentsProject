using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Data.SqlClient;



namespace test
{
    /// <summary>
    /// Interaction logic for DBView.xaml
    /// </summary>
    /// 
   
    public partial class DBView_User : Window
    {
        SqlConnection start;
        bool isToTests = false;
        bool isToUsers = false;
        bool isToStatisctics = false;
        List<User> ListUsers    = new List<User>();
        List<Tests> ListTests   = new List<Tests>();
        List<LogCompleteTest> Logs = new List<LogCompleteTest>();
        List<Group> groups = new List<Group>();
       /// <summary>
       /// Форма панели админинстрирования
       /// </summary>
       /// <param name="admin">значение, указывающее, является ли пользователь Админинстратором</param>
       /// <param name="UserName">Имя пользователя</param>
       /// <param name="_Connect_string">Символьное выражение строки подключения</param>
       /// <param name="_password">пароль пользователя подключения</param>
        public DBView_User(bool admin, string UserName, string _Connect_string, string _password)
        {
            try
            {
                InitializeComponent();
                start                   = new SqlConnection(_Connect_string);
                start.Open();
                SignalBorder.Background = Brushes.Green;
                ListUser.MouseUp       += AddUserEvent;
                Statistics.MouseUp     += StatisticsEvent;
                ShowTest.MouseUp       += ShowTestEvent;
                ShowGroup.MouseUp      += ShowGroupEvent;
                Exit.MouseUp           += ExitEvent;
                Exit.MouseUp           += Window_Closed;
                this.MouseUp += (sender, e) =>
                {
                    
                        ActiveGridView.ColumnWidth = (int)((ActiveGridView.Width / 6) - 1.5);
                };
                showUsers();
                CreateButtonsTopBarForUsers();
                showCounts();
            }
            catch(Exception ex)
            {
                SignalBorder.Background = Brushes.Red;
                MessageBox.Show("Error \n" + ex.Message);
            }
        }

        private void DBView_User_MouseUp1(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CreateButtonsTopBarForUsers()
        {
            TopCommandBar.Children.RemoveRange(1, 2);
            TopCommandBar.Children.Add(CreateMenuItemCommandBar("AddUserButton", 230, "Добавить пользователя", 25));
            TopCommandBar.Children.Add(CreateMenuItemCommandBar("DeleteButton", 220, "Удалить пользователя", 5));
            TopCommandBar.Children[1].MouseUp += DBView_User_MouseUp;
            TopCommandBar.Children[2].MouseUp += DBView_User_MouseUp_Delete_button;
        }
        /// <summary>
        /// Отображение количества всех пользователей 
        /// </summary>
        private void showCounts()
        {
            Counts("Teacher", TeacherCountText);
            Counts("TestAutor", CreatorCountText);
            Counts("Student1", StudentCountText);
            SumCounts();
        }
        /// <summary>
        /// подсчет суммы количества пользователей
        /// </summary>
        private void SumCounts()
        {
            int Teachers = Convert.ToInt32(TeacherCountText.Text);
            int Students = Convert.ToInt32(StudentCountText.Text);
            int TestAutors = Convert.ToInt32(CreatorCountText.Text);
            int Sum = Teachers + Students + TestAutors;
            UsersCountText.Text = Sum.ToString();
        }
        /// <summary>
        /// Реализация запроса на получение количества  пользователей базы данных
        /// </summary>
        /// <param name="Table">Имя таблицы, в которой требуется подсчитать</param>
        /// <param name="target">В какой элемент типа UIElement требуется занести данные</param>
        private void Counts(string Table, TextBlock target)
        {
            SqlCommand command = new SqlCommand("select COUNT(*) from "+ Table, start);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                target.Text = reader[0].ToString();
            }
            reader.Close();
            
        }

        /// <summary>
        /// отображение всех пользователей в активном DataGrid
        /// </summary>
        private void showUsers()
        {            
            try
            {
                isToUsers = true;
                isToTests = false;
                isToStatisctics = false;
                CommandTeachers();
                CommandStudents();
                CommandTestAutors();
                ActiveGridView.ItemsSource = ListUsers;
                ActiveGridView.ColumnWidth = (int)((ActiveGridView.Width / 6) - 1.5);
                
                }
            catch (Exception ex)
            {
                MessageBox.Show("Error Query: \n" + ex);
            }

        }
        private void ShowTests()
        {
            isToTests = true;
            isToUsers = false;
            isToStatisctics = false;

            ListTests.Clear();
            ActiveGridView.ItemsSource = null;
            CommandTests();
            ActiveGridView.ItemsSource = ListTests;
        }
        private void showStatistics()
        {
            isToTests = false;
            isToUsers = false;
            isToStatisctics = true;
            Logs.Clear();
            ActiveGridView.ItemsSource = null;
            CommandStatistics();
            ActiveGridView.ItemsSource = Logs;
                
        }
        private void showGroups()
        {
            groups.Clear();
            ActiveGridView.ItemsSource = null;
            CommandGroups();
            ActiveGridView.ItemsSource = groups;
        }

        private void DBView_User_MouseUp_Delete_button(object sender, MouseButtonEventArgs e)
        {
            DeleteUser DU = new DeleteUser(start, ListUsers);
            DU.ShowDialog();
            RefreshActiveDataGrid();
        }

        void DBView_User_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddUserWindow AUW = new AddUserWindow(start);
            AUW.ShowDialog();
            RefreshActiveDataGrid();
        }

        private void RefreshActiveDataGrid()
        {
            ListUsers.Clear();
            ListTests.Clear();
            ActiveGridView.ItemsSource = null;
            showUsers();
            showCounts();
        }
        
        private DockPanel CreateMenuItemCommandBar(string name, int width, string ContextItem, int MarginLeft)
        {
            DockPanel MenuItemCommandBar            = new DockPanel(); 
            MenuItemCommandBar.Width                = width;
            MenuItemCommandBar.Height               = TopCommandBar.Height - SignalBorder.Height - 13;
            MenuItemCommandBar.HorizontalAlignment  = System.Windows.HorizontalAlignment.Left;
            MenuItemCommandBar.Background           = new SolidColorBrush(Color.FromArgb(255, 48, 50, 57));
            DropShadowEffect DSE                    = new DropShadowEffect();
            DSE.BlurRadius                          = 6;
            DSE.ShadowDepth                         = 2;
            DSE.Direction                           = 360;
            MenuItemCommandBar.Effect               = DSE;               
            MenuItemCommandBar.Name                 = name;
            MenuItemCommandBar.Margin               = new Thickness(MarginLeft,0,10,0);
            MenuItemCommandBar.MouseEnter           += MenuItemCommandBar_MouseEnter;
            MenuItemCommandBar.MouseLeave           += MenuItemCommandBar_MouseLeave;
            TextBlock Content                       = new TextBlock();
            Content.Text                            = ContextItem;
            Content.FontFamily                      = new System.Windows.Media.FontFamily("Segoe UI Light");
            Content.FontSize                        = 20;
            Content.Foreground                      = Brushes.White;
            Content.Padding                         = new Thickness(10, 0, 0, 0);
            Content.TextAlignment                   = TextAlignment.Center;
            Content.VerticalAlignment               = System.Windows.VerticalAlignment.Center;
            Content.HorizontalAlignment             = System.Windows.HorizontalAlignment.Left;
            MenuItemCommandBar.Children.Add(Content);
            return MenuItemCommandBar;
        }


        #region handle events topBarEffects
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
        #endregion
        /// <summary>
        /// запрос на получение данных из таблицы "Учителя" и занесение данных в активный dataGrid
        /// </summary>
        private void CommandTeachers()
        {
            string QueryTeachers            = "select * from Teacher";
            SqlCommand commandTeachers      = new SqlCommand(QueryTeachers, start);
            SqlDataReader readerTeachers    = commandTeachers.ExecuteReader();
            while (readerTeachers.Read())
            {
                Teacher newTeacher          = new Teacher();
                newTeacher.Id               = (int)readerTeachers["ID"];
                newTeacher.Login            = readerTeachers["Login"].ToString();
                newTeacher.Name             = readerTeachers["Name"].ToString();
                newTeacher.SecondName       = readerTeachers["Second_Name"].ToString();
                newTeacher.ThirdName        = readerTeachers["Third_Name"].ToString();
                newTeacher.Role             = "teacher";
                ListUsers.Add(newTeacher);
            }
            readerTeachers.Close();
        }
        /// <summary>
        /// запрос на получение данных из таблицы "Студенты" и занесение данных в активный dataGrid
        /// </summary>
        private void CommandStudents()
        {
            string QueryStudents = "select * from Student1";
            SqlCommand commandStudents = new SqlCommand(QueryStudents, start);
            SqlDataReader readerStudents = commandStudents.ExecuteReader();
            while (readerStudents.Read())
            {
                Students newStudent         = new Students();
                newStudent.Id               = (int)readerStudents["ID"];
                newStudent.Login            = readerStudents["Login"].ToString();
                newStudent.Name             = readerStudents["Name"].ToString();
                newStudent.SecondName       = readerStudents["Second_Name"].ToString();
                newStudent.ThirdName        = readerStudents["Third_Name"].ToString();
                //newStudent.Id_Group       = (int)readerStudents["ID_Group"];
                newStudent.Role             = "student";
                ListUsers.Add(newStudent);
            }
            readerStudents.Close();
        }
        /// <summary>
        /// запрос на получение данных из таблицы "Составители тестов" и занесение данных в активный dataGrid
        /// </summary>
        private void CommandTestAutors()
        {
            string QueryStudents = "select * from TestAutor";
            SqlCommand commandStudents = new SqlCommand(QueryStudents, start);
            SqlDataReader readerTestAutors = commandStudents.ExecuteReader();
            while (readerTestAutors.Read())
            {
                TestAutor newTestAutor      = new TestAutor();
                newTestAutor.Id             = (int)readerTestAutors["ID"];
                newTestAutor.Login          = readerTestAutors["Login"].ToString();
                newTestAutor.Name           = readerTestAutors["Name"].ToString();
                newTestAutor.SecondName     = readerTestAutors["Second_Name"].ToString();
                newTestAutor.ThirdName      = readerTestAutors["Third_Name"].ToString();
                //newStudent.Id_Group       = (int)readerStudents["ID_Group"];
                newTestAutor.Role           = "testAutor";
                ListUsers.Add(newTestAutor);
            }
            readerTestAutors.Close();
        }
        /// <summary>
        /// запрос на получение данных из таблицы "Тесты" и занесение данных в активный dataGrid
        /// </summary>
        private void CommandTests()
        {
            string QueryTest = "select * from Test";
            SqlCommand commandTest = new SqlCommand(QueryTest, start);
            SqlDataReader readerTestAutors = commandTest.ExecuteReader();
            while (readerTestAutors.Read())
            {
                Tests newTest = new Tests();
                newTest.Id = (int)readerTestAutors["ID"];
                newTest.Subject = readerTestAutors["Subject"].ToString();
                newTest.TestName = readerTestAutors["Test_Name"].ToString();
                newTest.Link = readerTestAutors["LINK"].ToString();
                ListTests.Add(newTest);
            }
            readerTestAutors.Close();
        }

        private void CommandGroups()
        {
            string QueryGroup = "select * from Groups";
            SqlCommand commandGroup = new SqlCommand(QueryGroup, start);
            SqlDataReader reader = commandGroup.ExecuteReader();
            while (reader.Read())
            {
                Group group = new Group();
                group.Id = (int)reader["ID"];
                group.NameGroup = reader["Name"].ToString();
                groups.Add(group);
            }
            reader.Close();
        }
        /// <summary>
        /// запрос на получение данных из таблицы "Статистика" и занесение данных в активный dataGrid
        /// </summary>
        private void CommandStatistics()
        {
            string Statistics = "select Log_Complete_Test.ID as ID, Student1.Login as Login, Test.Test_Name as Test, Log_Complete_Test.percentComplete as percentComplete, Log_Complete_Test.Right_questions as RightQuestions, Log_Complete_Test.Complete as Complete  from Log_Complete_Test, Student1, Test where Log_Complete_Test.ID_Student = Student1.ID and Log_Complete_Test.ID_Test = Test.ID";
            SqlCommand command = new SqlCommand(Statistics, start);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                LogCompleteTest newLog = new LogCompleteTest();
                newLog.Id = (int)reader["ID"];
                newLog.Login = reader["Login"].ToString();
                newLog.Test = reader["Test"].ToString();
                newLog.Percent = (int)reader["percentComplete"];
                newLog.Right = (int)reader["RightQuestions"];
                if(reader["Complete"].ToString() == "да")
                {
                    newLog.IsComplete = true;
                } else
                {
                    newLog.IsComplete = false;
                }
                Logs.Add(newLog);
            }
            reader.Close();
        }
        void ExitEvent(object sender, MouseButtonEventArgs e)
        {
            
        }
        void ShowGroupEvent(object sender, MouseButtonEventArgs e)
        {
            try
            {
                showGroups();
                CreateButtonsTopBarForGroups();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateButtonsTopBarForGroups()
        {
            TopCommandBar.Children.RemoveRange(1, 2);
            TopCommandBar.Children.Add(CreateMenuItemCommandBar("AddGroupButton", 230, "Добавить группу", 25));
            TopCommandBar.Children.Add(CreateMenuItemCommandBar("AddGroupButton", 230, "Удалить группу", 5));
            TopCommandBar.Children[1].MouseUp += (sender, e) =>
            {
                AddGroup AG = new AddGroup(start);
                AG.ShowDialog();
                showGroups();
            };
            TopCommandBar.Children[2].MouseUp += (sender, e) =>
            {
                DelGroup DG = new DelGroup(start);
                DG.ShowDialog();
                showGroups();
            };
        }
        void ShowTestEvent(object sender, MouseButtonEventArgs e)
        {
            try
            { 
                ShowTests();
                CreateButtonsTopBarForTests();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateButtonsTopBarForTests()
        {
            TopCommandBar.Children.RemoveRange(1, 2);
            TopCommandBar.Children.Add(CreateMenuItemCommandBar("AddTestButton", 230, "Добавить Тест", 25));
            TopCommandBar.Children.Add(CreateMenuItemCommandBar("DeleteTestButton", 220, "Удалить Тест", 5));
            TopCommandBar.Children[1].MouseUp += (sender, e) =>
            {
                AddTest AT = new AddTest(start, 0);
                AT.ShowDialog();
                ActiveGridView.ItemsSource = null;
                ShowTests();
            };
            TopCommandBar.Children[2].MouseUp += (sender, e) =>
            {
                DelTest DT = new DelTest(start);
                DT.ShowDialog();
                ActiveGridView.ItemsSource = null;
                ShowTests();
            };
        }
        
        void StatisticsEvent(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TopCommandBar.Children.RemoveRange(1, 2);
                showStatistics();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void AddUserEvent(object sender, MouseButtonEventArgs e)
        {
            RefreshActiveDataGrid();
            CreateButtonsTopBarForUsers();
            //MessageBox.Show("DockPanelCopy");
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Hide();
            start.Close();
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            PrintWindow PW = new PrintWindow(ActiveGridView);
            PW.ShowDialog();
        }


       
       
    }
}
