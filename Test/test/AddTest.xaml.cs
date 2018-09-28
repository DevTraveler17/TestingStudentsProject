using Microsoft.Win32;
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
using System.Xml;
using System.Xml.Linq;
using System.Data.Odbc;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace test
{
    /// <summary>
    /// Логика взаимодействия для AddTest.xaml
    /// </summary>
    public partial class AddTest : Window
    {
        SqlConnection localsqlConnection;
        string fileName;
        int ID_testAutor;
        int CountQuestions = 0;
        int indexQuestion = 0;
        int startpositionMarging = 116;
        int maxVariables = 5;
        public AddTest(SqlConnection local, int ID_testAdmin)
        {
            InitializeComponent();
            localsqlConnection = local;
            ID_testAutor = ID_testAdmin;
        }

        Grid AddQuestionGenerateSector()
        {
            int CountLocalAnswers                   = 0;
            Grid Sector                             = new Grid();
            Sector.Name                             = "sector_" + indexQuestion;
            Sector.Width                            = ExplorerText.Width;
            Sector.Height                           = 220;
        #region generate TextBlock
            TextBlock contentTextSector             = new TextBlock();
            contentTextSector.Text                  = "Содержание вопроса " + (indexQuestion + 1);
            contentTextSector.Foreground            = Brushes.White;
            contentTextSector.FontSize              = 16;
            contentTextSector.Margin                = new Thickness(10, 10, 0, 0);
            contentTextSector.HorizontalAlignment   = System.Windows.HorizontalAlignment.Left;
        #endregion
            //0 element
            Sector.Children.Add(contentTextSector);
        #region generate delete button

            Button delThisQuestion                  = new Button();
            delThisQuestion.Width                   = 20;
            delThisQuestion.Height                  = 20;
            delThisQuestion.Content                 = "X";
            delThisQuestion.ToolTip                 = "Удалить вопрос";
            delThisQuestion.Background              = new SolidColorBrush(Color.FromArgb(255, 245, 128, 128));
            delThisQuestion.HorizontalAlignment     = System.Windows.HorizontalAlignment.Right;
            delThisQuestion.VerticalAlignment       = System.Windows.VerticalAlignment.Top;
            delThisQuestion.Margin                  = new Thickness(0, 10, 10, 0);
            #region event delete this grid
            delThisQuestion.Click += (sender, e) =>
            {
                ExplorerText.Children.RemoveAt(ExplorerText.Children.IndexOf(Sector));
                CountQuestions--;
                ExplorerText.Height = 230 * CountQuestions;
            };
            #endregion
        #endregion
            //1 element
            Sector.Children.Add(delThisQuestion);
        #region generate TextBox
            TextBox ContextText                     = new TextBox();
            ContextText.HorizontalAlignment         = System.Windows.HorizontalAlignment.Left;
            ContextText.Height                      = 50;
            ContextText.Margin                      = new Thickness(9, 36, 0, 0);
            ContextText.TextWrapping                = TextWrapping.Wrap;
            ContextText.Text                        = "";
            ContextText.VerticalAlignment           = System.Windows.VerticalAlignment.Top;
            ContextText.Width                       = 377;
        #endregion
            //2 element
            Sector.Children.Add(ContextText);
        #region generate buttons
            Button Add                              = new Button();
            Add.Content                             = "Добавить вариант";
            Add.HorizontalAlignment                 = System.Windows.HorizontalAlignment.Left;
            Add.VerticalAlignment                   = System.Windows.VerticalAlignment.Top;
            Add.Margin                              = new Thickness(10, 91, 0, 0);
            Add.Width                               = 186;
        #region event add variable
            Add.Click += (sender, e) =>
            {
                if (CountLocalAnswers < maxVariables)
                {
                    #region validate exceptions
                    if (Sector.Children.Count != 0)
                    {
                        if (Sector.Children[Sector.Children.Count-1] is TextBox)
                        {
                            TextBox thisTB = Sector.Children[Sector.Children.Count-1] as TextBox;
                            if (thisTB.Text != "")
                            {
                                Sector.Children.RemoveAt(Sector.Children.Count-1);
                            }
                            else
                            {
                                MessageBox.Show("Вариант ответа не задан");
                                Sector.Children.RemoveAt(Sector.Children.Count - 1);
                                Sector.Children.RemoveAt(Sector.Children.Count - 1);
                                CountLocalAnswers--;
                            }
                        }
                    }
                    #endregion
                    RadioButton Answer              = new RadioButton();
                    Answer.Content                  = "";
                    Answer.HorizontalAlignment      = System.Windows.HorizontalAlignment.Left;
                    Answer.Margin                   = new Thickness(10, startpositionMarging + (20 * CountLocalAnswers), 0, 0);
                    Answer.Foreground               = Brushes.White;
                    Sector.Children.Add(Answer);
                    CountLocalAnswers++;
                    TextBox contextInsert = new TextBox();
                    contextInsert.Text = "";
                    contextInsert.Height = 20;
                    contextInsert.Width = 130;
                    contextInsert.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    contextInsert.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    contextInsert.Margin = new Thickness(30, startpositionMarging + ((20 * CountLocalAnswers)-20), 0, 0);
                    contextInsert.LostFocus += (sender1, e1) => {
                        Answer.Content = contextInsert.Text;
                        Sector.Children.RemoveAt(Sector.Children.IndexOf(contextInsert));
                    };
                    Sector.Children.Add(contextInsert);
                }
                else
                {
                    MessageBox.Show("Достигнуто максимальное количество вариантов ответа");
                }
            };
        #endregion
            //3 eleent
            Sector.Children.Add(Add);

            Button del                              = new Button();
            del.Content                             = "Удалить вариант";
            del.HorizontalAlignment                 = System.Windows.HorizontalAlignment.Left;
            del.VerticalAlignment                   = System.Windows.VerticalAlignment.Top;
            del.Margin                              = new Thickness(200, 91, 0, 0);
            del.Width                               = 186;
        #region event delete variable
            del.Click += (sender, e) =>
            {
                if (Sector.Children.Count > Sector.Children.IndexOf(del)+1)
                {
                    #region validate exceptions
                    if (Sector.Children[Sector.Children.Count - 1] is TextBox)
                    {
                        TextBox thisTB = Sector.Children[Sector.Children.Count - 1] as TextBox;
                        if (thisTB.Text == "")
                        {
                            Sector.Children.RemoveAt(Sector.Children.Count - 1);
                        }
                    }
                    #endregion
                    Sector.Children.RemoveAt(Sector.Children.Count-1);
                    CountLocalAnswers--;
                }
                else
                {
                    MessageBox.Show("Нет вариантов ответа");
                }
            };
            #endregion
            Sector.Children.Add(del);
            #endregion
            CountQuestions++;
            indexQuestion++;
            return Sector;
        }


        

        private void Add_question_button_Click(object sender, RoutedEventArgs e)
        {
            Grid newGridToExplorer = AddQuestionGenerateSector();
            ExplorerText.Children.Add(newGridToExplorer);
            ExplorerText.Height = 220 * CountQuestions;
            scrollViewer.ScrollToEnd();
        }

        private void Generate_button_Click(object sender, RoutedEventArgs e)
        {
            List<XElement> Questions = new List<XElement>();
            List<RadioButton> Answers;
            XDocument newXDoc = new XDocument();
            XElement Main = new XElement("Main");
            newXDoc.Add(Main);
            for (int i = 0; i < ExplorerText.Children.Count; i++)
            {
                Questions.Add(new XElement("Question"));

                  
                    XElement content = new XElement("Content", generateContent(i));
                    Questions[i].Add(content);

                    Answers = generateAnswers(i);
                    for (int z = 0; z < Answers.Count; z++)
                    {
                        if (Answers[z].IsChecked == true)
                        {
                            XElement RightAnswer = new XElement("answer", Answers[z].Content);
                            XAttribute AttRight = new XAttribute("right", "yes");
                            RightAnswer.Add(AttRight);
                            Questions[i].Add(RightAnswer);
                        }
                        else
                        {
                            XElement Answer = new XElement("answer", Answers[z].Content);
                        XAttribute AttRight = new XAttribute("right", "no");
                        Answer.Add(AttRight);
                        Questions[i].Add(Answer);
                        }
                    }
            }
            Main.Add(Questions);
            SaveFileDialog SFD = new SaveFileDialog();
            if (SFD.ShowDialog() == true)
            {
                SFD.DefaultExt = "*.xml";
                fileName = SFD.FileName;
                newXDoc.Save(fileName);
                string query = "INSERT INTO Test ([Subject],[Test_Name],[LINK],[ID_TestAutor]) values(\'" + SubjectText.Text + "\', \'" + TestNameText.Text + "\',\'" + fileName + "\'," + ID_testAutor + ")";
                SqlCommand command = new SqlCommand(query, localsqlConnection);
                command.ExecuteNonQuery();
                command.Dispose();
                MessageBox.Show("Файл сохранен");
                AddTestWindow_Closed(sender, e);
            }
            
           
        }

        private List<RadioButton> generateAnswers(int index)
        {
            List<RadioButton> Answers = new List<RadioButton>();
            Grid thisGrid = ExplorerText.Children[index] as Grid;
            for (int i = 0; i < thisGrid.Children.Count; i++)
            {
                if (thisGrid.Children[i] is RadioButton)
                {
                    RadioButton thisRadio = thisGrid.Children[i] as RadioButton;
                    Answers.Add(thisRadio);
                }
            }
            return Answers;
        }
        private string generateContent(int index)
        {
            Grid thisGrid = ExplorerText.Children[index] as Grid;
            TextBox thisTextBox = thisGrid.Children[2] as TextBox;
            return thisTextBox.Text;
        }
        private void AddTestWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
