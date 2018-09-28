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
using System.Windows.Documents;
using System.Data;

namespace test
{
	/// <summary>
	/// Логика взаимодействия для PrintWindow.xaml
	/// </summary>
	public partial class PrintWindow : Window
	{
        FlowDocument flow;
        List<string> Cols = new List<string>();
        Table table;
		public PrintWindow(DataGrid grid)
		{
			this.InitializeComponent();
            flow = new System.Windows.Documents.FlowDocument();
            flow.Background = new SolidColorBrush(Color.FromRgb(33, 34, 40));

            table = new System.Windows.Documents.Table();
            
           

            

            table.BorderThickness = new Thickness(1);
            table.BorderBrush = new SolidColorBrush(Colors.White);
            table.Foreground = new SolidColorBrush(Colors.White);
            TableRowGroup trg = new System.Windows.Documents.TableRowGroup();
            

            for (int i = 0; i < grid.Columns.Count; i++)
            {
                table.Columns.Add(new TableColumn());
                
                table.Columns[i].Width = new GridLength(MainFlowDocument1.Width / grid.Columns.Count - 10);
                Cols.Add(grid.Columns[i].Header.ToString());
            }
            if (grid.ItemsSource is List<User>)
            {
                GenerateTextToList("Список пользователей");
                List<User> userList = grid.ItemsSource as List<User>;
                generateColumns(trg);
                foreach (User user in userList)
                {
                    TableRow row = new System.Windows.Documents.TableRow();
                    row.Background = new SolidColorBrush(Color.FromArgb(255, 63, 64, 70));
                    addCell(user.Id.ToString(), row);
                    addCell(user.Login.ToString(), row);
                    addCell(user.Name.ToString(), row);
                    addCell(user.SecondName.ToString(), row);
                    addCell(user.ThirdName.ToString(), row);
                    addCell(user.Role.ToString(), row);
                    trg.Rows.Add(row);
                }
                table.RowGroups.Add(trg);
                GetCount(trg);
            }
            else if (grid.ItemsSource is List<Tests>)
            {
                GenerateTextToList("Cписок Тестов");
                List<Tests> tests = grid.ItemsSource as List<Tests>;
                generateColumns(trg);
                foreach (Tests test in tests)
                {
                    TableRow row = new System.Windows.Documents.TableRow();
                    row.Background = new SolidColorBrush(Color.FromArgb(255, 63, 64, 70));
                    addCell(test.Id.ToString(), row);
                    addCell(test.Subject.ToString(), row);
                    addCell(test.TestName.ToString(), row);
                    addCell(test.Link.ToString(), row);
                    trg.Rows.Add(row);
                }
                table.RowGroups.Add(trg);
                GetCount(trg);
            }
            else if (grid.ItemsSource is List<LogCompleteTest>)
            {
                GenerateTextToList("Cписок Тестов");
                List<LogCompleteTest> logs = grid.ItemsSource as List<LogCompleteTest>;
                generateColumns(trg);
                foreach (LogCompleteTest log in logs)
                {
                    TableRow row = new System.Windows.Documents.TableRow();
                    row.Background = new SolidColorBrush(Color.FromArgb(255, 63, 64, 70));
                    addCell(log.Id.ToString(), row);
                    addCell(log.Right.ToString(), row);
                    addCell(log.Percent.ToString() + " %", row);
                    addCell(log.Login.ToString(), row);
                    addCell(log.Test.ToString(), row);
                    addCell(log.IsComplete.ToString(), row);
                    trg.Rows.Add(row);
                }
                table.RowGroups.Add(trg);
                GetCount(trg);

            }

            
            flow.Blocks.Add(table);
            flow.ColumnWidth = MainFlowDocument1.Width;
            
            MainFlowDocument1.Document = flow;
            
			// Вставьте ниже код, необходимый для создания объекта.
		}

        private void addCell(string p, System.Windows.Documents.TableRow row)
        {
            row.Cells.Add(getDataToClass(p));
        }

        private void GenerateTextToList(string text)
        {
            Run thisRun = new System.Windows.Documents.Run();
            thisRun.Text = text;
            Paragraph par = new System.Windows.Documents.Paragraph();
            par.Padding = new Thickness(10, 0, 0, 0);
            par.Foreground = new SolidColorBrush(Colors.White);
            par.FontSize = 20;
            par.FontFamily = new System.Windows.Media.FontFamily("Segoe UI Light");
            par.Inlines.Add(thisRun.Text);
            flow.Blocks.Add(par);
        }

        private void GetCount(System.Windows.Documents.TableRowGroup trg)
        {
            TableRow row = new System.Windows.Documents.TableRow();
            row.Background = new SolidColorBrush(Color.FromArgb(255, 83, 84, 90));
            row.Cells.Add(getDataToClass("Всего:"));
            row.Cells.Add(getDataToClass((trg.Rows.Count - 1).ToString()));
            trg.Rows.Add(row);
        }

        private void generateColumns(TableRowGroup _trg)
        {
            TableRow row = new System.Windows.Documents.TableRow();
            row.Background = new SolidColorBrush(Color.FromArgb(255, 83, 84, 90));
            foreach (string col in Cols)
            {
                row.Cells.Add(getDataToClass(col));
            }
            _trg.Rows.Add(row);
        }

        private TableCell getDataToClass(string p)
        {
            Run run = new System.Windows.Documents.Run();
            run.Text = p;
            TableCell tabCell = new System.Windows.Documents.TableCell();
            tabCell.Blocks.Add(new Paragraph(run));
            return tabCell;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                
                int margin = 5;
                Size pageSize = new Size(dialog.PrintableAreaHeight - margin * 2, dialog.PrintableAreaHeight - margin * 2);
                IDocumentPaginatorSource paginator = flow;
                paginator.DocumentPaginator.PageSize = pageSize;
                dialog.PrintDocument(paginator.DocumentPaginator, "Flow print"); 
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Height = 622;
            MainFlowDocument1.Width = this.Width - 10;
            flow.PageWidth = MainFlowDocument1.Width;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                table.Columns[i].Width = new GridLength(MainFlowDocument1.Width / table.Columns.Count - 10);
            }
            
        }
	}
}