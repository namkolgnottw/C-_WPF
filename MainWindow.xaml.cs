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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.IO;
using System.Threading;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    /*
    public class cellTemplateSelector:DataTemplateSelector
    {
        public DataTemplate PositiveValueTemplate
        {
            get;set;
        }
        public DataTemplate NegativeValueTemplate
        {
            get;set;
        }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataRow row = item as DataRow;
            if (row != null)
            {
                if (long.Parse(row["V5"]) < 0)
                    return NegativeValueTemplate;
                else
                    return PositiveValueTemplate;
            }
            else
            {
                Logger.Log("Template Selector function parameter item convert to DataRow failed.");
                return base.SelectTemplate(item, container);
            }
        }
    }
    */
    /*
    class PositiveNegativeValueConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value < 0) // make neagtive number be blue
            {
                return Brushes.Blue;
            } else
            {
                return Brushes.Red;  // Positive number be red
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    */
    public class Logger
    {
        public static void LogStart()
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                w.WriteLine("-------------------------------\n");
                w.WriteLine("\r\nLogEntry : ");
            }
        }
        public static void Log(string logMessage)
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                w.WriteLine(logMessage);
            }
        }
    }

    public class Data_Table
    {
        public DataTable table;
        public string[] s_columnheader;
        public string[] v_columnheader;
        public StreamReader file;


        public void RowAdd(string[] s_column_val, string[] v_column_val)
        {
            object[] new_row = new object[145];
            for (int i = 0; i < s_columnheader.Length; i++)  // add s columns
            {
               
                new_row[i] = s_column_val[i];
            }
            int j = 0;

            for (int i = 13; i < v_columnheader.Length; i++,j++)  // add s columns
            {
                if (v_columnheader[j] == "V2")  // convert time format
                {
                    string a = "";
                    a = Char.ToString(v_column_val[j][4]) + ":" + Char.ToString(v_column_val[j][3]) + Char.ToString(v_column_val[j][2]) + ":" + Char.ToString(v_column_val[j][1]) + Char.ToString(v_column_val[j][0]);
                    new_row[i] = a;
                }
                else
                {
                    long val0;
                    if (v_column_val[j] == "")
                        val0 = 0;
                    else
                        val0 = long.Parse(v_column_val[j]);

                    if (val0 > 10000)
                    {
                        double val1 = Convert.ToDouble(val0) / 10000000000;
                        string s0 = String.Format("{0:F4}", val1);  // round to 2 decimal
                        new_row[i] = s0;

                    }
                    else
                    {
                        new_row[i] = v_column_val[j];
                    }
                }
            }
            //Logger.Log("end copying s_column_val, new_row prepared to write");

            table.BeginLoadData();
            table.LoadDataRow(new_row, true);
            table.EndLoadData();

        }
        public Data_Table()
        {
            file = new StreamReader(@"TickData.txt"); // for later read data
            s_columnheader = new string[] {"S0", "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "S10", "S11", "S12" };
            v_columnheader = new string[132];
            for (int i=0; i<132; i++)
            {
                v_columnheader[i] = "V"+ Convert.ToString(i);
            }

            table = new DataTable();
            var key = new DataColumn[1]; 

            for (int i = 0; i<s_columnheader.Length; i++)  // add s columns
            {
                DataColumn column = new DataColumn();
                column.DataType = Type.GetType("System.String");
                column.ColumnName = s_columnheader[i];
                table.Columns.Add(column);
                if (s_columnheader[i]=="S0")
                {
                    key[0] = column;  // add column "S0" to key
                }
            }
            for (int i = 0; i < v_columnheader.Length; i++)  // add s columns
            {
                    table.Columns.Add(v_columnheader[i], typeof(string));
            }
            table.PrimaryKey = key; // set unique key
        }
    }
    public partial class MainWindow : Window
    {
        public Data_Table data_table;

        /*
        public void initDataGrid()
        {
            string[] s_columnheader = new string[] { "S0", "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "S10", "S11", "S12" };
            string[] v_columnheader = new string[132];

            for (int i = 0; i < s_columnheader.Length; i++)
            {
                DataGridTemplateColumn template_col0 = new DataGridTemplateColumn();
                template_col0.Header = s_columnheader[i];
                DataGrid1.Colums.Add();
            }
        }
        */

        public MainWindow()
        {
            InitializeComponent();

            data_table = new Data_Table();
            DataGrid1.ItemsSource = data_table.table.DefaultView;



        }
    }
}
