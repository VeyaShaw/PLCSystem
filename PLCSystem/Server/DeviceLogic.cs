using PLCSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using PLCSystem.Models;
using System.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Threading;

namespace PLCSystem.BLL
{
    public class DeviceLogic
    {
        //if (type == 1)
        //{
        //    int bus = 1;
        //    if (itemList.Count != 1)
        //    {
        //        foreach (string name in itemList)
        //        {
        //            if (bus != 1)
        //            {
        //                TabControl tab = firstfloor.FindName(name) as TabControl;

        //                firstfloor.Children.Remove(tab);
        //            }
        //            bus++;
        //        }
        //    }
        //}

        private static DataAccess data = new DataAccess();
        /// <summary>
        /// 设备总数
        /// </summary>
        public static int TotalDevice { get; set; }
        /// <summary>
        /// 报警
        /// </summary>
        public static Task MainTask = null;
        /// <summary>
        /// 是否开启报警
        /// </summary>
        public static bool isRunning = true;




        private static List<string> itemList = new List<string>();


        public static void UIInitial(UserControl user, Grid firstfloor, Button LeftSwitch, Button RightSwitch, int Startdevice, int Endevice, int type)
        {
            List<SYSCraneSetModel> SYSCraneSetModel = new List<SYSCraneSetModel>();
            SYSCraneSetModel = data.CraneSetList(Startdevice, Endevice);
            int count = SYSCraneSetModel.Count+2;
            int countA = 1;
            int countB = 1;
            foreach (SYSCraneSetModel Cset in data.CraneSetList(Startdevice, Endevice))
            {
                SYSDeviceModel mo = new SYSDeviceModel();
                mo.MachineNo = Cset.MachineNo;
                mo.CraneNO = Cset.CraneNO;
                foreach (SYSDeviceModel dev in data.deviceList(mo, 1))
                {
                    //if (type == 0)
                    //{
                    //    itemList.Add(dev.MaName);
                    //}
                    if (countA <= 8)
                    {
                        TabControl tab = new TabControl();
                        tab.Name = dev.MaName;
                        tab.FontSize = 10;
                        Thickness Thickness = new Thickness(1, 2, 1, 2);
                        tab.Margin = Thickness;
                        tab.Style = (Style)user.FindResource("TabControlInLine");
                        Grid.SetRow(tab, 0);
                        Grid.SetColumn(tab, countA);
                        TabItem tabItem = new TabItem();
                        tabItem.Name = dev.CrName;
                        tabItem.Header = "机号：" + dev.MachineNo + dev.CraneNO;
                        var bc = new BrushConverter();
                        tabItem.Background = (Brush)bc.ConvertFrom("#FFE5E5E5");


                        #region grid布局
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
                        StackPanel stackA = new StackPanel();
                        TextBlock textA = new TextBlock();
                        textA.Text = "当前状态";
                        textA.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Thickness txtthA = new Thickness(5, 0, 0, 0);
                        textA.Margin = txtthA;
                        textA.VerticalAlignment = VerticalAlignment.Center;
                        Grid.SetRow(stackA, 0);
                        stackA.Orientation = Orientation.Horizontal;
                        Thickness STTHA = new Thickness(5, 0, 0, 0);
                        stackA.Margin = STTHA;
                        stackA.VerticalAlignment = VerticalAlignment.Center;
                        stackA.Children.Add(textA);
                        #endregion
                        grid.Children.Add(stackA);

                        #region 第2部分
                        StackPanel stackB = new StackPanel();
                        Label lblB = new Label();
                        lblB.Name = dev.StatusName;
                        lblB.Content = dev.CraneStatus;
                        Label lblzt = firstfloor.FindName(dev.StatusName) as Label;
                        if (lblzt == null)
                        {
                            firstfloor.RegisterName(dev.StatusName, lblB);

                        }
                        lblB.Style = (Style)user.FindResource("LabelInfo");
                        lblB.VerticalContentAlignment = VerticalAlignment.Center;
                        lblB.Height = 23;
                        lblB.Width = 72;
                        Grid.SetRow(stackB, 0);
                        Grid.SetColumn(stackB, 1);
                        stackB.Orientation = Orientation.Horizontal;
                        stackB.VerticalAlignment = VerticalAlignment.Center;
                        stackB.Children.Add(lblB);
                        #endregion
                        grid.Children.Add(stackB);

                        #region 第3部分
                        StackPanel stackC = new StackPanel();
                        TextBlock txtC = new TextBlock();
                        txtC.Text = "报警状态";
                        txtC.VerticalAlignment = VerticalAlignment.Center;
                        txtC.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackC, 0);
                        Grid.SetColumn(stackC, 3);
                        stackC.Orientation = Orientation.Horizontal;
                        stackC.VerticalAlignment = VerticalAlignment.Center;
                        Thickness StB = new Thickness(5, 0, 0, 0);
                        stackC.Margin = StB;
                        stackC.Children.Add(txtC);
                        #endregion
                        grid.Children.Add(stackC);

                        #region 第4部分
                        StackPanel stackD = new StackPanel();
                        TextBlock txtD = new TextBlock();
                        Thickness txtTHD = new Thickness(5, 0, 0, 0);
                        txtD.Margin = txtTHD;
                        txtD.Text = "当前单号";
                        txtD.VerticalAlignment = VerticalAlignment.Center;
                        txtD.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackD, 1);
                        stackD.Orientation = Orientation.Horizontal;
                        stackD.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHC = new Thickness(5, 0, 0, 0);
                        stackD.Margin = STTHC;
                        stackD.Children.Add(txtD);
                        #endregion
                        grid.Children.Add(stackD);

                        #region 第五部分
                        StackPanel stackE = new StackPanel();
                        Label lblE = new Label();
                        lblE.Name = dev.OrName;
                        lblE.Content = dev.OrderNumber;

                        Label lblDH = firstfloor.FindName(dev.OrName) as Label;
                        if (lblDH == null)
                        {
                            firstfloor.RegisterName(dev.OrName, lblE);

                        }

                        lblE.Style = (Style)user.FindResource("LabelInfo");
                        lblE.VerticalAlignment = VerticalAlignment.Center;
                        lblE.Height = 23;
                        lblE.Width = 72;
                        Grid.SetRow(stackE, 1);
                        Grid.SetColumn(stackE, 1);
                        stackE.Orientation = Orientation.Horizontal;
                        stackE.VerticalAlignment = VerticalAlignment.Center;
                        stackE.Children.Add(lblE);
                        #endregion
                        grid.Children.Add(stackE);

                        #region 第六部分
                        StackPanel stackF = new StackPanel();
                        TextBlock txtE = new TextBlock();
                        Thickness txtTHE = new Thickness(5, 0, 0, 0);
                        txtE.Margin = txtTHE;
                        txtE.Text = "车  牌  号 ";
                        txtE.VerticalAlignment = VerticalAlignment.Center;
                        txtE.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackF, 2);


                        stackF.Orientation = Orientation.Horizontal;
                        stackF.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHD = new Thickness(5, 0, 0, 0);
                        stackF.Margin = STTHD;
                        stackF.Children.Add(txtE);
                        #endregion
                        grid.Children.Add(stackF);

                        #region 第七部分
                        StackPanel stackG = new StackPanel();
                        Label lblF = new Label();
                        lblF.Name = dev.CNName;
                        lblF.Content = dev.CarNumber;
                        Label lblCPH = firstfloor.FindName(dev.CNName) as Label;
                        if (lblCPH == null)
                        {
                            firstfloor.RegisterName(dev.CNName, lblF);
                        }


                        lblF.Style = (Style)user.FindResource("LabelInfo");
                        lblF.VerticalAlignment = VerticalAlignment.Center;
                        lblF.Height = 23;
                        lblF.Width = 72;
                        Grid.SetRow(stackG, 2);
                        Grid.SetColumn(stackG, 1);
                        stackG.Orientation = Orientation.Horizontal;
                        stackG.VerticalAlignment = VerticalAlignment.Center;
                        stackG.Children.Add(lblF);
                        #endregion
                        grid.Children.Add(stackG);

                        #region 第八部分
                        StackPanel stackH = new StackPanel();
                        TextBlock txtF = new TextBlock();
                        Thickness txtTHF = new Thickness(3, 0, 0, 0);
                        txtF.Margin = txtTHF;
                        txtF.Text = "预  装  量 ";
                        txtF.VerticalAlignment = VerticalAlignment.Center;
                        txtF.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackH, 3);
                        stackH.Orientation = Orientation.Horizontal;
                        stackH.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHF = new Thickness(5, 0, 0, 0);
                        stackH.Margin = STTHF;
                        stackH.Children.Add(txtF);
                        #endregion
                        grid.Children.Add(stackH);

                        #region 第九部分
                        StackPanel stackJ = new StackPanel();
                        Label lblG = new Label();
                        lblG.Name = dev.PVName;
                        lblG.Content = dev.PlannedVolume + "Kg";

                        Label lblYZL = firstfloor.FindName(dev.PVName) as Label;
                        if (lblYZL == null)
                        {
                            firstfloor.RegisterName(dev.PVName, lblG);
                        }



                        lblG.Style = (Style)user.FindResource("LabelInfo");
                        lblG.VerticalAlignment = VerticalAlignment.Center;
                        lblG.Height = 23;
                        lblG.Width = 72;
                        Grid.SetRow(stackJ, 3);
                        Grid.SetColumn(stackJ, 1);
                        stackJ.Orientation = Orientation.Horizontal;
                        stackJ.VerticalAlignment = VerticalAlignment.Center;
                        stackJ.Children.Add(lblG);
                        #endregion
                        grid.Children.Add(stackJ);

                        #region 第十部分
                        StackPanel stackK = new StackPanel();
                        TextBlock txtG = new TextBlock();
                        Thickness txtTHG = new Thickness(3, 0, 0, 0);
                        txtG.Margin = txtTHG;
                        txtG.Text = "实  装  量 ";
                        txtG.VerticalAlignment = VerticalAlignment.Center;
                        txtG.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackK, 4);
                        stackK.Orientation = Orientation.Horizontal;
                        stackK.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHG = new Thickness(5, 0, 0, 0);
                        stackK.Margin = STTHG;
                        stackK.Children.Add(txtG);
                        #endregion
                        grid.Children.Add(stackK);

                        #region 第十一部分
                        StackPanel stackL = new StackPanel();
                        Label lblH = new Label();
                        lblH.Name = dev.AIName;
                        lblH.Content = dev.AmountInstall + "Kg";
                        Label lblSZL = firstfloor.FindName(dev.AIName) as Label;
                        if (lblSZL == null)
                        {
                            firstfloor.RegisterName(dev.AIName, lblH);
                        }
                        lblH.Style = (Style)user.FindResource("LabelInfo");
                        lblH.VerticalAlignment = VerticalAlignment.Center;
                        lblH.Height = 23;
                        lblH.Width = 72;
                        Grid.SetRow(stackL, 4);
                        Grid.SetColumn(stackL, 1);
                        stackL.Orientation = Orientation.Horizontal;
                        stackL.VerticalAlignment = VerticalAlignment.Center;
                        stackL.Children.Add(lblH);
                        #endregion
                        grid.Children.Add(stackL);

                        #region 第十二部分
                        StackPanel stackQ = new StackPanel();
                        TextBlock txtH = new TextBlock();
                        Thickness txtTHH = new Thickness(3, 0, 0, 0);
                        txtH.Margin = txtTHH;
                        txtH.Text = "瞬时流量";
                        txtH.VerticalAlignment = VerticalAlignment.Center;
                        txtH.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackQ, 5);
                        stackQ.Orientation = Orientation.Horizontal;
                        stackQ.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHH = new Thickness(5, 0, 0, 0);
                        stackQ.Margin = STTHH;
                        stackQ.Children.Add(txtH);
                        #endregion
                        grid.Children.Add(stackQ);

                        #region 第十三部分
                        StackPanel stackW = new StackPanel();
                        Label lblJ = new Label();
                        lblJ.Name = dev.ITName;
                        lblJ.Content = dev.InsTraffic + "T/h";
                        Label lblSHLL = firstfloor.FindName(dev.ITName) as Label;
                        if (lblSHLL == null)
                        {
                            firstfloor.RegisterName(dev.ITName, lblJ);
                        }


                        lblJ.Style = (Style)user.FindResource("LabelInfo");
                        lblJ.VerticalAlignment = VerticalAlignment.Center;
                        lblJ.Height = 23;
                        lblJ.Width = 72;
                        Grid.SetRow(stackW, 5);
                        Grid.SetColumn(stackW, 1);
                        stackW.Orientation = Orientation.Horizontal;
                        stackW.VerticalAlignment = VerticalAlignment.Center;
                        stackW.Children.Add(lblJ);
                        #endregion
                        grid.Children.Add(stackW);

                        #region 第十四部分
                        StackPanel stackR = new StackPanel();
                        TextBlock txtJ = new TextBlock();
                        Thickness txtTHJ = new Thickness(3, 0, 0, 0);
                        txtJ.Margin = txtTHH;
                        txtJ.Text = "累计流量";
                        txtJ.VerticalAlignment = VerticalAlignment.Center;
                        txtJ.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackR, 6);
                        stackR.Orientation = Orientation.Horizontal;
                        stackR.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHJ = new Thickness(5, 0, 0, 0);
                        stackR.Margin = STTHH;
                        stackR.Children.Add(txtJ);
                        #endregion
                        grid.Children.Add(stackR);

                        #region 第十五部分
                        StackPanel stackT = new StackPanel();
                        Label lblK = new Label();
                        lblK.Name = dev.AName;
                        lblK.Content = dev.Accumulate + "T";
                        Label lblLJLL = firstfloor.FindName(dev.AName) as Label;
                        if (lblLJLL == null)
                        {
                            firstfloor.RegisterName(dev.AName, lblK);
                        }
                        lblK.Style = (Style)user.FindResource("LabelInfo");
                        lblK.VerticalAlignment = VerticalAlignment.Center;
                        lblK.Height = 23;
                        lblK.Width = 72;
                        Grid.SetRow(stackT, 6);
                        Grid.SetColumn(stackT, 1);
                        stackT.Orientation = Orientation.Horizontal;
                        stackT.VerticalAlignment = VerticalAlignment.Center;
                        stackT.Children.Add(lblK);
                        #endregion
                        grid.Children.Add(stackT);

                        #region 第16部分
                        StackPanel stackY = new StackPanel();
                        TextBlock txtK = new TextBlock();
                        Thickness txtTHK = new Thickness(3, 0, 0, 0);
                        txtK.Margin = txtTHK;
                        txtK.Text = " 温      度 ";
                        txtK.VerticalAlignment = VerticalAlignment.Center;
                        txtK.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackY, 7);
                        stackY.Orientation = Orientation.Horizontal;
                        stackY.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHK = new Thickness(5, 0, 0, 0);
                        stackY.Margin = STTHK;
                        stackY.Children.Add(txtK);
                        #endregion
                        grid.Children.Add(stackY);

                        #region 第17部分
                        StackPanel stU = new StackPanel();
                        Label lblL = new Label();
                        lblL.Name = dev.TName;
                        lblL.Content = dev.Temperature + "℃";

                        Label lblWD = firstfloor.FindName(dev.TName) as Label;
                        if (lblWD == null)
                        {
                            firstfloor.RegisterName(dev.TName, lblL);
                        }

                        lblL.Style = (Style)user.FindResource("LabelInfo");
                        lblL.VerticalAlignment = VerticalAlignment.Center;
                        lblL.Height = 23;
                        lblL.Width = 72;
                        Grid.SetRow(stU, 7);
                        Grid.SetColumn(stU, 1);
                        stU.Orientation = Orientation.Horizontal;
                        stU.VerticalAlignment = VerticalAlignment.Center;
                        stU.Children.Add(lblL);
                        #endregion
                        grid.Children.Add(stU);

                        #region 第18部分
                        StackPanel STI = new StackPanel();
                        TextBlock txtl = new TextBlock();
                        Thickness txtTHL = new Thickness(3, 0, 0, 0);
                        txtl.Margin = txtTHL;
                        txtl.Text = " 密      度 ";
                        txtl.VerticalAlignment = VerticalAlignment.Center;
                        txtl.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(STI, 8);
                        STI.Orientation = Orientation.Horizontal;
                        STI.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHL = new Thickness(5, 0, 0, 0);
                        STI.Margin = STTHL;
                        STI.Children.Add(txtl);
                        #endregion
                        grid.Children.Add(STI);

                        #region 第19部分
                        StackPanel STO = new StackPanel();
                        Label lblQ = new Label();
                        lblQ.Name = dev.DName;
                        lblQ.Content = dev.Denser + "Kg/m³";
                        Label lblMD = firstfloor.FindName(dev.DName) as Label;
                        if (lblMD == null)
                        {
                            firstfloor.RegisterName(dev.DName, lblQ);
                        }


                        lblQ.Style = (Style)user.FindResource("LabelInfo");
                        lblQ.VerticalAlignment = VerticalAlignment.Center;
                        lblQ.Height = 25;
                        lblQ.Width = 72;
                        Grid.SetRow(STO, 8);
                        Grid.SetColumn(STO, 1);
                        STO.Orientation = Orientation.Horizontal;
                        STO.VerticalAlignment = VerticalAlignment.Center;
                        STO.Children.Add(lblQ);
                        #endregion
                        grid.Children.Add(STO);

                        #region 第20部分
                        StackPanel STP = new StackPanel();
                        TextBlock txtQ = new TextBlock();
                        Thickness txtTHQ = new Thickness(5, 8, 0, 0);
                        txtQ.Margin = txtTHQ;
                        txtQ.Text = "压      力 ";
                        txtQ.VerticalAlignment = VerticalAlignment.Center;
                        txtQ.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(STP, 9);
                        STI.Orientation = Orientation.Horizontal;
                        STI.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHQ = new Thickness(5, 0, 0, 0);
                        STP.Margin = STTHQ;
                        STP.Children.Add(txtQ);
                        #endregion
                        grid.Children.Add(STP);

                        #region 第21部分
                        StackPanel STZ = new StackPanel();
                        Label lblW = new Label();

                        lblW.Name = dev.PName;
                        lblW.Content = dev.Pressure + "Mpa";
                        Label lblYL = firstfloor.FindName(dev.PName) as Label;
                        if (lblYL == null)
                        {
                            firstfloor.RegisterName(dev.PName, lblW);
                        }


                        lblW.Style = (Style)user.FindResource("LabelInfo");
                        lblW.VerticalAlignment = VerticalAlignment.Center;
                        lblW.Height = 25;
                        lblW.Width = 72;
                        Grid.SetRow(STZ, 9);
                        Grid.SetColumn(STZ, 1);
                        Thickness STTHE = new Thickness(0, 3, 0, 0);
                        STZ.Margin = STTHE;
                        STZ.Orientation = Orientation.Horizontal;
                        STZ.VerticalAlignment = VerticalAlignment.Center;
                        STZ.Children.Add(lblW);
                        #endregion                        
                        grid.Children.Add(STZ);

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
                        //Thickness STTHE = new Thickness(0, 3, 0, 0);
                        //STX.Margin = STTHE;
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
                        butjt.Content = "急停";
                        Button butJJT = firstfloor.FindName(dev.EmeStopName) as Button;
                        if (butjt == null)
                        {
                            firstfloor.RegisterName(dev.EmeStopName, butJJT);
                        }
                        butjt.Style = (Style)user.FindResource("ButtonDanger");
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
                        List<SYSConfigModel> ConList = new List<SYSConfigModel>();
                        ConList = data.ConfigList();
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
                            if (conf.Priority == 0)
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
                                if (dev.AlarmA == 1)
                                {
                                    buttx.Style = (Style)user.FindResource("ButtonSuccess");
                                }
                                else if(dev.AlarmA == 0)
                                {
                                    buttx.Style = (Style)user.FindResource("ButtonDanger");
                                }
                         
                                buttx.Width = 40;
                                Grid.SetRow(STbjA, conf.Rowscount);
                                Grid.SetColumn(STbjA, conf.Collcount);
                                //STbjA.Margin = BJBJ;
                                STbjA.Orientation = Orientation.Horizontal;
                                STbjA.VerticalAlignment = VerticalAlignment.Center;
                                STbjA.Children.Add(buttx);
                            }
                            //静电
                            else if (conf.Priority == 1)
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
                                //butjd.Style = (Style)user.FindResource("ButtonSuccess");
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
                                if (butOil== null)
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
                            //液接
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


                            //气连
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
                            //液归
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
                            //气归
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
                            //温度
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
                            //密度
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
                            //压力
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
                    else
                    {
                        TabControl tab = new TabControl();
                        tab.Name = dev.MaName;
                        tab.FontSize = 10;
                        Thickness Thickness = new Thickness(1, 2, 1, 2);
                        tab.Margin = Thickness;
                        tab.Style = (Style)user.FindResource("TabControlInLine");
                        Grid.SetRow(tab, 1);
                        Grid.SetColumn(tab, countB);
                        TabItem tabItem = new TabItem();
                        tabItem.Name = dev.CrName;
                        tabItem.Header = "机号：" + dev.MachineNo + dev.CraneNO;
                        var bc = new BrushConverter();
                        tabItem.Background = (Brush)bc.ConvertFrom("#FFE5E5E5");
                        #region grid布局
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
                        StackPanel stackA = new StackPanel();
                        TextBlock textA = new TextBlock();
                        textA.Text = "当前状态";
                        textA.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Thickness txtthA = new Thickness(5, 0, 0, 0);
                        textA.Margin = txtthA;
                        textA.VerticalAlignment = VerticalAlignment.Center;
                        Grid.SetRow(stackA, 0);
                        stackA.Orientation = Orientation.Horizontal;
                        Thickness STTHA = new Thickness(5, 0, 0, 0);
                        stackA.Margin = STTHA;
                        stackA.VerticalAlignment = VerticalAlignment.Center;
                        stackA.Children.Add(textA);
                        #endregion
                        grid.Children.Add(stackA);

                        #region 第2部分
                        StackPanel stackB = new StackPanel();
                        Label lblB = new Label();
                        lblB.Name = dev.StatusName;
                        lblB.Content = dev.CraneStatus;
                        Label lblzt = firstfloor.FindName(dev.StatusName) as Label;
                        if (lblzt == null)
                        {
                            firstfloor.RegisterName(dev.StatusName, lblB);

                        }
                        lblB.Style = (Style)user.FindResource("LabelInfo");
                        lblB.VerticalContentAlignment = VerticalAlignment.Center;
                        lblB.Height = 23;
                        lblB.Width = 72;
                        Grid.SetRow(stackB, 0);
                        Grid.SetColumn(stackB, 1);
                        stackB.Orientation = Orientation.Horizontal;
                        stackB.VerticalAlignment = VerticalAlignment.Center;
                        stackB.Children.Add(lblB);
                        #endregion
                        grid.Children.Add(stackB);

                        #region 第3部分
                        StackPanel stackC = new StackPanel();
                        TextBlock txtC = new TextBlock();
                        txtC.Text = "报警状态";
                        txtC.VerticalAlignment = VerticalAlignment.Center;
                        txtC.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackC, 0);
                        Grid.SetColumn(stackC, 3);
                        stackC.Orientation = Orientation.Horizontal;
                        stackC.VerticalAlignment = VerticalAlignment.Center;
                        Thickness StB = new Thickness(5, 0, 0, 0);
                        stackC.Margin = StB;
                        stackC.Children.Add(txtC);
                        #endregion
                        grid.Children.Add(stackC);

                        #region 第4部分
                        StackPanel stackD = new StackPanel();
                        TextBlock txtD = new TextBlock();
                        Thickness txtTHD = new Thickness(5, 0, 0, 0);
                        txtD.Margin = txtTHD;
                        txtD.Text = "当前单号";
                        txtD.VerticalAlignment = VerticalAlignment.Center;
                        txtD.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackD, 1);
                        stackD.Orientation = Orientation.Horizontal;
                        stackD.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHC = new Thickness(5, 0, 0, 0);
                        stackD.Margin = STTHC;
                        stackD.Children.Add(txtD);
                        #endregion
                        grid.Children.Add(stackD);

                        #region 第五部分
                        StackPanel stackE = new StackPanel();
                        Label lblE = new Label();
                        lblE.Name = dev.OrName;
                        lblE.Content = dev.OrderNumber;

                        Label lblDH = firstfloor.FindName(dev.OrName) as Label;
                        if (lblDH == null)
                        {
                            firstfloor.RegisterName(dev.OrName, lblE);

                        }

                        lblE.Style = (Style)user.FindResource("LabelInfo");
                        lblE.VerticalAlignment = VerticalAlignment.Center;
                        lblE.Height = 23;
                        lblE.Width = 72;
                        Grid.SetRow(stackE, 1);
                        Grid.SetColumn(stackE, 1);
                        stackE.Orientation = Orientation.Horizontal;
                        stackE.VerticalAlignment = VerticalAlignment.Center;
                        stackE.Children.Add(lblE);
                        #endregion
                        grid.Children.Add(stackE);

                        #region 第六部分
                        StackPanel stackF = new StackPanel();
                        TextBlock txtE = new TextBlock();
                        Thickness txtTHE = new Thickness(5, 0, 0, 0);
                        txtE.Margin = txtTHE;
                        txtE.Text = "车  牌  号 ";
                        txtE.VerticalAlignment = VerticalAlignment.Center;
                        txtE.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackF, 2);


                        stackF.Orientation = Orientation.Horizontal;
                        stackF.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHD = new Thickness(5, 0, 0, 0);
                        stackF.Margin = STTHD;
                        stackF.Children.Add(txtE);
                        #endregion
                        grid.Children.Add(stackF);

                        #region 第七部分
                        StackPanel stackG = new StackPanel();
                        Label lblF = new Label();
                        lblF.Name = dev.CNName;
                        lblF.Content = dev.CarNumber;
                        Label lblCPH = firstfloor.FindName(dev.CNName) as Label;
                        if (lblCPH == null)
                        {
                            firstfloor.RegisterName(dev.CNName, lblF);
                        }


                        lblF.Style = (Style)user.FindResource("LabelInfo");
                        lblF.VerticalAlignment = VerticalAlignment.Center;
                        lblF.Height = 23;
                        lblF.Width = 72;
                        Grid.SetRow(stackG, 2);
                        Grid.SetColumn(stackG, 1);
                        stackG.Orientation = Orientation.Horizontal;
                        stackG.VerticalAlignment = VerticalAlignment.Center;
                        stackG.Children.Add(lblF);
                        #endregion
                        grid.Children.Add(stackG);

                        #region 第八部分
                        StackPanel stackH = new StackPanel();
                        TextBlock txtF = new TextBlock();
                        Thickness txtTHF = new Thickness(3, 0, 0, 0);
                        txtF.Margin = txtTHF;
                        txtF.Text = "预  装  量 ";
                        txtF.VerticalAlignment = VerticalAlignment.Center;
                        txtF.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackH, 3);
                        stackH.Orientation = Orientation.Horizontal;
                        stackH.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHF = new Thickness(5, 0, 0, 0);
                        stackH.Margin = STTHF;
                        stackH.Children.Add(txtF);
                        #endregion
                        grid.Children.Add(stackH);

                        #region 第九部分
                        StackPanel stackJ = new StackPanel();
                        Label lblG = new Label();
                        lblG.Name = dev.PVName;
                        lblG.Content = dev.PlannedVolume + "Kg";

                        Label lblYZL = firstfloor.FindName(dev.PVName) as Label;
                        if (lblYZL == null)
                        {
                            firstfloor.RegisterName(dev.PVName, lblG);
                        }



                        lblG.Style = (Style)user.FindResource("LabelInfo");
                        lblG.VerticalAlignment = VerticalAlignment.Center;
                        lblG.Height = 23;
                        lblG.Width = 72;
                        Grid.SetRow(stackJ, 3);
                        Grid.SetColumn(stackJ, 1);
                        stackJ.Orientation = Orientation.Horizontal;
                        stackJ.VerticalAlignment = VerticalAlignment.Center;
                        stackJ.Children.Add(lblG);
                        #endregion
                        grid.Children.Add(stackJ);

                        #region 第十部分
                        StackPanel stackK = new StackPanel();
                        TextBlock txtG = new TextBlock();
                        Thickness txtTHG = new Thickness(3, 0, 0, 0);
                        txtG.Margin = txtTHG;
                        txtG.Text = "实  装  量 ";
                        txtG.VerticalAlignment = VerticalAlignment.Center;
                        txtG.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackK, 4);
                        stackK.Orientation = Orientation.Horizontal;
                        stackK.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHG = new Thickness(5, 0, 0, 0);
                        stackK.Margin = STTHG;
                        stackK.Children.Add(txtG);
                        #endregion
                        grid.Children.Add(stackK);

                        #region 第十一部分
                        StackPanel stackL = new StackPanel();
                        Label lblH = new Label();
                        lblH.Name = dev.AIName;
                        lblH.Content = dev.AmountInstall + "Kg";
                        Label lblSZL = firstfloor.FindName(dev.AIName) as Label;
                        if (lblSZL == null)
                        {
                            firstfloor.RegisterName(dev.AIName, lblH);
                        }
                        lblH.Style = (Style)user.FindResource("LabelInfo");
                        lblH.VerticalAlignment = VerticalAlignment.Center;
                        lblH.Height = 23;
                        lblH.Width = 72;
                        Grid.SetRow(stackL, 4);
                        Grid.SetColumn(stackL, 1);
                        stackL.Orientation = Orientation.Horizontal;
                        stackL.VerticalAlignment = VerticalAlignment.Center;
                        stackL.Children.Add(lblH);
                        #endregion
                        grid.Children.Add(stackL);

                        #region 第十二部分
                        StackPanel stackQ = new StackPanel();
                        TextBlock txtH = new TextBlock();
                        Thickness txtTHH = new Thickness(3, 0, 0, 0);
                        txtH.Margin = txtTHH;
                        txtH.Text = "瞬时流量";
                        txtH.VerticalAlignment = VerticalAlignment.Center;
                        txtH.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackQ, 5);
                        stackQ.Orientation = Orientation.Horizontal;
                        stackQ.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHH = new Thickness(5, 0, 0, 0);
                        stackQ.Margin = STTHH;
                        stackQ.Children.Add(txtH);
                        #endregion
                        grid.Children.Add(stackQ);

                        #region 第十三部分
                        StackPanel stackW = new StackPanel();
                        Label lblJ = new Label();
                        lblJ.Name = dev.ITName;
                        lblJ.Content = dev.InsTraffic + "T/h";
                        Label lblSHLL = firstfloor.FindName(dev.ITName) as Label;
                        if (lblSHLL == null)
                        {
                            firstfloor.RegisterName(dev.ITName, lblJ);
                        }


                        lblJ.Style = (Style)user.FindResource("LabelInfo");
                        lblJ.VerticalAlignment = VerticalAlignment.Center;
                        lblJ.Height = 23;
                        lblJ.Width = 72;
                        Grid.SetRow(stackW, 5);
                        Grid.SetColumn(stackW, 1);
                        stackW.Orientation = Orientation.Horizontal;
                        stackW.VerticalAlignment = VerticalAlignment.Center;
                        stackW.Children.Add(lblJ);
                        #endregion
                        grid.Children.Add(stackW);

                        #region 第十四部分
                        StackPanel stackR = new StackPanel();
                        TextBlock txtJ = new TextBlock();
                        Thickness txtTHJ = new Thickness(3, 0, 0, 0);
                        txtJ.Margin = txtTHH;
                        txtJ.Text = "累计流量";
                        txtJ.VerticalAlignment = VerticalAlignment.Center;
                        txtJ.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackR, 6);
                        stackR.Orientation = Orientation.Horizontal;
                        stackR.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHJ = new Thickness(5, 0, 0, 0);
                        stackR.Margin = STTHH;
                        stackR.Children.Add(txtJ);
                        #endregion
                        grid.Children.Add(stackR);

                        #region 第十五部分
                        StackPanel stackT = new StackPanel();
                        Label lblK = new Label();
                        lblK.Name = dev.AName;
                        lblK.Content = dev.Accumulate + "T";
                        Label lblLJLL = firstfloor.FindName(dev.AName) as Label;
                        if (lblLJLL == null)
                        {
                            firstfloor.RegisterName(dev.AName, lblK);
                        }
                        lblK.Style = (Style)user.FindResource("LabelInfo");
                        lblK.VerticalAlignment = VerticalAlignment.Center;
                        lblK.Height = 23;
                        lblK.Width = 72;
                        Grid.SetRow(stackT, 6);
                        Grid.SetColumn(stackT, 1);
                        stackT.Orientation = Orientation.Horizontal;
                        stackT.VerticalAlignment = VerticalAlignment.Center;
                        stackT.Children.Add(lblK);
                        #endregion
                        grid.Children.Add(stackT);

                        #region 第16部分
                        StackPanel stackY = new StackPanel();
                        TextBlock txtK = new TextBlock();
                        Thickness txtTHK = new Thickness(3, 0, 0, 0);
                        txtK.Margin = txtTHK;
                        txtK.Text = " 温      度 ";
                        txtK.VerticalAlignment = VerticalAlignment.Center;
                        txtK.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(stackY, 7);
                        stackY.Orientation = Orientation.Horizontal;
                        stackY.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHK = new Thickness(5, 0, 0, 0);
                        stackY.Margin = STTHK;
                        stackY.Children.Add(txtK);
                        #endregion
                        grid.Children.Add(stackY);

                        #region 第17部分
                        StackPanel stU = new StackPanel();
                        Label lblL = new Label();
                        lblL.Name = dev.TName;
                        lblL.Content = dev.Temperature + "℃";

                        Label lblWD = firstfloor.FindName(dev.TName) as Label;
                        if (lblWD == null)
                        {
                            firstfloor.RegisterName(dev.TName, lblL);
                        }

                        lblL.Style = (Style)user.FindResource("LabelInfo");
                        lblL.VerticalAlignment = VerticalAlignment.Center;
                        lblL.Height = 23;
                        lblL.Width = 72;
                        Grid.SetRow(stU, 7);
                        Grid.SetColumn(stU, 1);
                        stU.Orientation = Orientation.Horizontal;
                        stU.VerticalAlignment = VerticalAlignment.Center;
                        stU.Children.Add(lblL);
                        #endregion
                        grid.Children.Add(stU);

                        #region 第18部分
                        StackPanel STI = new StackPanel();
                        TextBlock txtl = new TextBlock();
                        Thickness txtTHL = new Thickness(3, 0, 0, 0);
                        txtl.Margin = txtTHL;
                        txtl.Text = " 密      度 ";
                        txtl.VerticalAlignment = VerticalAlignment.Center;
                        txtl.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(STI, 8);
                        STI.Orientation = Orientation.Horizontal;
                        STI.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHL = new Thickness(5, 0, 0, 0);
                        STI.Margin = STTHL;
                        STI.Children.Add(txtl);
                        #endregion
                        grid.Children.Add(STI);

                        #region 第19部分
                        StackPanel STO = new StackPanel();
                        Label lblQ = new Label();
                        lblQ.Name = dev.DName;
                        lblQ.Content = dev.Denser + "Kg/m³";
                        Label lblMD = firstfloor.FindName(dev.DName) as Label;
                        if (lblMD == null)
                        {
                            firstfloor.RegisterName(dev.DName, lblQ);
                        }


                        lblQ.Style = (Style)user.FindResource("LabelInfo");
                        lblQ.VerticalAlignment = VerticalAlignment.Center;
                        lblQ.Height = 25;
                        lblQ.Width = 72;
                        Grid.SetRow(STO, 8);
                        Grid.SetColumn(STO, 1);
                        STO.Orientation = Orientation.Horizontal;
                        STO.VerticalAlignment = VerticalAlignment.Center;
                        STO.Children.Add(lblQ);
                        #endregion
                        grid.Children.Add(STO);

                        #region 第20部分
                        StackPanel STP = new StackPanel();
                        TextBlock txtQ = new TextBlock();
                        Thickness txtTHQ = new Thickness(5, 8, 0, 0);
                        txtQ.Margin = txtTHQ;
                        txtQ.Text = "压      力 ";
                        txtQ.VerticalAlignment = VerticalAlignment.Center;
                        txtQ.Style = (Style)user.FindResource("TextBlockDefaultBold");
                        Grid.SetRow(STP, 9);
                        STI.Orientation = Orientation.Horizontal;
                        STI.VerticalAlignment = VerticalAlignment.Center;
                        Thickness STTHQ = new Thickness(5, 0, 0, 0);
                        STP.Margin = STTHQ;
                        STP.Children.Add(txtQ);
                        #endregion
                        grid.Children.Add(STP);

                        #region 第21部分
                        StackPanel STZ = new StackPanel();
                        Label lblW = new Label();

                        lblW.Name = dev.PName;
                        lblW.Content = dev.Pressure + "Mpa";
                        Label lblYL = firstfloor.FindName(dev.PName) as Label;
                        if (lblYL == null)
                        {
                            firstfloor.RegisterName(dev.PName, lblW);
                        }


                        lblW.Style = (Style)user.FindResource("LabelInfo");
                        lblW.VerticalAlignment = VerticalAlignment.Center;
                        lblW.Height = 25;
                        lblW.Width = 72;
                        Grid.SetRow(STZ, 9);
                        Grid.SetColumn(STZ, 1);
                        Thickness STTHE = new Thickness(0, 3, 0, 0);
                        STZ.Margin = STTHE;
                        STZ.Orientation = Orientation.Horizontal;
                        STZ.VerticalAlignment = VerticalAlignment.Center;
                        STZ.Children.Add(lblW);
                        #endregion                        
                        grid.Children.Add(STZ);

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
                        //Thickness STTHE = new Thickness(0, 3, 0, 0);
                        //STX.Margin = STTHE;
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
                        butjt.Content = "急停";
                        Button butJJT = firstfloor.FindName(dev.EmeStopName) as Button;
                        if (butjt == null)
                        {
                            firstfloor.RegisterName(dev.EmeStopName, butJJT);
                        }
                        butjt.Style = (Style)user.FindResource("ButtonDanger");
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
                        List<SYSConfigModel> ConList = new List<SYSConfigModel>();
                        ConList = data.ConfigList();
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
                            if (conf.Priority == 0)
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
                                if (dev.AlarmA == 1)
                                {
                                    buttx.Style = (Style)user.FindResource("ButtonSuccess");
                                }
                                else if (dev.AlarmA == 0)
                                {
                                    buttx.Style = (Style)user.FindResource("ButtonDanger");
                                }

                                buttx.Width = 40;
                                Grid.SetRow(STbjA, conf.Rowscount);
                                Grid.SetColumn(STbjA, conf.Collcount);
                                //STbjA.Margin = BJBJ;
                                STbjA.Orientation = Orientation.Horizontal;
                                STbjA.VerticalAlignment = VerticalAlignment.Center;
                                STbjA.Children.Add(buttx);
                            }
                            //静电
                            else if (conf.Priority == 1)
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
                                //butjd.Style = (Style)user.FindResource("ButtonSuccess");
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
                            //液接
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


                            //气连
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
                            //液归
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
                            //气归
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
                            //温度
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
                            //密度
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
                            //压力
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
                        countB++;
                    }

                }
            }

            if (count == 1)
            {
                RightSwitch.Visibility = Visibility.Hidden;
            }
           

        }
      

        /// <summary>
        /// 通讯信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="firstfloor"></param>
        /// <param name="Startdevice"></param>
        /// <param name="Endevice"></param>
        public  static void CommunicationInfo(UserControl user, Grid firstfloor, int Startdevice, int Endevice) 
        {

            //MainTask = Task.Run(async() =>
            //{

            //    while (isRunning)
            //    {
            //        await Task.Delay(100);
            //        foreach (SYSDeviceModel dev in data.DeviceAddress(Startdevice, Endevice))
            //        {
            //            Ping pingSender = new Ping();
            //            PingReply reply;
            //            reply = pingSender.Send(dev.IP, 1000);
            //            foreach (SYSDeviceModel devm in data.Commdata(dev.IP))
            //            {

            //                if (dev.IP == devm.IP)
            //                {
            //                    if (reply.Status == IPStatus.Success)
            //                    {

            //                        if (user.Dispatcher.Thread != Thread.CurrentThread)
            //                        {
            //                            user.Dispatcher.Invoke(() =>
            //                            {
            //                                Button btntx = firstfloor.FindName(devm.AlarmAName) as Button;
            //                                btntx.Style = (Style)user.FindResource("ButtonSuccess");
            //                            });
            //                        }
                                 

            //                    }
            //                    else
            //                    {
            //                        if (user.Dispatcher.Thread != Thread.CurrentThread)
            //                        {
            //                            user.Dispatcher.Invoke(() =>
            //                            {
            //                                Button btntx = firstfloor.FindName(devm.AlarmAName) as Button;
            //                                btntx.Style = (Style)user.FindResource("ButtonDanger");
            //                            });

            //                        }
                                      






            //                    }
            //                }

            //            }

            //        }




            //    }


            //});




        }
     

        public static int DeviceCount() 
        {

            return data.DeviceCount();
        }

        public static void MainStart() 
        {
            isRunning = true;
            //MainTask.ConfigureAwait(true);
        }

        public static void MainDispose()
        {
            isRunning = false;
            if (MainTask != null)
                MainTask.ConfigureAwait(true);

        }




    }
}
