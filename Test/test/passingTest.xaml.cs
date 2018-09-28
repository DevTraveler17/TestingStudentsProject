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
using System.Windows.Shapes;
using System.Data.Odbc;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace test
{
	/// <summary>
	/// Логика взаимодействия для passingTest.xaml
	/// </summary>
    /// 

    public class Answers
    {
        public string content;
        public bool right;

        
    }
    public class Question
    {
        public string content;
        public List<Answers>answer;

        public Question()
        {
            answer = new List<Answers>();
        }
    }
	public partial class passingTest : Window
	{
        int RightQuestions;

        string Theme;
        SqlConnection local;
        string FileName;
        int id;
        int IdTest;
        double isPercentComplete;
        List<Question> Questions = new List<Question>();
        public passingTest(string _theme, SqlConnection _local, int id_student)
		{
			this.InitializeComponent();
            Theme = _theme;
            local = _local;
            id = id_student;
            
			// Вставьте ниже код, необходимый для создания объекта.
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RezultGrid.Visibility = System.Windows.Visibility.Hidden;
            try
            {
                ThemeText.Text = Theme;
                List<XElement> elements = new List<XElement>();
                List<XAttribute> att = new List<XAttribute>();
                SqlCommand command = new SqlCommand("select * from Test where Test_Name=\'" + Theme + "\'", local);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    IdTest = (int)reader["ID"];
                    FileName = reader["LINK"].ToString();
                }
                reader.Close();
                XDocument thisTest = XDocument.Load(FileName);
                foreach (XElement question in thisTest.Root.Elements("Question"))
                {

                    Question thisQuestion = new Question();
                    XElement content = question.Element("Content");
                    thisQuestion.content = content.Value;


                    XNode node = question.FirstNode;
                    node = node.NextNode;
                    while (node != null)
                    {
                        if (node.NodeType == XmlNodeType.Element)
                        {
                            Answers answer = new Answers();
                            XElement element = (XElement)node;
                            answer.content = element.Value;
                            if (element.Attribute("right").Value == "yes")
                            {
                                answer.right = true;
                            }
                            else
                            {
                                answer.right = false;
                            }
                            thisQuestion.answer.Add(answer);
                           node = node.NextNode;
                        }
                    }
                    Questions.Add(thisQuestion);
                    
                }

                for (int i = 0; i < Questions.Count; i++)
                {
                    Grid AddedQuestion = generateQuestionBlock(Questions[i].content, Questions[i].answer, i+1);
                   LayoutRoot.Children.Add(AddedQuestion);
                    LayoutRoot.Height += AddedQuestion.Height;
                    
                }
                LayoutRoot.Height += 40;
                LayoutRoot.Children.Add(generateButtonToRezult());
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private Button generateButtonToRezult()
        {
            Button thisButton = new Button();
            thisButton.Content = "Закончить и проверить";
            thisButton.Height = 30;
            thisButton.Width = 200;
            thisButton.Margin = new Thickness(0, 20, 0, 0);
            thisButton.Click += (sender, e) => {
                MainTestGrid.Visibility = System.Windows.Visibility.Hidden;
                RezultGrid.Visibility = System.Windows.Visibility.Visible;
                AllQuestionsText.Text = Questions.Count().ToString();
                RightQuestionsText.Text = RightQuestions.ToString();
                isPercentComplete = ((Convert.ToDouble(RightQuestions) / Convert.ToDouble(Questions.Count())) * 100);
                PercentText.Text = isPercentComplete.ToString() + " %";
                if (isPercentComplete > 70)
                {
                    IsCopmleteText.Text = "да";
                }
                else
                {
                    IsCopmleteText.Text = "нет";
                }
                 
            };
            return thisButton;
        }

        private Grid generateQuestionBlock(string ContentQuestion, List<Answers> _answer, int index)
        {
            int startPosition = 65;
            Grid questionGrid                       = new Grid();
            questionGrid.HorizontalAlignment        = HorizontalAlignment.Left;
            questionGrid.Height                     = 130;
            questionGrid.Margin                     = new Thickness(0, 0, 0, 0);
            questionGrid.VerticalAlignment          = VerticalAlignment.Top;
            questionGrid.Width                      = 490;

            TextBlock rec = new TextBlock();
            rec.HorizontalAlignment = HorizontalAlignment.Left;
            rec.VerticalAlignment = VerticalAlignment.Top;
            rec.Foreground = new SolidColorBrush(Colors.White);
            rec.FontSize = 14;
            rec.Margin = new Thickness(10, 20, 0, 0);
            rec.Text = "Вопрос " + index;
            questionGrid.Children.Add(rec);

            TextBlock contentTextBlock              = new TextBlock();
            contentTextBlock.HorizontalAlignment    = HorizontalAlignment.Left;
            contentTextBlock.VerticalAlignment      = VerticalAlignment.Top;
            contentTextBlock.Foreground             = new SolidColorBrush(Colors.White);
            contentTextBlock.FontSize               = 13;
            contentTextBlock.Margin                 = new Thickness(20, 40, 0, 0);
            contentTextBlock.Text                   = ContentQuestion;
            if (ContentQuestion.Length > 55)
            {
                questionGrid.Height += 20;
                startPosition += 20;
            }
            contentTextBlock.TextWrapping           = TextWrapping.Wrap;
            questionGrid.Children.Add(contentTextBlock);

            List<Answers> thisAnswers               = _answer;
            

            for(int i = 0;i < thisAnswers.Count;i++)
            {
                RadioButton oneasAnswer             = new RadioButton();
                oneasAnswer.Content                 = thisAnswers[i].content;
                oneasAnswer.HorizontalAlignment     = HorizontalAlignment.Left;
                oneasAnswer.Margin                  = new Thickness(34.125, startPosition, 0, 0);
                oneasAnswer.VerticalAlignment       = VerticalAlignment.Top;
                oneasAnswer.Foreground              = new SolidColorBrush(Colors.White);
                oneasAnswer.Checked                 += (sender, e) =>
                    {
                        if(thisAnswers[(questionGrid.Children.IndexOf((RadioButton)sender))-2].right == true)
                            {
                                RightQuestions++;
                                questionGrid.IsEnabled = false;
                                questionGrid.Opacity = 0.5;                           
                            } else
                            {
                                questionGrid.IsEnabled = false;
                                questionGrid.Opacity = 0.5;
                        }
                    };
                questionGrid.Children.Add(oneasAnswer);
                startPosition += 15;
                
            }
            Border border = new Border();
            border.Width = questionGrid.Width;
            border.Height = 2;
            border.HorizontalAlignment = HorizontalAlignment.Left;
            border.VerticalAlignment = VerticalAlignment.Bottom;
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = new SolidColorBrush(Colors.Gray);
            border.Margin = new Thickness(0, 0, 0, -10);
            questionGrid.Children.Add(border);
            return questionGrid;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO Log_Complete_Test (ID_Student ,ID_Test ,percentComplete ,Right_questions ,Complete) VALUES (" + id + " ," + IdTest + ", "+ Convert.ToInt32(isPercentComplete) +" ," + Convert.ToInt32(RightQuestions) + ", \'" + IsCopmleteText.Text + "\')";
                SqlCommand command = new SqlCommand(query, local);
                command.ExecuteNonQuery();
                MessageBox.Show("Данные отправлены");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}