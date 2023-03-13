using PLCSystem.Service;
using PLCSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
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

namespace PLCSystem.Views
{
    /// <summary>
    /// PLCMon.xaml 的交互逻辑
    /// </summary>
    public partial class PLCMon : UserControl
    {

   
        public PLCMon()
        {
            InitializeComponent();
            this.DataContext = new PLCMonViewModels(this, EquMon, LeftSwitch, RightSwitch, 0, 11, 1);

        }




      
     
     
    }
}
