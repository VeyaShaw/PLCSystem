using log4net.Core;
using Newtonsoft.Json.Linq;
using PLCSystem.Base;
using PLCSystem.Common.Log;
using PLCSystem.Config;
using PLCSystem.Models;
using PLCSystem.Service;
using S7.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Linq;

namespace PLCSystem.ViewModels
{
   public class PLCMonViewModels: INotifyPropertyChanged
    {

        private static Grid Crposlayout;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly static TaskScheduler _syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        /// <summary>
        /// A
        /// </summary>
        public static Task FirstGroupTask = null;
        /// <summary>
        /// 是否开启报警
        /// </summary>
        public static bool isRunning = true;
     
        /// <summary>
        /// 总设备数
        /// </summary>
        private static int TotalDevSet = 0;
        /// <summary>
        /// 总页数
        /// </summary>
        private static int PageCount;
        /// <summary>
        /// 每页
        /// </summary>
        private static int EachPage = 12;
        /// <summary>
        /// 当前页
        /// </summary>
        private static int CurrentPage;
        ///// <summary>
        ///// 开始页
        ///// </summary>
        private static int StartPage;
        /// <summary>
        /// 结束页
        /// </summary>
        private static int EndPage;

        private readonly static string temp = ConfigInfo.ReadIniString("LeftLayout", "温度").Trim();

        private readonly static string den = ConfigInfo.ReadIniString("LeftLayout", "密度").Trim();

        private readonly static string pres = ConfigInfo.ReadIniString("LeftLayout", "压力").Trim();

        private readonly static string level = ConfigInfo.ReadIniString("LeftLayout", "液位").Trim();

        private readonly static string insquan = ConfigInfo.ReadIniString("UnitConfig", "预装量").Trim();

        private readonly static string Actquan = ConfigInfo.ReadIniString("UnitConfig", "实装量").Trim();

        private readonly static string Insflow = ConfigInfo.ReadIniString("UnitConfig", "瞬时流量").Trim();
        private readonly static string Actflow = ConfigInfo.ReadIniString("UnitConfig", "累计流量").Trim();
        private readonly static string Tempun = ConfigInfo.ReadIniString("UnitConfig", "温度").Trim();
        private readonly static string Denun = ConfigInfo.ReadIniString("UnitConfig", "密度").Trim();
        private readonly static string Presun = ConfigInfo.ReadIniString("UnitConfig", "压力").Trim();
        private readonly static string Levun = ConfigInfo.ReadIniString("UnitConfig", "液位").Trim();
        private static  List<SYSConfigModel> ConList = new List<SYSConfigModel>();
        private CommandBase leftpage;
        public CommandBase Leftpage
        {
            get
            {
                if (leftpage == null)
                {
                   
                    leftpage = new CommandBase();
                    leftpage.DoExecute = new Action<object>(obj =>
                    {
                        UserControl user = obj as UserControl;
                        Grid EquMon = user.FindName("EquMon") as Grid;
                        Button Leftbut = EquMon.FindName("LeftSwitch") as Button;
                        Button Rightbut = EquMon.FindName("RightSwitch") as Button;
                        int index = CurrentPage - 2;
                        CurrentPage--;
                        if (CurrentPage == 1)
                        {
                            Rightbut.Visibility = Visibility.Visible;
                            Leftbut.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            Rightbut.Visibility = Visibility.Visible;
                            Leftbut.Visibility = Visibility.Visible;
                        }

                        var sub = keys.ElementAt(index);
                        var main = Convert.ToInt32(sub.Key);
                        Deinitialization(user, EquMon, sub.Value + 1, sub.Value + EachPage);
                        PageCalc(user, EquMon, Leftbut, Rightbut, Convert.ToInt32(main), sub.Value, 3);
                        FirstGroupDispose();
                        isRunning = true;
                        StartPage = Convert.ToInt32(main);
                        EndPage = sub.Value;
                        DeviceCollection(StartPage, EndPage);
                        FirstGroupDevice(user, EquMon, Leftbut, Rightbut);
                    });
                }
                return leftpage;
            }
        }

        private CommandBase righpage;
        public CommandBase RighPage
        {
            get
            {
                if (righpage == null)
                {
                    righpage = new CommandBase();
                    righpage.DoExecute = new Action<object>(obj =>
                    {
                        FirstGroupDispose();
                        UserControl user = obj as UserControl;
                        Grid EquMon = user.FindName("EquMon") as Grid;
                        Button Leftbut = EquMon.FindName("LeftSwitch") as Button;
                        Button Rightbut = EquMon.FindName("RightSwitch") as Button;
                        Leftbut.Visibility = Visibility.Visible;
                        CurrentPage++;
                        int index = 0;
                        if (CurrentPage == PageCount)
                        {
                            Rightbut.Visibility = Visibility.Hidden;
                            Leftbut.Visibility = Visibility.Visible;
                            if (keys.Count == 4)
                            {
                                index = CurrentPage - 2;
                            }
                            else
                            {
                                index = CurrentPage - 2;
                            }

                        }
                        else
                        {
                            index = CurrentPage - 2;
                        }
                        var sub = keys.ElementAt(index);
                        var main = Convert.ToInt32(sub.Key);

                        Deinitialization(user, EquMon, Convert.ToInt32(main), sub.Value);
                        StartPage = EndPage + 1;
                        EndPage = EndPage + EachPage;
                        PageCalc(user, EquMon, Leftbut, Rightbut, StartPage, EndPage, 2);
                        isRunning = true;
                        DeviceCollection(StartPage, EndPage);
                        FirstGroupDevice(user, EquMon, Leftbut, Rightbut);
                    });
                }
                return righpage;

            }
        }

        private static List<SYSCraneSetModel> setModelsList = new List<SYSCraneSetModel>();
        private static Dictionary<int, int> keys = new Dictionary<int, int>();

        private static DeviceService devser = new DeviceService();

        private static CommandBase stopA;
        public static CommandBase StopA
        {
            get
            {
                if (stopA == null)
                {
                    stopA = new CommandBase();
                    stopA.DoExecute = new Action<object>(obj =>
                    {
                      
                            Crposlayout.Dispatcher.Invoke(()=> 
                            {
                                try
                                {
                                    string[] name = obj.ToString().Split(',');
                                    Plc plc = new Plc(CpuType.S7200Smart, name[0], 0, 1);
                                    plc.Open();
                                    var statusA = plc.Read("Q2.0");
                                    foreach (SYSDeviceModel de in devser.Commdata(name[0]))
                                    {

                                        Button Stop = Crposlayout.FindName(de.EmeStopName) as Button;
                                        Button Stoper = Crposlayout.FindName(de.AlarmJName) as Button;
                                        if (Convert.ToBoolean(statusA) == true)
                                        {
                                            plc.Write("Q2.0", (int)0);
                                            if (Stop != null && Stoper != null)
                                            {
                                                Stop.Style = (Style)Crposlayout.FindResource("ButtonInfo");
                                                Stoper.Style = (Style)Crposlayout.FindResource("ButtonSuccess");
                                            }
                                        }
                                        else if (Convert.ToBoolean(statusA) == false)
                                        {
                                            plc.Write("Q2.0", (int)1);
                                            if (Stop != null && Stoper != null)
                                            {
                                                Stop.Style = (Style)Crposlayout.FindResource("ButtonDanger");
                                                Stoper.Style = (Style)Crposlayout.FindResource("ButtonDanger");
                                            }
                                        }
                                    }
                                    plc.Close();
                                }
                                catch (Exception ex) 
                                {
                                    LogerHelper.Error("Stop",ex);
                                }
                              
                            });
                    });
                }
                return stopA;
            }
        }

        //SYSDevicebus
        private static List<SYSConfigModel> ConMoList = new List<SYSConfigModel>();
        public static void PageCalc(UserControl user, Grid firstfloor, Button LeftSwitch, Button RightSwitch, int pages, int end, int buytype)
        {
            try
            {
                if (buytype == 2 || buytype == 1)
                {
                    if (buytype == 1)
                    {
                        keys.Add(pages, end);
                    }
                    else
                    {
                        if (keys.ContainsKey(pages))
                        {

                        }
                        else
                        {
                            keys.Add(pages, end);
                        }
                    }
                }
                if (buytype == 1)
                {
                    TotalDevSet = DeviceService.DeviceCount();
                    PageCount = (TotalDevSet + EachPage - 1) / EachPage;
                    setModelsList = DeviceService.TotalDeviceCount();
                    if (PageCount == 0 || PageCount == 1)
                    {
                        LeftSwitch.Visibility = Visibility.Hidden;
                        RightSwitch.Visibility = Visibility.Hidden;
                        CurrentPage = 1;
                    }
                    else if (PageCount > 1)
                    {
                        LeftSwitch.Visibility = Visibility.Hidden;
                        RightSwitch.Visibility = Visibility.Visible;
                        CurrentPage = 1;
                    }
                }
                int countA = 1;
                int A = 0;
                int B = 0;
                if (buytype == 1 || buytype == 2)
                {
                    A = pages + 5;
                    B = A + 6;
                } 
                else if (buytype==3) 
                {
                    A = pages + 5;
                    B = end;
                }
                for (int i = pages; i <= end; i++)
                {
                    EndPage = i;
                    SYSDeviceModel mo = new SYSDeviceModel();
                    int Actnum = setModelsList.Count - 1;
                    if (i <= Actnum)
                    {
                        mo.MachineNo = setModelsList[i].MachineNo;
                        mo.CraneNO = setModelsList[i].CraneNO;
                        foreach (SYSDeviceModel dev in DeviceService.deviceList(mo, 1))
                        {
                           TabControl tab = new TabControl();
                            tab.Name = dev.MaName;
                            HandyControl.Controls.TabControl tabzc = firstfloor.FindName(dev.MaName) as HandyControl.Controls.TabControl;
                            if (tabzc == null)
                            {
                                firstfloor.RegisterName(dev.MaName, tab);
                            }
                            else
                            {
                                firstfloor.Children.Remove(tab);
                                firstfloor.UnregisterName(dev.MaName);
                                firstfloor.RegisterName(dev.MaName, tab);
                            }
                            tab.FontSize = 10;
                            Thickness Thickness = new Thickness(1, 2, 1, 2);
                            tab.Margin = Thickness;
                            tab.Style = (Style)user.FindResource("TabControlInLine");
                            if (i <= A)
                            {
                                Grid.SetRow(tab, 0);
                                Grid.SetColumn(tab, countA);
                            }
                            else if (i <= B)
                            {
                                if (i == A + 1)
                                {
                                    countA = 1;
                                }
                                Grid.SetRow(tab, 1);
                                Grid.SetColumn(tab, countA);
                            }
                          TabItem tabItem = new TabItem();
                            tabItem.Name = dev.CrName;
                            Grid gritab = new Grid();
                            ColumnDefinition SeA = new ColumnDefinition();
                            SeA.Width = new GridLength(35);
                            ColumnDefinition SeB = new ColumnDefinition();
                            SeB.Width = new GridLength(138);
                            ColumnDefinition SeC = new ColumnDefinition();
                            SeC.Width = new GridLength(35);
                            gritab.ColumnDefinitions.Add(SeA);
                            gritab.ColumnDefinitions.Add(SeB);
                            gritab.ColumnDefinitions.Add(SeC);

                            TextBlock seat = new TextBlock();
                            seat.HorizontalAlignment = HorizontalAlignment.Left;
                            seat.Text = dev.MachineNo + "机" + dev.CraneNO;
                            seat.Width = 35;
                            seat.FontSize = 10;
                            seat.Style = (Style)user.FindResource("TextBlockDefaultPrimary");
                            Grid.SetColumn(seat, 0);
                            gritab.Children.Add(seat);

                            TextBlock CrN = new TextBlock();
                            CrN.HorizontalAlignment = HorizontalAlignment.Left;
                            CrN.Text = dev.MaNameal;
                            CrN.FontSize = 9;
                            CrN.Width = 135;
                            CrN.Style = (Style)user.FindResource("TextBlockDefaultPrimary");
                            Grid.SetColumn(CrN, 1);
                            gritab.Children.Add(CrN);

                            TextBlock CrPstaus = new TextBlock();
                            CrPstaus.HorizontalAlignment = HorizontalAlignment.Right;
                            CrPstaus.Name = dev.StatusName;
                            CrPstaus.Text = dev.CraneStatus;
                            CrPstaus.FontSize = 10;
                            CrPstaus.Width = 35;
                            CrPstaus.Style = (Style)user.FindResource("TextBlockDefaultPrimary");
                            Grid.SetColumn(CrPstaus, 2);
                            gritab.Children.Add(CrPstaus);
                            TextBlock CrPsta = firstfloor.FindName(dev.StatusName) as TextBlock;
                            if (CrPsta == null)
                            {
                                firstfloor.RegisterName(dev.StatusName, CrPstaus);
                            }
                            else
                            {
                                firstfloor.Children.Remove(CrPsta);
                                firstfloor.UnregisterName(dev.StatusName);
                                firstfloor.RegisterName(dev.StatusName, CrPstaus);
                            }

                            tabItem.Header = gritab;
                            TabItem Item = firstfloor.FindName(dev.CrName) as TabItem;
                            if (Item == null)
                            {
                                firstfloor.RegisterName(dev.CrName, tabItem);
                            }
                            else
                            {
                                firstfloor.Children.Remove(tabItem);
                                firstfloor.UnregisterName(dev.CrName);
                                firstfloor.RegisterName(dev.CrName, tabItem);
                            }
                            var bc = new BrushConverter();
                            tabItem.Background = (Brush)bc.ConvertFrom("#FFE5E5E5");
                            #region grid布局
                            int height = 25;
                            int width = 78;
                            Grid grid = new Grid();
                            #region gridcontent
                            //grid.ShowGridLines = true;
                            grid.Background = (Brush)bc.ConvertFrom("#FFE5E5E5");
                            grid.Height = 351;
                            RowDefinition rowA = new RowDefinition();
                            rowA.Height = new GridLength(27);
                            RowDefinition rowB = new RowDefinition();
                            rowB.Height = new GridLength(27);
                            RowDefinition rowC = new RowDefinition();
                            rowC.Height = new GridLength(27);
                            RowDefinition rowD = new RowDefinition();
                            rowD.Height = new GridLength(27);
                            RowDefinition rowE = new RowDefinition();
                            rowE.Height = new GridLength(27);
                            RowDefinition rowF = new RowDefinition();
                            rowF.Height = new GridLength(27);
                            RowDefinition rowG = new RowDefinition();
                            rowG.Height = new GridLength(27);
                            RowDefinition rowH = new RowDefinition();
                            rowH.Height = new GridLength(27);
                            RowDefinition rowJ = new RowDefinition();
                            rowJ.Height = new GridLength(27);
                            RowDefinition rowK = new RowDefinition();
                            rowK.Height = new GridLength(27);
                            RowDefinition rowL = new RowDefinition();
                            rowL.Height = new GridLength(27);
                            RowDefinition rowQ = new RowDefinition();
                            rowQ.Height = new GridLength(27);


                            grid.RowDefinitions.Add(rowA);
                            grid.RowDefinitions.Add(rowB);
                            grid.RowDefinitions.Add(rowC);
                            grid.RowDefinitions.Add(rowD);
                            grid.RowDefinitions.Add(rowE);
                            grid.RowDefinitions.Add(rowF);
                            grid.RowDefinitions.Add(rowG);
                            grid.RowDefinitions.Add(rowH);
                            grid.RowDefinitions.Add(rowJ);
                            grid.RowDefinitions.Add(rowK);
                            grid.RowDefinitions.Add(rowL);
                            grid.RowDefinitions.Add(rowQ);



                            ColumnDefinition colA = new ColumnDefinition();
                            colA.Width = new GridLength(60);
                            ColumnDefinition colB = new ColumnDefinition();
                            colB.Width = new GridLength(80);
                            ColumnDefinition colC = new ColumnDefinition();
                            colB.Width = new GridLength(80);

                            grid.ColumnDefinitions.Add(colA);
                            grid.ColumnDefinitions.Add(colB);
                            grid.ColumnDefinitions.Add(colC);
                            #endregion
                            #region 第1部分
                            StackPanel stackD = new StackPanel();
                            TextBlock txtD = new TextBlock();
                            Thickness txtTHD = new Thickness(5, 0, 0, 0);
                            txtD.Margin = txtTHD;
                            txtD.Text = "当前单号";
                            txtD.VerticalAlignment = VerticalAlignment.Center;
                            txtD.Style = (Style)user.FindResource("TextBlockDefaultBold");
                            Grid.SetRow(stackD, 0);
                            stackD.Orientation = Orientation.Horizontal;
                            stackD.VerticalAlignment = VerticalAlignment.Center;
                            Thickness STTHC = new Thickness(5, 0, 0, 0);
                            stackD.Margin = STTHC;
                            stackD.Children.Add(txtD);
                            #endregion
                            grid.Children.Add(stackD);
                            #region 第2部分
                            StackPanel stackE = new StackPanel();
                            Label lblE = new Label();
                            lblE.Name = dev.OrName;
                            lblE.Content = dev.OrderNumber;

                            Label lblDH = firstfloor.FindName(dev.OrName) as Label;
                            if (lblDH == null)
                            {
                                firstfloor.RegisterName(dev.OrName, lblE);
                            }
                            else
                            {
                                firstfloor.Children.Remove(lblDH);
                                firstfloor.UnregisterName(dev.OrName);
                                firstfloor.RegisterName(dev.OrName, lblE);
                            }
                            lblE.Style = (Style)user.FindResource("LabelInfo");
                            lblE.VerticalAlignment = VerticalAlignment.Center;
                            lblE.Height = height;
                            lblE.Width = width;
                            Grid.SetRow(stackE, 0);
                            Grid.SetColumn(stackE, 1);
                            stackE.Orientation = Orientation.Horizontal;
                            stackE.VerticalAlignment = VerticalAlignment.Center;
                            stackE.Children.Add(lblE);
                            #endregion
                            grid.Children.Add(stackE);
                            #region 第3部分
                            StackPanel stackF = new StackPanel();
                            TextBlock txtE = new TextBlock();
                            Thickness txtTHE = new Thickness(5, 0, 0, 0);
                            txtE.Margin = txtTHE;
                            txtE.Text = "车  牌  号 ";
                            txtE.VerticalAlignment = VerticalAlignment.Center;
                            txtE.Style = (Style)user.FindResource("TextBlockDefaultBold");
                            Grid.SetRow(stackF, 1);
                            stackF.Orientation = Orientation.Horizontal;
                            stackF.VerticalAlignment = VerticalAlignment.Center;
                            Thickness STTHD = new Thickness(5, 0, 0, 0);
                            stackF.Margin = STTHD;
                            stackF.Children.Add(txtE);
                            #endregion
                            grid.Children.Add(stackF);
                            #region 第4部分
                            StackPanel stackG = new StackPanel();
                            Label lblF = new Label();
                            lblF.Name = dev.CarNumber;
                            lblF.Content = dev.CarNumber;
                            Label lblCPH = firstfloor.FindName(dev.CNName) as Label;
                            if (lblCPH == null)
                            {
                                firstfloor.RegisterName(dev.CNName, lblF);
                            }
                            else
                            {
                                firstfloor.Children.Remove(lblCPH);
                                firstfloor.UnregisterName(dev.CNName);
                                firstfloor.RegisterName(dev.CNName, lblF);
                            }
                            lblF.Style = (Style)user.FindResource("LabelInfo");
                            lblF.VerticalAlignment = VerticalAlignment.Center;
                            lblF.Height = height;
                            lblF.Width = width;
                            Grid.SetRow(stackG, 1);
                            Grid.SetColumn(stackG, 1);
                            stackG.Orientation = Orientation.Horizontal;
                            stackG.VerticalAlignment = VerticalAlignment.Center;
                            stackG.Children.Add(lblF);
                            #endregion
                            grid.Children.Add(stackG);

                            //#region 第5部分
                            //StackPanel stackC = new StackPanel();
                            //TextBlock txtC = new TextBlock();
                            //txtC.Text = "     报警状态";

                            //txtC.VerticalAlignment = VerticalAlignment.Center;
                            //txtC.Style = (Style)user.FindResource("TextBlockDefaultBold");
                            //Grid.SetRow(stackC, 0);
                            //Grid.SetColumn(stackC, 3);
                            //stackC.Orientation = Orientation.Horizontal;
                            //stackC.VerticalAlignment = VerticalAlignment.Center;
                            //Thickness StB = new Thickness(5, 0, 0, 0);
                            //stackC.Margin = StB;
                            //stackC.Children.Add(txtC);
                            //#endregion
                            //grid.Children.Add(stackC);

                            #region 第6部分
                            StackPanel stackH = new StackPanel();
                            TextBlock txtF = new TextBlock();
                            Thickness txtTHF = new Thickness(3, 0, 0, 0);
                            txtF.Margin = txtTHF;
                            txtF.Text = "预  装  量 ";
                            txtF.VerticalAlignment = VerticalAlignment.Center;
                            txtF.Style = (Style)user.FindResource("TextBlockDefaultBold");
                            Grid.SetRow(stackH, 2);
                            stackH.Orientation = Orientation.Horizontal;
                            stackH.VerticalAlignment = VerticalAlignment.Center;
                            Thickness STTHF = new Thickness(5, 0, 0, 0);
                            stackH.Margin = STTHF;
                            stackH.Children.Add(txtF);
                            #endregion
                            grid.Children.Add(stackH);

                            #region 第7部分
                            StackPanel stackJ = new StackPanel();
                            Label lblG = new Label();
                            lblG.Name = dev.PVName;
                            lblG.Content = dev.PlannedVolume + insquan;
                            Label lblYZL = firstfloor.FindName(dev.PVName) as Label;
                            if (lblYZL == null)
                            {
                                firstfloor.RegisterName(dev.PVName, lblG);
                            }
                            else
                            {
                                firstfloor.Children.Remove(lblYZL);
                                firstfloor.UnregisterName(dev.PVName);
                                firstfloor.RegisterName(dev.PVName, lblG);
                            }
                            lblG.Style = (Style)user.FindResource("LabelInfo");
                            lblG.VerticalAlignment = VerticalAlignment.Center;
                            lblG.Height = height;
                            lblG.Width = width;
                            Grid.SetRow(stackJ, 2);
                            Grid.SetColumn(stackJ, 1);
                            stackJ.Orientation = Orientation.Horizontal;
                            stackJ.VerticalAlignment = VerticalAlignment.Center;
                            stackJ.Children.Add(lblG);
                            #endregion
                            grid.Children.Add(stackJ);

                            #region 第8部分
                            StackPanel stackK = new StackPanel();
                            TextBlock txtG = new TextBlock();
                            Thickness txtTHG = new Thickness(3, 0, 0, 0);
                            txtG.Margin = txtTHG;
                            txtG.Text = "实  装  量 ";
                            txtG.VerticalAlignment = VerticalAlignment.Center;
                            txtG.Style = (Style)user.FindResource("TextBlockDefaultBold");
                            Grid.SetRow(stackK, 3);
                            stackK.Orientation = Orientation.Horizontal;
                            stackK.VerticalAlignment = VerticalAlignment.Center;
                            Thickness STTHG = new Thickness(5, 0, 0, 0);
                            stackK.Margin = STTHG;
                            stackK.Children.Add(txtG);
                            #endregion
                            grid.Children.Add(stackK);

                            #region 第9部分
                            StackPanel stackL = new StackPanel();
                            Label lblH = new Label();
                            lblH.Name = dev.AIName;
                            lblH.Content = dev.AmountInstall + Actquan;
                            Label lblSZL = firstfloor.FindName(dev.AIName) as Label;
                            if (lblYZL == null)
                            {
                                firstfloor.RegisterName(dev.AIName, lblH);
                            }
                            else
                            {
                                firstfloor.Children.Remove(lblSZL);
                                firstfloor.UnregisterName(dev.AIName);
                                firstfloor.RegisterName(dev.AIName, lblH);
                            }
                            lblH.Style = (Style)user.FindResource("LabelInfo");
                            lblH.VerticalAlignment = VerticalAlignment.Center;
                            lblH.Height = height;
                            lblH.Width = width;
                            Grid.SetRow(stackL, 3);
                            Grid.SetColumn(stackL, 1);
                            stackL.Orientation = Orientation.Horizontal;
                            stackL.VerticalAlignment = VerticalAlignment.Center;
                            stackL.Children.Add(lblH);
                            #endregion
                            grid.Children.Add(stackL);

                            #region 第10部分
                            StackPanel stackQ = new StackPanel();
                            TextBlock txtH = new TextBlock();
                            Thickness txtTHH = new Thickness(3, 0, 0, 0);
                            txtH.Margin = txtTHH;
                            txtH.Text = "瞬时流量";
                            txtH.VerticalAlignment = VerticalAlignment.Center;
                            txtH.Style = (Style)user.FindResource("TextBlockDefaultBold");
                            Grid.SetRow(stackQ, 4);
                            stackQ.Orientation = Orientation.Horizontal;
                            stackQ.VerticalAlignment = VerticalAlignment.Center;
                            Thickness STTHH = new Thickness(5, 0, 0, 0);
                            stackQ.Margin = STTHH;
                            stackQ.Children.Add(txtH);
                            #endregion
                            grid.Children.Add(stackQ);

                            #region 第11部分
                            StackPanel stackW = new StackPanel();
                            Label lblJ = new Label();
                            lblJ.Name = dev.ITName;
                            lblJ.Content = dev.InsTraffic + Insflow;
                            Label lblSHLL = firstfloor.FindName(dev.ITName) as Label;
                            if (lblSHLL == null)
                            {
                                firstfloor.RegisterName(dev.ITName, lblJ);
                            }
                            else
                            {
                                firstfloor.Children.Remove(lblSHLL);
                                firstfloor.UnregisterName(dev.ITName);
                                firstfloor.RegisterName(dev.ITName, lblJ);
                            }
                            lblJ.Style = (Style)user.FindResource("LabelInfo");
                            lblJ.VerticalAlignment = VerticalAlignment.Center;
                            lblJ.Height = height;
                            lblJ.Width = width;
                            Grid.SetRow(stackW, 4);
                            Grid.SetColumn(stackW, 1);
                            stackW.Orientation = Orientation.Horizontal;
                            stackW.VerticalAlignment = VerticalAlignment.Center;
                            stackW.Children.Add(lblJ);
                            #endregion
                            grid.Children.Add(stackW);

                            #region 第12部分
                            StackPanel stackR = new StackPanel();
                            TextBlock txtJ = new TextBlock();
                            Thickness txtTHJ = new Thickness(3, 0, 0, 0);
                            txtJ.Margin = txtTHH;
                            txtJ.Text = "累计流量";
                            txtJ.VerticalAlignment = VerticalAlignment.Center;
                            txtJ.Style = (Style)user.FindResource("TextBlockDefaultBold");
                            Grid.SetRow(stackR, 5);
                            stackR.Orientation = Orientation.Horizontal;
                            stackR.VerticalAlignment = VerticalAlignment.Center;
                            Thickness STTHJ = new Thickness(5, 0, 0, 0);
                            stackR.Margin = STTHH;
                            stackR.Children.Add(txtJ);
                            #endregion
                            grid.Children.Add(stackR);

                            #region 第13部分
                            StackPanel stackT = new StackPanel();
                            Label lblK = new Label();
                            lblK.Name = dev.AName;
                            lblK.Content = dev.Accumulate + Actflow;
                            Label lblLJLL = firstfloor.FindName(dev.AName) as Label;
                            if (lblLJLL == null)
                            {
                                firstfloor.RegisterName(dev.AName, lblK);
                            }
                            else
                            {
                                firstfloor.Children.Remove(lblLJLL);
                                firstfloor.UnregisterName(dev.AName);
                                firstfloor.RegisterName(dev.AName, lblK);
                            }
                            lblK.Style = (Style)user.FindResource("LabelInfo");
                            lblK.VerticalAlignment = VerticalAlignment.Center;
                            lblK.Height = height;
                            lblK.Width = width;
                            Grid.SetRow(stackT, 5);
                            Grid.SetColumn(stackT, 1);
                            stackT.Orientation = Orientation.Horizontal;
                            stackT.VerticalAlignment = VerticalAlignment.Center;
                            stackT.Children.Add(lblK);
                            #endregion
                            grid.Children.Add(stackT);

                            int Count = 6;
                            if (temp == "1")
                            {
                                #region 第14部分
                                StackPanel stackY = new StackPanel();
                                TextBlock txtK = new TextBlock();
                                Thickness txtTHK = new Thickness(3, 0, 0, 0);
                                txtK.Margin = txtTHK;
                                txtK.Text = " 温      度 ";
                                txtK.VerticalAlignment = VerticalAlignment.Center;
                                txtK.Style = (Style)user.FindResource("TextBlockDefaultBold");
                                Grid.SetRow(stackY, Count);
                                stackY.Orientation = Orientation.Horizontal;
                                stackY.VerticalAlignment = VerticalAlignment.Center;
                                Thickness STTHK = new Thickness(5, 0, 0, 0);
                                stackY.Margin = STTHK;
                                stackY.Children.Add(txtK);
                                #endregion
                                grid.Children.Add(stackY);
                                #region 第15部分
                                StackPanel stU = new StackPanel();
                                Label lblL = new Label();
                                lblL.Name = dev.TName;
                                lblL.Content = dev.Temperature + Tempun;

                                Label lblWD = firstfloor.FindName(dev.TName) as Label;
                                if (lblWD == null)
                                {
                                    firstfloor.RegisterName(dev.TName, lblL);
                                }
                                else
                                {
                                    firstfloor.Children.Remove(lblWD);
                                    firstfloor.UnregisterName(dev.TName);
                                    firstfloor.RegisterName(dev.TName, lblL);
                                }
                                lblL.Style = (Style)user.FindResource("LabelInfo");
                                lblL.VerticalAlignment = VerticalAlignment.Center;
                                lblL.Height = height;
                                lblL.Width = width;
                                Grid.SetRow(stU, Count);
                                Grid.SetColumn(stU, 1);
                                stU.Orientation = Orientation.Horizontal;
                                stU.VerticalAlignment = VerticalAlignment.Center;
                                stU.Children.Add(lblL);
                                #endregion
                                grid.Children.Add(stU);
                                Count++;
                            }
                            if (den == "1")
                            {
                                #region 第16部分
                                StackPanel STI = new StackPanel();
                                TextBlock txtl = new TextBlock();
                                Thickness txtTHL = new Thickness(3, 0, 0, 0);
                                txtl.Margin = txtTHL;
                                txtl.Text = " 密      度 ";
                                txtl.VerticalAlignment = VerticalAlignment.Center;
                                txtl.Style = (Style)user.FindResource("TextBlockDefaultBold");
                                Grid.SetRow(STI, Count);
                                STI.Orientation = Orientation.Horizontal;
                                STI.VerticalAlignment = VerticalAlignment.Center;
                                Thickness STTHL = new Thickness(5, 0, 0, 0);
                                STI.Margin = STTHL;
                                STI.Children.Add(txtl);
                                #endregion
                                grid.Children.Add(STI);

                                #region 第17部分
                                StackPanel STO = new StackPanel();
                                Label lblQ = new Label();
                                lblQ.Name = dev.DName;
                                lblQ.Content = dev.Denser + Denun;
                                Label lblMD = firstfloor.FindName(dev.DName) as Label;
                                if (lblMD == null)
                                {
                                    firstfloor.RegisterName(dev.DName, lblQ);
                                }
                                else
                                {
                                    firstfloor.Children.Remove(lblMD);
                                    firstfloor.UnregisterName(dev.DName);
                                    firstfloor.RegisterName(dev.DName, lblQ);
                                }
                                lblQ.Style = (Style)user.FindResource("LabelInfo");
                                lblQ.VerticalAlignment = VerticalAlignment.Center;
                                lblQ.Height = height;
                                lblQ.Width = width;
                                Grid.SetRow(STO, Count);
                                Grid.SetColumn(STO, 1);
                                STO.Orientation = Orientation.Horizontal;
                                STO.VerticalAlignment = VerticalAlignment.Center;
                                STO.Children.Add(lblQ);
                                #endregion
                                grid.Children.Add(STO);
                                Count++;
                            }
                            if (pres == "1")
                            {
                                #region 第18部分
                                StackPanel STP = new StackPanel();
                                TextBlock txtQ = new TextBlock();
                                Thickness txtTHQ = new Thickness(3, 0, 0, 0);
                                txtQ.Margin = txtTHQ;
                                txtQ.Text = " 压      力 ";
                                txtQ.VerticalAlignment = VerticalAlignment.Center;
                                txtQ.Style = (Style)user.FindResource("TextBlockDefaultBold");
                                Grid.SetRow(STP, Count);
                                STP.Orientation = Orientation.Horizontal;
                                STP.VerticalAlignment = VerticalAlignment.Center;
                                Thickness STTHQ = new Thickness(5, 0, 0, 0);
                                STP.Orientation = Orientation.Horizontal;
                                STP.Margin = STTHQ;
                                STP.Children.Add(txtQ);
                                #endregion
                                grid.Children.Add(STP);
                                #region 第19部分
                                StackPanel STZ = new StackPanel();
                                Label lblW = new Label();

                                lblW.Name = dev.PName;
                                lblW.Content = dev.Pressure + Presun;
                                Label lblYL = firstfloor.FindName(dev.PName) as Label;
                                if (lblYL == null)
                                {
                                    firstfloor.RegisterName(dev.PName, lblW);
                                }
                                else
                                {
                                    firstfloor.Children.Remove(lblYL);
                                    firstfloor.UnregisterName(dev.PName);
                                    firstfloor.RegisterName(dev.PName, lblW);
                                }
                                lblW.Style = (Style)user.FindResource("LabelInfo");
                                lblW.VerticalAlignment = VerticalAlignment.Center;
                                lblW.Height = height;
                                lblW.Width = width;
                                Grid.SetRow(STZ, Count);
                                Grid.SetColumn(STZ, 1);
                                STZ.Orientation = Orientation.Horizontal;
                                STZ.VerticalAlignment = VerticalAlignment.Center;
                                STZ.Children.Add(lblW);
                                #endregion
                                grid.Children.Add(STZ);
                                Count++;
                            }
                            if (level == "1")
                            {

                                #region 第20部分
                                StackPanel stackA = new StackPanel();
                                TextBlock textA = new TextBlock();
                                textA.Text = " 液      位 ";
                                textA.Style = (Style)user.FindResource("TextBlockDefaultBold");
                                Thickness txtthA = new Thickness(5, 0, 0, 0);
                                textA.Margin = txtthA;
                                textA.VerticalAlignment = VerticalAlignment.Center;
                                Grid.SetRow(stackA, Count);
                                stackA.Orientation = Orientation.Horizontal;
                                Thickness STTHA = new Thickness(5, 0, 0, 0);
                                stackA.Margin = STTHA;
                                stackA.VerticalAlignment = VerticalAlignment.Center;
                                stackA.Children.Add(textA);
                                #endregion
                                grid.Children.Add(stackA);

                                #region 第21部分
                                StackPanel stackB = new StackPanel();
                                Label lblB = new Label();
                                lblB.Name = dev.LiName;
                                lblB.Content = dev.Leveltran+ Levun;
                                Label lblzt = firstfloor.FindName(dev.LiName) as Label;
                                if (lblzt == null)
                                {
                                    firstfloor.RegisterName(dev.LiName, lblB);
                                }
                                else
                                {
                                    firstfloor.Children.Remove(lblzt);
                                    firstfloor.UnregisterName(dev.LiName);
                                    firstfloor.RegisterName(dev.LiName, lblB);
                                }
                                lblB.Style = (Style)user.FindResource("LabelInfo");
                                lblB.VerticalContentAlignment = VerticalAlignment.Center;
                                lblB.Height = height;
                                lblB.Width = width;
                                Grid.SetRow(stackB, Count);
                                Grid.SetColumn(stackB, 1);
                                stackB.Orientation = Orientation.Horizontal;
                                stackB.VerticalAlignment = VerticalAlignment.Center;
                                stackB.Children.Add(lblB);
                                #endregion
                                grid.Children.Add(stackB);
                            }
                           
                            #region 第22部分
                            StackPanel STX = new StackPanel();
                            ProgressBar pro = new ProgressBar();
                            pro.Background = Brushes.LightBlue;
                            pro.Width = 225;
                            pro.Height = 20;
                            pro.Name = dev.PaceName;
                            pro.Value = dev.Pace;
                            ProgressBar PBJD = firstfloor.FindName(dev.PaceName) as ProgressBar;
                            if (PBJD == null)
                            {
                                firstfloor.RegisterName(dev.PaceName, pro);
                            }


                            pro.Style = (Style)user.FindResource("ProgressBarSuccess");
                            Grid.SetRow(STX, 10);
                            Grid.SetColumnSpan(STX, 3);
                            STX.Orientation = Orientation.Horizontal;
                            STX.VerticalAlignment = VerticalAlignment.Center;
                            STX.Children.Add(pro);
                            #endregion
                            grid.Children.Add(STX);
                            #region 第23部分
                            StackPanel STV = new StackPanel();
                            Button butcz = new Button();
                            butcz.Name = dev.OperateName;
                            butcz.Content = "操作";
                            Button butCZ = firstfloor.FindName(dev.OperateName) as Button;
                            if (butCZ == null)
                            {
                                firstfloor.RegisterName(dev.OperateName, butcz);
                            }
                            butcz.Style = (Style)user.FindResource("ButtonInfo");
                            Thickness THButE = new Thickness(1, 0, 0, 0);
                            butcz.Margin = THButE;
                            butcz.Width = 50;

                            Button butjt = new Button();
                            butjt.Name = dev.EmeStopName;
                            butjt.Style = (Style)user.FindResource("ButtonInfo");
                            butjt.Content = "急停";
                            butjt.Command = StopA;
                            //+"," + dev.CraneNO + "," + dev.EmeStopName + "," + dev.AlarmJName;
                            butjt.CommandParameter = dev.IP;
                            Button butJJT = firstfloor.FindName(dev.EmeStopName) as Button;
                            if (butJJT == null)
                            {

                                firstfloor.RegisterName(dev.EmeStopName, butjt);

                            }
                            Thickness butjtE = new Thickness(1, 0, 0, 0);
                            butjt.Margin = butjtE;
                            butcz.Width = 50;
                            Grid.SetRow(STV, 11);
                            Grid.SetColumnSpan(STV, 3);
                            Thickness THE = new Thickness(60, 0, 60, 0);
                            STV.Margin = THE;
                            STV.Orientation = Orientation.Horizontal;
                            STV.VerticalAlignment = VerticalAlignment.Center;
                            STV.Children.Add(butcz);
                            STV.Children.Add(butjt);
                            #endregion
                            grid.Children.Add(STV);
                            #endregion
                            ConList = DeviceService.ConfigList();
                            #region 初始化指示灯
                            Button buttx = new Button();
                            Button butjd = new Button();
                            Button butyy = new Button();
                            Button butyc = new Button();
                            Button butkr = new Button();
                            Button butLA = new Button();
                            Button butql = new Button();
                            Button butqll = new Button();
                            Button butqy = new Button();
                            Button butjjt = new Button();
                            Button butgf = new Button();
                            Button butyg = new Button();
                            Button butqg = new Button();
                            Button butls = new Button();
                            Button butwd = new Button();
                            Button butmd = new Button();
                            Button butyyl = new Button();
                            #endregion
                            Thickness thTX = new Thickness(1, 0, 1, 1);
                            Thickness butTX = new Thickness(2, 0, 0, 0);
                            Thickness BJBJ = new Thickness(0, 2, 0, 0);
                            #region  初始化报警布局
                            StackPanel STbjA = new StackPanel();
                            StackPanel STbjB = new StackPanel();
                            StackPanel STbjC = new StackPanel();
                            StackPanel STbjD = new StackPanel();
                            StackPanel STbjE = new StackPanel();
                            StackPanel STbjF = new StackPanel();
                            StackPanel STbjG = new StackPanel();
                            StackPanel STbjH = new StackPanel();
                            StackPanel STbjJ = new StackPanel();
                            #endregion
                            foreach (SYSConfigModel conf in ConList)
                            {
                                //通讯
                                if (conf.Priority ==0)
                                {

                                    buttx.Name = dev.AlarmAName;
                                    buttx.Content = conf.ConfigName;
                                    Button butcomm = firstfloor.FindName(dev.AlarmAName) as Button;

                                    if (butcomm == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmAName, buttx);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butcomm);
                                        firstfloor.UnregisterName(dev.AlarmAName);
                                        firstfloor.RegisterName(dev.AlarmAName, buttx);
                                    }

                                    buttx.FontSize = 10;
                                    buttx.Padding = thTX;
                                    buttx.Margin = butTX;
                                    //if (dev.AlarmA == 1)
                                    //{
                                    buttx.Style = (Style)user.FindResource("ButtonSuccess");
                                    //}
                                    //else if (dev.AlarmA == 0)
                                    //{
                                    //    buttx.Style = (Style)user.FindResource("ButtonDanger");
                                    //}

                                    buttx.Width = 40;
                                    Grid.SetRow(STbjA, conf.Rowscount);
                                    Grid.SetColumn(STbjA, conf.Collcount);
                                    //STbjA.Margin = BJBJ;
                                    STbjA.Orientation = Orientation.Horizontal;
                                    STbjA.VerticalAlignment = VerticalAlignment.Center;
                                    STbjA.Children.Add(buttx);
                                }
                                //静电
                                else if (conf.Priority ==1)
                                {
                                 
                                    butjd.Name = dev.AlarmBName;
                                    butjd.Content = conf.ConfigName;
                                    butjd.FontSize = 10;
                                    butjd.Padding = thTX;
                                    butjd.Margin = butTX;
                                    Button butstelec = firstfloor.FindName(dev.AlarmBName) as Button;
                                    if (butstelec == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmBName, butjd);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butstelec);
                                        firstfloor.UnregisterName(dev.AlarmBName);
                                        firstfloor.RegisterName(dev.AlarmBName, butjd);
                                    }

                                    if (dev.AlarmB == 1)
                                    {
                                        butjd.Style = (Style)user.FindResource("ButtonSuccess");
                                    }
                                    else if (dev.AlarmB == 0)
                                    {
                                        butjd.Style = (Style)user.FindResource("ButtonDanger");
                                    }
                                    butjd.Style = (Style)user.FindResource("ButtonSuccess");
                                    butjd.Width = 40;
                                    Grid.SetRow(STbjA, conf.Rowscount);
                                    Grid.SetColumn(STbjA, conf.Collcount);
                                    //STbjA.Margin = BJBJ;
                                    STbjA.Orientation = Orientation.Horizontal;
                                    STbjA.VerticalAlignment = VerticalAlignment.Center;
                                    STbjA.Children.Add(butjd);
                                }
                                //溢油
                                else if (conf.Priority == 2)
                                {
                                    butyy.Name = dev.AlarmCName;
                                    butyy.Content = conf.ConfigName;
                                    Button butOil = firstfloor.FindName(dev.AlarmCName) as Button;
                                    if (butOil == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmCName, butyy);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butOil);
                                        firstfloor.UnregisterName(dev.AlarmCName);
                                        firstfloor.RegisterName(dev.AlarmCName, butyy);
                                    }
                                    butyy.Style = (Style)user.FindResource("ButtonSuccess");
                                    butyy.FontSize = 10;
                                    butyy.Padding = thTX;
                                    butyy.Margin = butTX;
                                    butyy.Width = 40;
                                    Grid.SetRow(STbjB, conf.Rowscount);
                                    Grid.SetColumn(STbjB, conf.Collcount);
                                    STbjB.Margin = BJBJ;
                                    STbjB.Orientation = Orientation.Horizontal;
                                    STbjB.VerticalAlignment = VerticalAlignment.Center;
                                    STbjB.Children.Add(butyy);
                                    //butyy.Width = 30;
                                    //butyy.Height = 25;

                                }
                                //钥匙
                                else if (conf.Priority == 3)
                                {
                                    butyc.Name = dev.AlarmDName;
                                    butyc.Content = conf.ConfigName;
                                    Button butyckey = firstfloor.FindName(dev.AlarmDName) as Button;
                                    if (butyckey == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmDName, butyc);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butyckey);
                                        firstfloor.UnregisterName(dev.AlarmDName);
                                        firstfloor.RegisterName(dev.AlarmDName, butyc);
                                    }
                                    butyc.Style = (Style)user.FindResource("ButtonSuccess");
                                    butyc.FontSize = 10;
                                    butyc.Padding = thTX;
                                    butyc.Margin = butTX;
                                    butyc.Width = 40;
                                    Grid.SetRow(STbjB, conf.Rowscount);
                                    Grid.SetColumn(STbjB, conf.Collcount);
                                    STbjB.Margin = BJBJ;
                                    STbjB.Orientation = Orientation.Horizontal;
                                    STbjB.VerticalAlignment = VerticalAlignment.Center;
                                    STbjB.Children.Add(butyc);
                                    //butyc.Width = 30;
                                    //butyc.Height = 25;
                                }
                                //可燃
                                else if (conf.Priority == 4)
                                {
                                    butkr.Name = dev.AlarmEName;
                                    butkr.Content = conf.ConfigName;
                                    Button butcombus = firstfloor.FindName(dev.AlarmEName) as Button;
                                    if (butcombus == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmEName, butkr);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butcombus);
                                        firstfloor.UnregisterName(dev.AlarmEName);
                                        firstfloor.RegisterName(dev.AlarmEName, butkr);
                                    }
                                    butkr.Style = (Style)user.FindResource("ButtonSuccess");
                                    butkr.FontSize = 10;
                                    butkr.Padding = thTX;
                                    butkr.Margin = butTX;
                                    butkr.Width = 40;
                                    Grid.SetRow(STbjC, conf.Rowscount);
                                    Grid.SetColumn(STbjC, conf.Collcount);
                                    STbjC.Margin = BJBJ;
                                    STbjC.Orientation = Orientation.Horizontal;
                                    STbjC.VerticalAlignment = VerticalAlignment.Center;
                                    STbjC.Children.Add(butkr);
                                    //butkr.Width = 30;
                                    //butkr.Height = 25;
                                }
                                //到位
                                else if (conf.Priority == 5)
                                {
                                    butLA.Name = dev.AlarmFName;
                                    butLA.Content = conf.ConfigName;
                                    Button butLiq = firstfloor.FindName(dev.AlarmFName) as Button;
                                    if (butLiq == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmFName, butLA);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butLiq);
                                        firstfloor.UnregisterName(dev.AlarmFName);
                                        firstfloor.RegisterName(dev.AlarmFName, butLA);
                                    }


                                    butLA.Style = (Style)user.FindResource("ButtonSuccess");
                                    butLA.FontSize = 10;
                                    butLA.Padding = thTX;
                                    butLA.Margin = butTX;
                                    butLA.Width = 40;
                                    Grid.SetRow(STbjC, conf.Rowscount);
                                    Grid.SetColumn(STbjC, conf.Collcount);
                                    STbjC.Margin = BJBJ;
                                    STbjC.Orientation = Orientation.Horizontal;
                                    STbjC.VerticalAlignment = VerticalAlignment.Center;
                                    STbjC.Children.Add(butLA);
                                    //butyl.Width = 30;
                                    //butyl.Height = 25;

                                }
                                //归位
                                else if (conf.Priority == 6)
                                {
                                    butql.Name = dev.AlarmGName;
                                    butql.Content = conf.ConfigName;
                                    Button butAir = firstfloor.FindName(dev.AlarmGName) as Button;
                                    if (butAir == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmGName, butql);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butAir);
                                        firstfloor.UnregisterName(dev.AlarmGName);
                                        firstfloor.RegisterName(dev.AlarmGName, butql);
                                    }
                                    butql.Style = (Style)user.FindResource("ButtonSuccess");
                                    butql.FontSize = 10;
                                    butql.Padding = thTX;
                                    butql.Margin = butTX;
                                    butql.Width = 40;
                                    Grid.SetRow(STbjD, conf.Rowscount);
                                    Grid.SetColumn(STbjD, conf.Rowscount);
                                    STbjD.Margin = BJBJ;
                                    STbjD.Orientation = Orientation.Horizontal;
                                    STbjD.VerticalAlignment = VerticalAlignment.Center;
                                    STbjD.Children.Add(butql);
                                }
                                //气流
                                else if (conf.Priority == 7)
                                {
                                    butqll.Name = dev.AlarmHName;
                                    butqll.Content = conf.ConfigName;
                                    Button butairflow = firstfloor.FindName(dev.AlarmHName) as Button;
                                    if (butairflow == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmHName, butqll);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butairflow);
                                        firstfloor.UnregisterName(dev.AlarmHName);
                                        firstfloor.RegisterName(dev.AlarmHName, butqll);
                                    }
                                    butqll.Style = (Style)user.FindResource("ButtonSuccess");
                                    butqll.FontSize = 10;
                                    butqll.Padding = thTX;
                                    butqll.Margin = butTX;
                                    butqll.Width = 40;
                                    Grid.SetRow(STbjD, conf.Rowscount);
                                    Grid.SetColumn(STbjD, conf.Rowscount);
                                    STbjD.Margin = BJBJ;
                                    STbjD.Orientation = Orientation.Horizontal;
                                    STbjD.VerticalAlignment = VerticalAlignment.Center;
                                    STbjD.Children.Add(butqll);
                                }
                                //气溢
                                else if (conf.Priority == 8)
                                {
                                    butqy.Name = dev.AlarmIName;
                                    butqy.Content = conf.ConfigName;


                                    butqy.Style = (Style)user.FindResource("ButtonSuccess");
                                    butqy.FontSize = 10;
                                    butqy.Padding = thTX;
                                    butqy.Margin = butTX;
                                    butqy.Width = 40;
                                    Button butAirover = firstfloor.FindName(dev.AlarmIName) as Button;
                                    if (butAirover == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmIName, butqy);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butAirover);
                                        firstfloor.UnregisterName(dev.AlarmIName);
                                        firstfloor.RegisterName(dev.AlarmIName, butqy);
                                    }



                                    Grid.SetRow(STbjE, conf.Rowscount);
                                    Grid.SetColumn(STbjE, conf.Rowscount);
                                    STbjE.Margin = BJBJ;
                                    STbjE.Orientation = Orientation.Horizontal;
                                    STbjE.VerticalAlignment = VerticalAlignment.Center;
                                    STbjE.Children.Add(butqy);
                                }
                                //急停
                                else if (conf.Priority == 9)
                                {
                                    butjjt.Name = dev.AlarmJName;
                                    butjjt.Content = conf.ConfigName;
                                    Button butEstop = firstfloor.FindName(dev.AlarmJName) as Button;
                                    if (butEstop == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmJName, butjjt);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butEstop);
                                        firstfloor.UnregisterName(dev.AlarmJName);
                                        firstfloor.RegisterName(dev.AlarmJName, butjjt);
                                    }

                                    butjjt.Style = (Style)user.FindResource("ButtonSuccess");
                                    butjjt.FontSize = 10;
                                    butjjt.Padding = thTX;
                                    butjjt.Margin = butTX;
                                    butjjt.Width = 40;

                                    Grid.SetRow(STbjE, conf.Rowscount);
                                    Grid.SetColumn(STbjE, conf.Rowscount);
                                    STbjE.Margin = BJBJ;
                                    STbjE.Orientation = Orientation.Horizontal;
                                    STbjE.VerticalAlignment = VerticalAlignment.Center;
                                    STbjE.Children.Add(butjjt);
                                }
                                //关阀
                                else if (conf.Priority == 10)
                                {
                                    butgf.Name = dev.AlarmKName;
                                    butgf.Content = conf.ConfigName;
                                    Button butgftCS = firstfloor.FindName(dev.AlarmKName) as Button;
                                    if (butgftCS == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmKName, butgf);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butgftCS);
                                        firstfloor.UnregisterName(dev.AlarmKName);
                                        firstfloor.RegisterName(dev.AlarmKName, butgf);
                                    }




                                    butgf.Style = (Style)user.FindResource("ButtonSuccess");
                                    butgf.FontSize = 10;
                                    butgf.Padding = thTX;
                                    butgf.Margin = butTX;
                                    butgf.Width = 40;

                                    Grid.SetRow(STbjF, conf.Rowscount);
                                    Grid.SetColumn(STbjF, conf.Rowscount);
                                    STbjF.Margin = BJBJ;
                                    STbjF.Orientation = Orientation.Horizontal;
                                    STbjF.VerticalAlignment = VerticalAlignment.Center;
                                    STbjF.Children.Add(butgf);
                                }
                                //阻车
                                else if (conf.Priority == 11)
                                {
                                    butyg.Name = dev.AlarmLName;
                                    butyg.Content = conf.ConfigName;
                                    Button butygCS = firstfloor.FindName(dev.AlarmLName) as Button;
                                    if (butygCS == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmLName, butyg);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butygCS);
                                        firstfloor.UnregisterName(dev.AlarmLName);
                                        firstfloor.RegisterName(dev.AlarmLName, butyg);
                                    }

                                    butyg.Style = (Style)user.FindResource("ButtonSuccess");
                                    butyg.FontSize = 10;
                                    butyg.Padding = thTX;
                                    butyg.Margin = butTX;
                                    butyg.Width = 40;
                                    Grid.SetRow(STbjF, conf.Rowscount);
                                    Grid.SetColumn(STbjF, conf.Rowscount);
                                    STbjF.Margin = BJBJ;
                                    STbjF.Orientation = Orientation.Horizontal;
                                    STbjF.VerticalAlignment = VerticalAlignment.Center;
                                    STbjF.Children.Add(butyg);
                                }
                                //在岗
                                else if (conf.Priority == 12)
                                {
                                    butqg.Name = dev.AlarmQName;
                                    butqg.Content = conf.ConfigName;
                                    Button butqgCS = firstfloor.FindName(dev.AlarmQName) as Button;
                                    if (butqgCS == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmQName, butqg);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butqgCS);
                                        firstfloor.UnregisterName(dev.AlarmQName);
                                        firstfloor.RegisterName(dev.AlarmQName, butqg);
                                    }


                                    butqg.Style = (Style)user.FindResource("ButtonSuccess");
                                    butqg.FontSize = 10;
                                    butqg.Padding = thTX;
                                    butqg.Margin = butTX;
                                    butqg.Width = 40;
                                    Grid.SetRow(STbjG, conf.Rowscount);
                                    Grid.SetColumn(STbjG, conf.Rowscount);
                                    STbjG.Margin = BJBJ;
                                    STbjG.Orientation = Orientation.Horizontal;
                                    STbjG.VerticalAlignment = VerticalAlignment.Center;
                                    STbjG.Children.Add(butqg);
                                }
                                //零速
                                else if (conf.Priority == 13)
                                {
                                    butls.Name = dev.AlarmWName;
                                    butls.Content = conf.ConfigName;
                                    Button butlsCS = firstfloor.FindName(dev.AlarmWName) as Button;
                                    if (butlsCS == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmWName, butls);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butlsCS);
                                        firstfloor.UnregisterName(dev.AlarmWName);
                                        firstfloor.RegisterName(dev.AlarmWName, butls);
                                    }



                                    butls.Style = (Style)user.FindResource("ButtonSuccess");
                                    butls.FontSize = 10;
                                    butls.Padding = thTX;
                                    butls.Margin = butTX;
                                    butls.Width = 40;
                                    Grid.SetRow(STbjG, conf.Rowscount);
                                    Grid.SetColumn(STbjG, conf.Rowscount);
                                    STbjG.Margin = BJBJ;
                                    STbjG.Orientation = Orientation.Horizontal;
                                    STbjG.VerticalAlignment = VerticalAlignment.Center;
                                    STbjG.Children.Add(butls);
                                }
                                //泵
                                else if (conf.Priority == 14)
                                {
                                    butwd.Name = dev.AlarmRName;
                                    butwd.Content = conf.ConfigName;
                                    Button butwdCS = firstfloor.FindName(dev.AlarmRName) as Button;
                                    if (butwdCS == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmRName, butwd);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butwdCS);
                                        firstfloor.UnregisterName(dev.AlarmRName);
                                        firstfloor.RegisterName(dev.AlarmRName, butwd);
                                    }

                                    butwd.Style = (Style)user.FindResource("ButtonSuccess");
                                    butwd.FontSize = 10;
                                    butwd.Padding = thTX;
                                    butwd.Margin = butTX;
                                    butwd.Width = 40;
                                    Grid.SetRow(STbjH, conf.Rowscount);
                                    Grid.SetColumn(STbjH, conf.Rowscount);
                                    STbjH.Margin = BJBJ;
                                    STbjH.Orientation = Orientation.Horizontal;
                                    STbjH.VerticalAlignment = VerticalAlignment.Center;
                                    STbjH.Children.Add(butwd);
                                }
                                //阀
                                else if (conf.Priority == 15)
                                {
                                    butmd.Name = dev.AlarmTName;
                                    butmd.Content = conf.ConfigName;
                                    Button butmdCS = firstfloor.FindName(dev.AlarmTName) as Button;
                                    if (butmdCS == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmTName, butmd);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butmdCS);
                                        firstfloor.UnregisterName(dev.AlarmTName);
                                        firstfloor.RegisterName(dev.AlarmTName, butmd);
                                    }


                                    butmd.Style = (Style)user.FindResource("ButtonSuccess");
                                    butmd.FontSize = 10;
                                    butmd.Padding = thTX;
                                    butmd.Margin = butTX;
                                    butmd.Width = 40;
                                    Grid.SetRow(STbjH, conf.Rowscount);
                                    Grid.SetColumn(STbjH, conf.Rowscount);
                                    STbjH.Margin = BJBJ;
                                    STbjH.Orientation = Orientation.Horizontal;
                                    STbjH.VerticalAlignment = VerticalAlignment.Center;
                                    STbjH.Children.Add(butmd);
                                }
                                //备用
                                else if (conf.Priority == 16)
                                {
                                    butyyl.Name = dev.AlarmOName;
                                    butyyl.Content = conf.ConfigName;
                                    Button butyylCS = firstfloor.FindName(dev.AlarmOName) as Button;
                                    if (butyylCS == null)
                                    {
                                        firstfloor.RegisterName(dev.AlarmOName, butyyl);
                                    }
                                    else
                                    {
                                        firstfloor.Children.Remove(butyylCS);
                                        firstfloor.UnregisterName(dev.AlarmOName);
                                        firstfloor.RegisterName(dev.AlarmOName, butyyl);
                                    }
                                    butyyl.Style = (Style)user.FindResource("ButtonSuccess");
                                    butyyl.FontSize = 10;
                                    butyyl.Padding = thTX;
                                    butyyl.Margin = butTX;
                                    butyyl.Width = 40;
                                    Grid.SetRow(STbjJ, conf.Rowscount);
                                    Grid.SetColumn(STbjJ, conf.Rowscount);
                                    STbjJ.Margin = BJBJ;
                                    STbjJ.Orientation = Orientation.Horizontal;
                                    STbjJ.VerticalAlignment = VerticalAlignment.Center;
                                    STbjJ.Children.Add(butyyl);
                                }

                                ConMoList.Add(conf);
                            }
                            grid.Children.Add(STbjA);
                            grid.Children.Add(STbjB);
                            grid.Children.Add(STbjC);
                            grid.Children.Add(STbjD);
                            grid.Children.Add(STbjE);
                            grid.Children.Add(STbjF);
                            grid.Children.Add(STbjG);
                            grid.Children.Add(STbjH);
                            grid.Children.Add(STbjJ);
                            tabItem.Content = grid;
                            tab.Items.Add(tabItem);
                            firstfloor.Children.Add(tab);
                            TabControl tabA = firstfloor.FindName(dev.MaName) as TabControl;
                            TabItem tabitemA = firstfloor.FindName(dev.CrName) as TabItem;
                            if (tabA == null && tabitemA == null)
                            {
                                firstfloor.RegisterName(dev.MaName, tab);
                                firstfloor.RegisterName(dev.CrName, tabItem);
                            }
                            countA++;
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                LogerHelper.Error("Calc:" + ex);
                MessageBox.Show("ERROR："+ex.ToString(),"温馨提示",MessageBoxButton.OK,MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
         

        }
        public static void Deinitialization(UserControl user, Grid firstfloor, int pages, int end)
        {
            for (int i = pages; i <= end; i++)
            {
                SYSDeviceModel mo = new SYSDeviceModel();
                int Actnum = setModelsList.Count - 1;
                if (i <= Actnum)
                {
                    mo.MachineNo = setModelsList[i].MachineNo;
                    mo.CraneNO = setModelsList[i].CraneNO;
                }
                foreach (SYSDeviceModel dev in DeviceService.deviceList(mo, 1))
                {

                  
                    Button comm = firstfloor.FindName(dev.AlarmAName) as Button;
                    if (comm != null)
                    {
                        firstfloor.Children.Remove(comm);
                        firstfloor.UnregisterName(dev.AlarmAName);
                    }
                    //静电
                    Button Man = firstfloor.FindName(dev.AlarmBName) as Button;
                    if (Man != null)
                    {
                        firstfloor.Children.Remove(Man);
                        firstfloor.UnregisterName(dev.AlarmBName);
                    }
                    //溢油
                    Button Oil = firstfloor.FindName(dev.AlarmCName) as Button;
                    if (Oil != null)
                    {
                        firstfloor.Children.Remove(Oil);
                        firstfloor.UnregisterName(dev.AlarmCName);
                    }
                    //钥匙
                    Button key = firstfloor.FindName(dev.AlarmDName) as Button;
                    if (key != null)
                    {
                        firstfloor.Children.Remove(key);
                        firstfloor.UnregisterName(dev.AlarmDName);
                    }
                    //可燃
                    Button combus = firstfloor.FindName(dev.AlarmEName) as Button;
                    if (combus != null)
                    {
                        firstfloor.Children.Remove(combus);
                        firstfloor.UnregisterName(dev.AlarmEName);
                    }
                    //液连
                    Button fluid = firstfloor.FindName(dev.AlarmFName) as Button;
                    if (fluid != null)
                    {
                        firstfloor.Children.Remove(fluid);
                        firstfloor.UnregisterName(dev.AlarmFName);
                    }
                    //气连
                    Button air = firstfloor.FindName(dev.AlarmGName) as Button;
                    if (air != null)
                    {
                        firstfloor.Children.Remove(air);
                        firstfloor.UnregisterName(dev.AlarmGName);
                    }
                    //气流
                    Button airflow = firstfloor.FindName(dev.AlarmHName) as Button;
                    if (airflow != null)
                    {
                        firstfloor.Children.Remove(airflow);
                        firstfloor.UnregisterName(dev.AlarmHName);
                    }
                    //气溢
                    Button Gas = firstfloor.FindName(dev.AlarmIName) as Button;
                    if (Gas != null)
                    {
                        firstfloor.Children.Remove(Gas);
                        firstfloor.UnregisterName(dev.AlarmIName);
                    }
                    //急停
                    Button Estop = firstfloor.FindName(dev.AlarmJName) as Button;
                    if (Estop != null)
                    {
                        firstfloor.Children.Remove(Estop);
                        firstfloor.UnregisterName(dev.AlarmJName);
                    }
                    //关阀
                    Button Close = firstfloor.FindName(dev.AlarmKName) as Button;
                    if (Close != null)
                    {
                        firstfloor.Children.Remove(Close);
                        firstfloor.UnregisterName(dev.AlarmKName);
                    }
                    //液归
                    Button Liquid = firstfloor.FindName(dev.AlarmLName) as Button;
                    if (Liquid != null)
                    {
                        firstfloor.Children.Remove(Liquid);
                        firstfloor.UnregisterName(dev.AlarmLName);
                    }
                    //气归
                    Button QiGUI = firstfloor.FindName(dev.AlarmQName) as Button;
                    if (QiGUI != null)
                    {
                        firstfloor.Children.Remove(QiGUI);
                        firstfloor.UnregisterName(dev.AlarmQName);
                    }
                    //零速
                    Button Zero = firstfloor.FindName(dev.AlarmWName) as Button;
                    if (Zero != null)
                    {
                        firstfloor.Children.Remove(Zero);
                        firstfloor.UnregisterName(dev.AlarmWName);
                    }
                    //温度
                    Button temper = firstfloor.FindName(dev.AlarmRName) as Button;
                    if (temper != null)
                    {
                        firstfloor.Children.Remove(temper);
                        firstfloor.UnregisterName(dev.AlarmRName);
                    }
                    //密度
                    Button density = firstfloor.FindName(dev.AlarmTName) as Button;
                    if (density != null)
                    {
                        firstfloor.Children.Remove(density);
                        firstfloor.UnregisterName(dev.AlarmTName);
                    }
                    //压力
                    Button pressure = firstfloor.FindName(dev.AlarmOName) as Button;
                    if (pressure != null)
                    {
                        firstfloor.Children.Remove(pressure);
                        firstfloor.UnregisterName(dev.AlarmOName);
                    }
                    ProgressBar PaceName = firstfloor.FindName(dev.PaceName) as ProgressBar;
                    if (PaceName != null)
                    {
                        firstfloor.Children.Remove(PaceName);
                        firstfloor.UnregisterName(dev.PaceName);
                    }
                    Button butCZ = firstfloor.FindName(dev.OperateName) as Button;
                    if (butCZ != null)
                    {
                        firstfloor.Children.Remove(butCZ);
                        firstfloor.UnregisterName(dev.OperateName);
                    }

                    Button butStop = firstfloor.FindName(dev.EmeStopName) as Button;
                    if (butStop != null)
                    {
                        firstfloor.Children.Remove(butStop);
                        firstfloor.UnregisterName(dev.EmeStopName);
                    }
                    Label Granestatus = firstfloor.FindName(dev.StatusName) as Label;
                    if (Granestatus != null)
                    {
                        firstfloor.Children.Remove(Granestatus);
                        firstfloor.UnregisterName(dev.StatusName);
                    }

                    Label OrName = firstfloor.FindName(dev.OrName) as Label;
                    if (OrName != null)
                    {
                        firstfloor.Children.Remove(OrName);
                        firstfloor.UnregisterName(dev.OrName);
                    }

                    Label PVName = firstfloor.FindName(dev.PVName) as Label;
                    if (PVName != null)
                    {
                        firstfloor.Children.Remove(PVName);
                        firstfloor.UnregisterName(dev.PVName);
                    }

                    Label AIName = firstfloor.FindName(dev.AIName) as Label;
                    if (AIName != null)
                    {
                        firstfloor.Children.Remove(AIName);
                        firstfloor.UnregisterName(dev.AIName);
                    }

                    Label ITName = firstfloor.FindName(dev.ITName) as Label;
                    if (ITName != null)
                    {
                        firstfloor.Children.Remove(ITName);
                        firstfloor.UnregisterName(dev.ITName);
                    }

                    Label AName = firstfloor.FindName(dev.AName) as Label;
                    if (AName != null)
                    {
                        firstfloor.Children.Remove(AName);
                        firstfloor.UnregisterName(dev.AName);
                    }

                    Label TName = firstfloor.FindName(dev.TName) as Label;
                    if (TName != null)
                    {
                        firstfloor.Children.Remove(TName);
                        firstfloor.UnregisterName(dev.TName);
                    }

                    Label DName = firstfloor.FindName(dev.DName) as Label;
                    if (DName != null)
                    {
                        firstfloor.Children.Remove(DName);
                        firstfloor.UnregisterName(dev.DName);
                    }

                    Label PName = firstfloor.FindName(dev.PName) as Label;
                    if (PName != null)
                    {
                        firstfloor.Children.Remove(PName);
                        firstfloor.UnregisterName(dev.PName);
                    }
                    TabControl tabA = firstfloor.FindName(dev.MaName) as TabControl;
                    if (tabA != null)
                    {
                        firstfloor.Children.Remove(tabA);
                        firstfloor.UnregisterName(dev.MaName);
                    }

                    TabItem itemA = firstfloor.FindName(dev.CrName) as TabItem;
                    if (itemA != null)
                    {
                        firstfloor.Children.Remove(itemA);
                        firstfloor.UnregisterName(dev.CrName);
                    }

                }
            }
        }
    
        public PLCMonViewModels(UserControl User, Grid EquMon, Button leftSwitch, Button rightSwitch, int pages, int end, int buytype)
        {
            try
            {
                Crposlayout = EquMon;
                PLCMonViewModels.PageCalc(User, EquMon, leftSwitch, rightSwitch, 0, 11, 1);
                StartPage=0;
                EndPage = 11;
                DeviceCollection(StartPage, EndPage);
                FirstGroupDevice(User, EquMon, leftSwitch, rightSwitch);


            }
            catch (Exception ex) 
            {
                LogerHelper.Error("MonViewModels:",ex);
                MessageBox.Show("通讯故障，"+ex.ToString(),"温馨提示",MessageBoxButton.OK,MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
       

        private static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        private static List<SYSDeviceModel> devmList;
        private void DeviceCollection(int StartPage,int EndPage) 
        {
            devmList = new List<SYSDeviceModel>();
            for (int i = StartPage; i <= EndPage; i++) 
            {
                SYSDeviceModel mo = new SYSDeviceModel();
                int Actnum = setModelsList.Count - 1;
                if (i <= Actnum)
                {
                    mo.MachineNo = setModelsList[i].MachineNo;
                    mo.CraneNO = setModelsList[i].CraneNO;
                    devmList.Add(mo);
                }
            }
        }

        public static void FirstGroupDevice(UserControl User, Grid EquMon, Button leftSwitch, Button rightSwitch) 
        {
            FirstGroupTask = Task.Run(async() =>
            {
                    while (isRunning)
                    {
            
                    foreach (SYSDeviceModel mo in devmList)
                        {
                            foreach (SYSDeviceModel dev in DeviceService.deviceList(mo, 1))
                            {
                                Ping pingSender = new Ping();
                                PingReply reply;
                                reply = pingSender.Send(dev.IP, 100);
                                if (reply.Status == IPStatus.Success)
                                {
                                    ReadAlarmInfo(User, EquMon, dev);
                                }
                                else
                                {
                                CommError(EquMon, dev);
                                    continue;
                                }
                            }
                        await Task.Delay(100);
                    }
                    }
            });
        }

        public static void FirstGroupDispose()
        {
            isRunning = false;
            //if (FirstGroupTask != null)
            //    FirstGroupTask.ConfigureAwait(false);

        }

        private static Dictionary<int, bool> error = new Dictionary<int, bool>();

        private static void ReadAlarmInfo(UserControl user, Grid firstfloor,SYSDeviceModel dev)
        {
            Plc plc = new Plc(CpuType.S7200Smart, dev.IP, 0, 1);
            try
            {
                plc.Open();
                if (dev.CraneNO == "A")
                {
                    //静电
                    var  elecA= plc.Read("M20.0");
                    //溢油
                    var oilSpiA = plc.Read("M20.1");
                    //钥匙
                    var keyA= plc.Read("M20.2");
                    //可燃
                    var combusA = plc.Read("M20.3");
                    //到位
                    var InplaceA = plc.Read("M20.4");
                    //归位
                    var HomingA = plc.Read("M20.5");
                    //气流
                    var AirflowA = plc.Read("M20.6");
                    //气溢
                    var AerorrheaA = plc.Read("M20.7");
                    //急停
                    var StopA = plc.Read("M21.0");
                    //关阀
                    var ValveshutA= plc.Read("M18.0");
                    //阻车器
                    var TrajamA = plc.Read("M21.2");
                    //人员在岗
                    var OndutyA = plc.Read("M21.3");
                    //零速
                    var ZerospeA = plc.Read("M21.4");
                    //温度
                    var heatA = plc.Read("M21.5");
                    //压力
                    var presA = plc.Read("M21.6");
                    //密度
                    var densityA= plc.Read("M21.7");
                    //液位
                    var levelA = plc.Read("M21.1");
                    //远程急停
                    var EMStop = plc.Read("Q2.0");
                    //A阀开到位
                    var ValveopenA = plc.Read("M31.0");
                    //A阀关到位
                    var ValveoffA = plc.Read("M31.1");
                    //A泵运行反馈
                    var PumpfeedbA = plc.Read("M31.2");
                    Task.Factory.StartNew(() => AlarmInfo(firstfloor,dev, elecA, oilSpiA, keyA, combusA, InplaceA, HomingA, AirflowA, AerorrheaA, StopA, EMStop, ValveshutA, TrajamA, OndutyA, ZerospeA, heatA, presA, densityA, levelA, ValveopenA, ValveoffA, PumpfeedbA),
new CancellationTokenSource().Token, TaskCreationOptions.None, _syncContextTaskScheduler).Wait();
                    //A实时温度
                    var heataA = plc.Read(DataType.DataBlock, 1, 84, VarType.Real, 1); 
                    //A实时压力
                    var pressA = plc.Read(DataType.DataBlock, 1, 88, VarType.Real, 1);
                    //A实时密度
                    var densA = plc.Read(DataType.DataBlock, 1, 92, VarType.Real, 1);
                    //A实时液位(小数点保留两位)
                    var RealevelA = plc.Read(DataType.DataBlock, 1, 96, VarType.Real, 1);
                    //鹤位状态
                    var status = plc.Read("DB1.DBB0");
                    //卡号
                    var CardID = plc.Read("DB1.DBD2");
                    //预装量
                    var PlannedVolume = plc.Read("DB1.DBD6");
                    //实装量
                    var AmountInstall = plc.Read("DB1.DBD10");
                    //瞬时流量--保留一个小数点
                    var InsTraffic = plc.Read(DataType.DataBlock, 1, 22, VarType.Int, 1);
                    //累计流量
                    var Accumulate = plc.Read(DataType.DataBlock, 1, 18, VarType.DWord, 1);
                    //温度
                    var Temperature = plc.Read(DataType.DataBlock, 1, 84, VarType.Real, 1);
                    //密度
                    var Denser = plc.Read(DataType.DataBlock, 1, 92, VarType.Real, 1);
                    //压力
                    var Pressure = plc.Read(DataType.DataBlock, 1, 88, VarType.Real, 1);
                    //液位
                    var levelInfo = plc.Read(DataType.DataBlock, 1, 96, VarType.Real, 1);
                    Task.Factory.StartNew(() => LoadingInfo(firstfloor, dev, status, CardID, PlannedVolume, AmountInstall, InsTraffic, Accumulate, Temperature, Denser, Pressure, levelInfo),
new CancellationTokenSource().Token, TaskCreationOptions.None, _syncContextTaskScheduler).Wait();
                }
                else if (dev.CraneNO == "B") 
                {
                    //静电
                    var elecB = plc.Read("M22.0");
                    //溢油
                    var oilSpiB = plc.Read("M22.1");
                    //钥匙
                    var keyB = plc.Read("M22.2");
                    //可燃
                    var combusB = plc.Read("M22.3");
                    //到位
                    var InplaceB = plc.Read("M22.4");
                    //归位
                    var HomingB = plc.Read("M22.5");
                    //气流
                    var AirflowB = plc.Read("M22.6");
                    //气溢
                    var AerorrheaB = plc.Read("M22.7");
                    //急停
                    var StopB = plc.Read("M23.0");
                    //关阀
                    var ValveshutB = plc.Read("M19.0");
                    //阻车器
                    var TrajamB = plc.Read("M23.2");
                    //人员在岗
                    var OndutyB = plc.Read("M23.3");
                    //零速
                    var ZerospeB = plc.Read("M23.4");
                    //温度
                    var heatB = plc.Read("M23.5");
                    //压力
                    var presB = plc.Read("M23.6");
                    //密度
                    var densityB = plc.Read("M23.7");
                    //液位
                    var levelB = plc.Read("M23.1");
                    //远程急停
                    var EMStop = plc.Read("Q2.0");
                    //B阀开到位
                    var ValveopenB = plc.Read("M31.4");
                    //B阀关到位
                    var ValveoffB = plc.Read("M31.5");
                    //B泵运行反馈
                    var PumpfeedbB = plc.Read("M31.6");
                    Task.Factory.StartNew(() => AlarmInfo(firstfloor, dev, elecB, oilSpiB, keyB, combusB, InplaceB, HomingB, AirflowB, AerorrheaB, StopB, EMStop, ValveshutB, TrajamB, OndutyB, ZerospeB, heatB, presB, densityB, levelB, ValveopenB, ValveoffB, PumpfeedbB),
new CancellationTokenSource().Token, TaskCreationOptions.None, _syncContextTaskScheduler).Wait();
                    //B实时温度
                    var heataB = plc.Read(DataType.DataBlock, 1, 184, VarType.Real, 1);
                    //B实时压力
                    var pressB = plc.Read(DataType.DataBlock, 1, 188, VarType.Real, 1);
                    //B实时密度
                    var densB = plc.Read(DataType.DataBlock, 1, 192, VarType.Real, 1);
                    //B实时液位(小数点保留两位)
                    var RealevelB = plc.Read(DataType.DataBlock, 1, 196, VarType.Real, 1);
                    var status = plc.Read("DB1.DBB100");
                    var CardID = plc.Read("DB1.DBD102");
                    var PlannedVolume = plc.Read("DB1.DBD106");
                    var AmountInstall = plc.Read("DB1.DBD110");
                    var InsTraffic = plc.Read(DataType.DataBlock, 1, 122, VarType.Int, 1);
                    var Accumulate = plc.Read(DataType.DataBlock, 1, 118, VarType.DWord, 1);
                    var Temperature = plc.Read(DataType.DataBlock, 1, 184, VarType.Real, 1);
                    var Denser = plc.Read(DataType.DataBlock, 1, 192, VarType.Real, 1);
                    var Pressure = plc.Read(DataType.DataBlock, 1, 188, VarType.Real, 1);
                    var levelInfo = plc.Read(DataType.DataBlock, 1, 196, VarType.Real, 1);
                    Task.Factory.StartNew(() => LoadingInfo(firstfloor, dev, status, CardID, PlannedVolume, AmountInstall, InsTraffic, Accumulate, Temperature, Denser, Pressure, levelInfo),
new CancellationTokenSource().Token, TaskCreationOptions.None, _syncContextTaskScheduler).Wait();
                }
            }
            catch (Exception ex)
            {
                CommError(firstfloor, dev);
                LogerHelper.Error("ReadAlarmInfo:", ex);
                isRunning = true;
                return;
            }
            finally
            {
                plc.Close();
            }
        }
        
        private static void AlarmInfo(Grid firstfloor, SYSDeviceModel  dev,object   StaEcel, object OilSpill, object Key, object ComBus, object InPlace, object Homing, object Airflow, object Aerorrhea, object Stop,object EmStop ,object CloseValve, object CarStopper, object PerDuty, object Zerospeed, object temp, object
Density, object Pressure, object level, object ValveOpen, object ValveClose, object ValStatus)
        {
            //通讯
            Button Comm = firstfloor.FindName(dev.AlarmAName) as Button;
            if (Comm != null)
            {
                Comm.Style = (Style)firstfloor.FindResource("ButtonSuccess");
            }
            //静电
            Button Man = firstfloor.FindName(dev.AlarmBName) as Button;
            //溢油
            Button Oil = firstfloor.FindName(dev.AlarmCName) as Button;
            //钥匙
            Button key = firstfloor.FindName(dev.AlarmDName) as Button;
            //可燃
            Button combus = firstfloor.FindName(dev.AlarmEName) as Button;
            //到位
            Button InPla = firstfloor.FindName(dev.AlarmFName) as Button;
            //归位
            Button air = firstfloor.FindName(dev.AlarmGName) as Button;
            //气流
            Button airflow = firstfloor.FindName(dev.AlarmHName) as Button;
            //气溢
            Button Gas = firstfloor.FindName(dev.AlarmIName) as Button;
            //急停
            Button Stopbu = firstfloor.FindName(dev.AlarmJName) as Button;
            //关阀
            Button Close = firstfloor.FindName(dev.AlarmKName) as Button;
            //阻车
            Button Liquid = firstfloor.FindName(dev.AlarmLName) as Button;
            //人员在岗
            Button QiGUI = firstfloor.FindName(dev.AlarmQName) as Button;
            //零速
            Button Zero = firstfloor.FindName(dev.AlarmWName) as Button;
            if (Man!=null )
                {
                AlarmInfoExec(firstfloor, Man, StaEcel, OilSpill, Key, ComBus, InPlace, Homing, Airflow, Aerorrhea, Stop, EmStop, CloseValve, CarStopper, PerDuty, Zerospeed, temp,
Density, Pressure, level, ValveOpen, ValveClose, ValStatus);
                }
            if (Oil!=null )
                {
                AlarmInfoExec(firstfloor, Oil, StaEcel, OilSpill, Key, ComBus, InPlace, Homing, Airflow, Aerorrhea, Stop, EmStop, CloseValve, CarStopper, PerDuty, Zerospeed, temp,
Density, Pressure, level, ValveOpen, ValveClose, ValStatus);
                }
            if (key!=null )
                {
                AlarmInfoExec(firstfloor, key, StaEcel, OilSpill, Key, ComBus, InPlace, Homing, Airflow, Aerorrhea, Stop, EmStop, CloseValve, CarStopper, PerDuty, Zerospeed, temp,
Density, Pressure, level, ValveOpen, ValveClose, ValStatus);
                }
            if (combus!=null)
                {
                AlarmInfoExec(firstfloor, combus, StaEcel, OilSpill, Key, ComBus, InPlace, Homing, Airflow, Aerorrhea, Stop, EmStop, CloseValve, CarStopper, PerDuty, Zerospeed, temp,
Density, Pressure, level, ValveOpen, ValveClose, ValStatus);
                }
            if (InPla!=null )
                {
                AlarmInfoExec(firstfloor, InPla, StaEcel, OilSpill, Key, ComBus, InPlace, Homing, Airflow, Aerorrhea, Stop, EmStop, CloseValve, CarStopper, PerDuty, Zerospeed, temp,
Density, Pressure, level, ValveOpen, ValveClose, ValStatus);
                }
            if (air!=null  )
                {
                AlarmInfoExec(firstfloor, air, StaEcel, OilSpill, Key, ComBus, InPlace, Homing, Airflow, Aerorrhea, Stop, EmStop, CloseValve, CarStopper, PerDuty, Zerospeed, temp,
Density, Pressure, level, ValveOpen, ValveClose, ValStatus);
                }
            if (airflow!=null )
                {
                AlarmInfoExec(firstfloor, airflow, StaEcel, OilSpill, Key, ComBus, InPlace, Homing, Airflow, Aerorrhea, Stop, EmStop, CloseValve, CarStopper, PerDuty, Zerospeed, temp,
Density, Pressure, level, ValveOpen, ValveClose, ValStatus);
                }
            if (Gas!=null )
                {
                AlarmInfoExec(firstfloor, Gas, StaEcel, OilSpill, Key, ComBus, InPlace, Homing, Airflow, Aerorrhea, Stop, EmStop, CloseValve, CarStopper, PerDuty, Zerospeed, temp,
Density, Pressure, level, ValveOpen, ValveClose, ValStatus);
                }
            if (Stopbu!=null )
                {
                AlarmInfoExec(firstfloor, Stopbu, StaEcel, OilSpill, Key, ComBus, InPlace, Homing, Airflow, Aerorrhea, Stop, EmStop, CloseValve, CarStopper, PerDuty, Zerospeed, temp,
Density, Pressure, level, ValveOpen, ValveClose, ValStatus);
                }
            if (Close!=null )
                {
                AlarmInfoExec(firstfloor, Close, StaEcel, OilSpill, Key, ComBus, InPlace, Homing, Airflow, Aerorrhea, Stop, EmStop, CloseValve, CarStopper, PerDuty, Zerospeed, temp,
Density, Pressure, level, ValveOpen, ValveClose, ValStatus);
                }
            if (Liquid!=null) 
                {
                AlarmInfoExec(firstfloor, Liquid, StaEcel, OilSpill, Key, ComBus, InPlace, Homing, Airflow, Aerorrhea, Stop, EmStop, CloseValve, CarStopper, PerDuty, Zerospeed, temp,
Density, Pressure, level, ValveOpen, ValveClose, ValStatus);
                }
            if (QiGUI!=null )
                {
                AlarmInfoExec(firstfloor, QiGUI, StaEcel, OilSpill, Key, ComBus, InPlace, Homing, Airflow, Aerorrhea, Stop, EmStop, CloseValve, CarStopper, PerDuty, Zerospeed, temp,
Density, Pressure, level, ValveOpen, ValveClose, ValStatus);
                }
            if (Zero!=null)
                {
                AlarmInfoExec(firstfloor, Zero, StaEcel, OilSpill, Key, ComBus, InPlace, Homing, Airflow, Aerorrhea, Stop, EmStop, CloseValve, CarStopper, PerDuty, Zerospeed, temp,
Density, Pressure, level, ValveOpen, ValveClose, ValStatus);
                }
            //远程急停
            Button butJJT = firstfloor.FindName(dev.EmeStopName) as Button;
            if (butJJT != null) 
            {
                if (Convert.ToBoolean(EmStop) == false)
                {
                    butJJT.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(EmStop) == true)
                {
                    butJJT.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }
            }
            //温度
            Label lblWD = firstfloor.FindName(dev.TName) as Label;
            if (lblWD != null)
            {
                if (Convert.ToBoolean(temp) == true)
                {
                    lblWD.Style = (Style)firstfloor.FindResource("LabelDanger");
                }
                else if (Convert.ToBoolean(temp) == false)
                {
                    lblWD.Style = (Style)firstfloor.FindResource("LabelInfo");
                }
            }
            //密度
            Label lblMD = firstfloor.FindName(dev.DName) as Label;
            if (lblMD != null) 
            {
                if (Convert.ToBoolean(Density) == true)
                {
                    lblMD.Style = (Style)firstfloor.FindResource("LabelDanger");
                } 
                else if (Convert.ToBoolean(Density) == false) 
                {
                    lblMD.Style = (Style)firstfloor.FindResource("LabelInfo");
                }
            }
            //压力
            Label lblYL = firstfloor.FindName(dev.PName) as Label;
            if (lblYL != null) 
            {
                if (Convert.ToBoolean(Pressure) == true)
                {
                    lblYL.Style = (Style)firstfloor.FindResource("LabelDanger");
                } 
                else if (Convert.ToBoolean(Pressure)==false) 
                {
                    lblYL.Style = (Style)firstfloor.FindResource("LabelInfo");
                }
            }
            //液位
            Label lblzt = firstfloor.FindName(dev.LiName) as Label;
            if (lblzt != null) 
            {
                if (Convert.ToBoolean(level) == true)
                {
                    lblzt.Style = (Style)firstfloor.FindResource("LabelDanger");
                }
                else if (Convert.ToBoolean(level) == false)
                {
                    lblzt.Style = (Style)firstfloor.FindResource("LabelInfo");
                }
            }
            //泵
            Button density = firstfloor.FindName(dev.AlarmRName) as Button;
            if (density != null)
            {
                if (Convert.ToBoolean(ValStatus) == true)
                {
                    density.Style = (Style)firstfloor.FindResource("ButtonSuccess");

                }
                else if (Convert.ToBoolean(ValStatus) == false)
                {
                    density.Style = (Style)firstfloor.FindResource("ButtonBaseBaseStyle");
                }
            }
            //阀         
            Button temper = firstfloor.FindName(dev.AlarmTName) as Button;
            if (temper != null)
            {

                if (Convert.ToBoolean(ValveOpen) == false && Convert.ToBoolean(ValveClose) == false)
                {
                    temper.Style = (Style)firstfloor.FindResource("ButtonBaseBaseStyle");
                }
                else if (Convert.ToBoolean(ValveClose) == true)
                {
                    temper.Style = (Style)firstfloor.FindResource("ButtonDanger");
                } 
                else if (Convert.ToBoolean(ValveOpen) == true) 
                {
                    temper.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

             
            }
        }


        private static void AlarmInfoExec(Grid firstfloor, Button  Alar,object StaEcel, object OilSpill, object Key, object ComBus, object InPlace, object Homing, object Airflow, object Aerorrhea, object Stop, object EmStop, object CloseValve, object CarStopper, object PerDuty, object Zerospeed, object temp, object
Density, object Pressure, object level, object ValveOpen, object ValveClose, object ValStatus) 
        {
            if (Alar.Content.ToString() == "静电")
            {
                if (Convert.ToBoolean(StaEcel) == false)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(StaEcel) == true)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }

            }
            else if (Alar.Content.ToString() == "溢油")
            {
                if (Convert.ToBoolean(OilSpill) == false)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(OilSpill) == true)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }

            }
            else if (Alar.Content.ToString() == "钥匙")
            {
                if (Convert.ToBoolean(Key) == false)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(Key) == true)
                {

                    Alar.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }

            }
            else if (Alar.Content.ToString() == "可燃")
            {
                if (Convert.ToBoolean(ComBus) == false)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(ComBus) == true)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }

            }
            else if (Alar.Content.ToString() == "到位")
            {
                if (Convert.ToBoolean(InPlace) == false)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(InPlace) == true)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }

            }
            else if (Alar.Content.ToString() == "归位")
            {
                if (Convert.ToBoolean(Homing) == false)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(Homing) == true)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }

            }
            else if (Alar.Content.ToString() == "气流")
            {
                if (Convert.ToBoolean(Airflow) == false)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(Airflow) == true)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }

            }
            else if (Alar.Content.ToString() == "气溢")
            {
                if (Convert.ToBoolean(Aerorrhea) == false)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(Aerorrhea) == true)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }

            }
            else if (Alar.Content.ToString() == "急停")
            {
                if (Convert.ToBoolean(Stop) == false)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(Stop) == true)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }
            }
            else if (Alar.Content.ToString() == "关阀")
            {
                if (Convert.ToBoolean(CloseValve) == false)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(CloseValve) == true)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }

            }
            else if (Alar.Content.ToString() == "阻车") 
            {
                if (Convert.ToBoolean(CarStopper) == false)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(CarStopper) == true)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }

            }
            else if (Alar.Content.ToString() == "在岗")
            {
                if (Convert.ToBoolean(PerDuty) == false)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(PerDuty) == true)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }

            }
            else if (Alar.Content.ToString() == "零速")
            {
                if (Convert.ToBoolean(Zerospeed) == false)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                else if (Convert.ToBoolean(Zerospeed) == true)
                {
                    Alar.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }

            }
        }


        private static void LoadingInfo(Grid firstfloor, SYSDeviceModel dev,object Status,object CardNo,object PlannedVolume,object AmountInstall,object InsTraffic,object Accumulate,object Temperature,object Denser,object Pressure,object Level)
        {
            TextBlock CrPsta = firstfloor.FindName(dev.StatusName) as TextBlock;
            if (CrPsta != null)
            {
                switch (Status.ToString())
                {
                    case "0":
                        CrPsta.Text = " 无效";
                        break;
                    case "1":
                        CrPsta.Text = " 待机中";
                        break;
                    case "2":
                        CrPsta.Text = " 准备中";
                        break;
                    case "3":
                        CrPsta.Text = " 运行中";
                        break;
                    case "4":
                        CrPsta.Text = " 暂停中";
                        break;
                    case "5":
                        CrPsta.Text = " 停止中";
                        break;

                }
            }
            Label OrName = firstfloor.FindName(dev.OrName) as Label;
            if (OrName != null)
            {
                OrName.Content = CardNo;
            }
            Label PVName = firstfloor.FindName(dev.PVName) as Label;
            if (PVName != null)
            {
                PVName.Content = Convert.ToDecimal(PlannedVolume) + insquan;
            }
            Label AIName = firstfloor.FindName(dev.AIName) as Label;
            if (AIName != null)
            {
                AIName.Content = Convert.ToDecimal(AmountInstall) + Actquan;
            }
            Label ITName = firstfloor.FindName(dev.ITName) as Label;
            if (ITName != null)
            {
                ITName.Content = Convert.ToDecimal(InsTraffic) / 10 + Insflow;
            }
            Label AName = firstfloor.FindName(dev.AName) as Label;
            if (AName != null)
            {
                AName.Content = Convert.ToDecimal(Accumulate) / 100 + Actflow;
            }
            Label TName = firstfloor.FindName(dev.TName) as Label;
            if (TName != null)
            {
                TName.Content = Convert.ToDecimal(Temperature).ToString("#0.0") + Tempun;
            }
            Label DName = firstfloor.FindName(dev.DName) as Label;
            if (DName != null)
            {
                DName.Content = Convert.ToDecimal(Denser).ToString("#0.000") + Denun;
            }
            Label PName = firstfloor.FindName(dev.PName) as Label;
            if (PName != null)
            {
                PName.Content = Convert.ToDecimal(Pressure).ToString("#0.000") + Presun;
            }
            Label LiName = firstfloor.FindName(dev.LiName) as Label;
            if (LiName != null)
            {
                LiName.Content = Convert.ToDecimal(Level).ToString("#0.00") + Levun;
            }
            ProgressBar PaceName = firstfloor.FindName(dev.PaceName) as ProgressBar;
            if (Status.ToString() == "3" && PlannedVolume.ToString() != "0" && PlannedVolume.ToString() != "-1" && AmountInstall.ToString() != "0" && AmountInstall.ToString() != "-1")
            {
                if (PaceName != null)
                {
                    double Pace = (Convert.ToDouble(AmountInstall) / Convert.ToDouble(PlannedVolume)) * 100;
                    PaceName.Value = Pace;
                }
            }
            else if (Status.ToString() == "0" || Status.ToString() == "1")
            {
                if (PaceName != null)
                {

                    PaceName.Value = 0;
                }
            }
            Label lblDH = firstfloor.FindName(dev.OrName) as Label;
            if (lblDH != null)
            {
                OrderInfoModels orm = new OrderInfoModels();
                orm.OrderNo = lblDH.Content.ToString();
                Label lblCPH = firstfloor.FindName(dev.CNName) as Label;
                if (orm.OrderNo == lblDH.Content.ToString() && lblDH.Content.ToString() != "0")
                {
                    foreach (OrderInfoModels om in DeviceService.OrderListQuery(orm))
                    {
                     
                        if (lblCPH != null)
                        {
                            lblCPH.Content = om.CarNo;

                        }
                        break;
                    }
                }
                else 
                {
                    lblCPH.Content = "0";
                }


            }
        }

        private static void CommError(Grid firstfloor, SYSDeviceModel dev)
        {
            firstfloor.Dispatcher.Invoke(() =>
            {
                Button Comm = firstfloor.FindName(dev.AlarmAName) as Button;
                if (Comm != null)
                {
                    Comm.Style = (Style)firstfloor.FindResource("ButtonDanger");
                }
                Button Man = firstfloor.FindName(dev.AlarmBName) as Button;
                if (Man != null)
                {
                    Man.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button Oil = firstfloor.FindName(dev.AlarmCName) as Button;
                if (Oil != null)
                {
                    Oil.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button key = firstfloor.FindName(dev.AlarmDName) as Button;
                if (key != null)
                {
                    key.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button combus = firstfloor.FindName(dev.AlarmEName) as Button;
                if (combus != null)
                {
                    combus.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button fluid = firstfloor.FindName(dev.AlarmFName) as Button;
                if (fluid != null)
                {
                    fluid.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button air = firstfloor.FindName(dev.AlarmGName) as Button;
                if (air != null)
                {
                    air.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button airflow = firstfloor.FindName(dev.AlarmHName) as Button;
                if (airflow != null)
                {
                    airflow.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button Gas = firstfloor.FindName(dev.AlarmIName) as Button;
                if (Gas != null)
                {
                    Gas.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                Button Estop = firstfloor.FindName(dev.AlarmJName) as Button;
                if (Estop != null)
                {
                    Estop.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button Close = firstfloor.FindName(dev.AlarmKName) as Button;
                if (Close != null)
                {
                    Close.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button Liquid = firstfloor.FindName(dev.AlarmLName) as Button;
                if (Liquid != null)
                {
                    Liquid.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button QiGUI = firstfloor.FindName(dev.AlarmQName) as Button;
                if (QiGUI != null)
                {
                    QiGUI.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button Zero = firstfloor.FindName(dev.AlarmWName) as Button;
                if (Zero != null)
                {
                    Zero.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button temper = firstfloor.FindName(dev.AlarmRName) as Button;
                if (temper != null)
                {
                    temper.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button density = firstfloor.FindName(dev.AlarmTName) as Button;
                if (density != null)
                {
                    density.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }

                Button pressure = firstfloor.FindName(dev.AlarmOName) as Button;
                if (pressure != null)
                {
                    pressure.Style = (Style)firstfloor.FindResource("ButtonSuccess");
                }
                TextBlock CrPsta = firstfloor.FindName(dev.StatusName) as TextBlock;

                if (CrPsta != null)
                {
                    CrPsta.Text = "无效";
                }
            });
               

          
        }

    }
}