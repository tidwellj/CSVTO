using Microsoft.Win32;
using System.IO;
using Newtonsoft.Json; // Make sure to have Newtonsoft.Json installed via NuGet
using System.Collections.Generic;
using System.Windows;
using System.Dynamic;
using Newtonsoft.Json.Converters;
using System.Windows.Controls;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Windows.Media;
using System.Windows.Documents;
using System.Diagnostics.Eventing.Reader;

namespace csvto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        
    {
        private int x = 0;
        private string json;
        public MainWindow()
        {
            InitializeComponent();

            

            // LoadJsonData();




        }


        private void LoadJsonData()
        {
            // Example JSON string
            string jsonString = json;
        
        }

        private void DisplayJsonInDataGrid(string jsonString)
        {
            DataTable dataTable = JsonToDataTable(jsonString);
            

            gr_Copy.ItemsSource = dataTable.DefaultView;

        }

        private DataTable JsonToDataTable(string jsonString)
        {
            var dataTable = new DataTable();

            // Parse the JSON array of arrays
            JArray jsonArray = JArray.Parse(jsonString);

            // Assuming the first array contains the data for columns, if not, you need to adjust accordingly
            if (jsonArray.Count > 0)
            {
                int columnCount = jsonArray[0].Children().Count();
                for (int i = 0; i < columnCount; i++)
                {
                    // Generate column names dynamically (Column1, Column2, etc.)
                    dataTable.Columns.Add($"Column{i + 1}", typeof(string));
                }

                foreach (JArray rowArray in jsonArray)
                {
                    var row = dataTable.NewRow();
                    int columnIndex = 0;
                    foreach (var cell in rowArray)
                    {
                        row[columnIndex] = cell.ToString();
                        columnIndex++;
                    }
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        private void CSVButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var dataCSV = new DataTable();
                var datatabe = new DataTable();
                // Read the CSV file
                // Read the CSV file
                var csvLines = File.ReadAllLines(filePath);
                var csvData = new List<List<string>>();
                //var dataTable = new DataTable();

                // Skip the headers by starting with i = 1 if you don't want them included
                // if (JsonCheck.IsChecked == true)

                

                    var headers = csvLines[0].Split(',').Skip(1);
                    foreach (var header in headers)
                    {
                        dataCSV.Columns.Add(new DataColumn(header.Trim()));
                    }





                    for (int i = 0; i < csvLines.Length; i++) // Start from 1 if you want to skip headers in CSV
                    {


                        // Skip the first column for headers
                        var line = csvLines[i];
                        if (string.IsNullOrWhiteSpace(line)) // Skip empty lines
                            continue;


                        var rowValues = line.Split(',').Skip(1).Select(value => value.Trim()).ToArray(); // Skip the first column for each row as well
                        if (rowValues.Length == dataCSV.Columns.Count)
                        {
                            dataCSV.Rows.Add(rowValues);
                        }
                    var rowData = rowValues.ToList(); // Convert row to list
                    csvData.Add(rowData);



                }
                    gr.ItemsSource = dataCSV.DefaultView;
                //File.WriteAllText(Path.ChangeExtension(filePath, "carp.json"), json);
                //var json = JsonConvert.SerializeObject(csvLines), Formatting.Indented);
                if (CreateJsonCheck.IsChecked == true)
                {
                    var json = JsonConvert.SerializeObject(csvData, Formatting.Indented);

                    // var json = csvData;
                    // Optionally, write the JSON to a file
                    File.WriteAllText(Path.ChangeExtension(filePath, "awwwwk.json"), json);
                    //OpenJsonFile_Click(JsonButton, new RoutedEventArgs(Button.ClickEvent));
                    //DisplayJsonInDataGrid(jsonString);
                }


            }
        }

        private ScrollViewer GetScrollViewer(UIElement element)
        {
            if (element is ScrollViewer)
            {
                return (ScrollViewer)element;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                var result = GetScrollViewer(child as UIElement);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
        private void DataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (GridScrollCheck.IsChecked == true)
            {
                if (sender == gr)
                {
                    // Synchronize dataGrid2's scroll position with dataGrid1
                    var scrollViewer2 = GetScrollViewer(gr_Copy);
                    if (scrollViewer2 != null)
                    {
                        scrollViewer2.ScrollToHorizontalOffset(e.HorizontalOffset);
                        scrollViewer2.ScrollToVerticalOffset(e.VerticalOffset);
                    }
                }
                else if (sender == gr_Copy)
                {
                    // Synchronize dataGrid1's scroll position with dataGrid2
                    var scrollViewer1 = GetScrollViewer(gr);
                    if (scrollViewer1 != null)
                    {
                        scrollViewer1.ScrollToHorizontalOffset(e.HorizontalOffset);
                        scrollViewer1.ScrollToVerticalOffset(e.VerticalOffset);
                    }
                }
            }
            else if (GridScrollCheck.IsChecked == false) {
            }
        }
        private void OpenJsonFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"; // Filter to only show JSON or all files
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Optional: start directory
            if (openFileDialog.ShowDialog() == true)
            {
                var dataCSV = new DataTable();

                string fileContent = File.ReadAllText(openFileDialog.FileName);
                //var csvLines = File.ReadAllLines(openFileDialog.FileName);
               // var csvData = new List<List<string>>();


                //DataTable dataTable = JsonToDataTable(jsonString);
                MessageBox.Show(fileContent);
               //var data  = JsonConvert.DeserializeObject<List<ExpandoObject>>(fileContent, new ExpandoObjectConverter());
                // DisplayJsonInDataGrid(csvlines);
                
                // Set the parsed data as the ItemsSource for the DataGrid
                //gr_Copy.ItemsSource = data;
                DisplayJsonInDataGrid(fileContent);
                


            }
        }



    }
}