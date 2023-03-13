using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using PLCSystem.Models;
using PLCSystem.Base;

namespace PLCSystem.ViewModels
{
    public class MainViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public MainModel MainModel { get; set; } = new MainModel();

        private void NavPage(string name)
        {
            Type type = Type.GetType(name);
            this.MainModel.MainContent = (System.Windows.UIElement)Activator.CreateInstance(type);
        }
        public MainViewModel()
        {

            //MainModel.UserName = "Administartor";

            this.NavPage("PLCSystem.Views.PLCMon");

            //Task.Run(async () =>
            //{
            //    while (true)
            //    {
            //        await Task.Delay(500);
            //        //MainModel.Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //    }
            //});
        }

    }
}



//private CommandBase _menuItemCommand;



//public CommandBase MenuItemCommand
//{
//    get
//    {
//        if (_menuItemCommand == null)
//        {
//            _menuItemCommand = new CommandBase();
//            _menuItemCommand.DoExecute = new Action<object>(obj =>
//            {
//                // 反射
//                this.NavPage(obj.ToString());
//            });
//        }
//        return _menuItemCommand;
//    }
//}