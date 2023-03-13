using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PLCSystem.Base;
using PLCSystem.Common;
using PLCSystem.Config;
using PLCSystem.Views;



namespace PLCSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //if (new LoginWindow().ShowDialog() == true)
        //{
        //    new MainWindow().ShowDialog();
        //}





        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (File.Exists(Path.GetFullPath("SystemParam.ini"))!=true) 
            {
                ConfigInfo.SaveLeftIniFile();
                ConfigInfo.SaveUnitIniFile();
                ConfigInfo.SaveAlarmIniFile();
            }
            GlobalMonitor.Start();
            new MainWindow().ShowDialog();
            Application.Current.Shutdown();

      

        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            GlobalMonitor.Dispose();

        }
    }
}
