using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
//using System.Data;
using System.IO;
using System.Threading;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            Logger.LogStart();
            Logger.Log("program start");
            MainWindow wnd0 = new MainWindow();
            wnd0.Show();
            Data_Table table1 = wnd0.data_table;

            new Thread(() =>
            {
                Data_Table table1 = wnd0.data_table;
                    string line;
                    while ((line = table1.file.ReadLine()) != null)
                    {

                      this.Dispatcher.Invoke(new Action(()=>
                      {
                          for (int k = 0; k < 100; k++)
                          {
                              string line0 = table1.file.ReadLine();
                              if (line0 == null) break;

                              string line2 = line0.Remove(line0.Length - 2, 2); // remove ";." in the end of line

                              string[] line_split;
                              line_split = line2.Split(";");

                              string[] v_column_value = new string[132];
                              string[] s_column_value = new string[13];
                              for (int i = 0; i < v_column_value.Length; i++)
                                  v_column_value[i] = "";
                              for (int i = 0; i < s_column_value.Length; i++)
                                  s_column_value[i] = "";

                              foreach (string a in line_split)
                              {
                                  string[] split0 = a.Split("=");

                                  string[] split1 = split0[0].Split(":");
                                  if (split1[0] == "V")
                                  {
                                      v_column_value[Convert.ToInt32(split1[1])] = split0[1];
                                  }
                                  else
                                  { // split1[0] == "s"
                                      s_column_value[Convert.ToInt32(split1[1])] = split0[1];
                                  }
                              }

                              // Read 100 rows
                              table1.RowAdd(s_column_value, v_column_value); // bug here
                              Logger.Log("row added");

                          }
                      }));

                    Logger.Log("10 row updated, sleep 1 sec");
                    Thread.Sleep(1000);

                }
                table1.file.Close();

            }).Start();

            Logger.Log("end thread start");

        }

    }
}
