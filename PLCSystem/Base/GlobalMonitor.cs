using PLCSystem.Common.Log;
using PLCSystem.DAL;
using PLCSystem.Models;
using PLCSystem.Service;
using S7.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.Windows.Input;
using System.Windows.Markup;

namespace PLCSystem.Base
{
    /// <summary>
    /// 全局监控
    /// </summary>
    public class GlobalMonitor
    {

        /// <summary>
        /// 获取启用的鹤位数量
        /// </summary>
        private static List<SYSDeviceModel> devmList;

        /// <summary>
        /// 
        /// </summary>
        private static DeviceService devser = new DeviceService();
        /// <summary>
        /// 全局监控
        /// </summary>
        public  static Task TaskMain = null;
        /// <summary>
        /// 正在运行
        /// </summary>
         public  static bool isRunning = true;

        private static readonly TaskScheduler _syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        public static void Start()
        {
            TaskMain = Task.Factory.StartNew(async() =>
            {
           
                while (isRunning)
                {
                    foreach (SYSCraneSetModel Cr in DeviceService.TotalDeviceCount())
                    {
                        SYSDeviceModel devm = new SYSDeviceModel();
                        devm.MachineNo = Cr.MachineNo;
                        devm.CraneNO = Cr.CraneNO;
                        foreach (SYSDeviceModel dev in DeviceService.deviceList(devm, 1))
                        {
                            Ping pingSender = new Ping();
                            PingReply reply;
                            reply = pingSender.Send(dev.IP, 100);
                            if (reply.Status == IPStatus.Success)
                            {
                                Task.Factory.StartNew(() => Alarmlog(dev),
                                new CancellationTokenSource().Token, TaskCreationOptions.None, _syncContextTaskScheduler).Wait();
                            }
                            else
                            {
                                Task.Factory.StartNew(() => AlarmLogError(dev),
                                new CancellationTokenSource().Token, TaskCreationOptions.None, _syncContextTaskScheduler).Wait();
                                continue;
                            }
                        }
                        await Task.Delay(1000);
                    }
                }
            });
        }

        private static void Alarmlog(SYSDeviceModel dev) 
        {
            Plc plc = new Plc(CpuType.S7200Smart, dev.IP, 0, 1);
            try
            {
                plc.Open();
                AlarmInfo alar = new AlarmInfo();
                alar.DeviceNO = dev.MachineNo;
                alar.TagNo = dev.CraneNO;
                alar.AlarmName = "网络故障";
                alar.Flag = 0;
                if (devser.AlarmsList(alar).Count() == 1) 
                {
                    alar.Flag = 1;
                    devser.AlarmEdit(alar);
                }
                if (dev.CraneNO == "A")
                {
                    #region 静电报警
                    var elecA = plc.Read("M20.0");
                    alar.AlarmName = "静电报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar,Convert.ToBoolean(elecA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    # region 溢油报警
                    var oilSpiA = plc.Read("M20.1");
                    alar.AlarmName = "溢油报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(oilSpiA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 钥匙报警
                    var keyA = plc.Read("M20.2");
                    alar.AlarmName = "钥匙报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(keyA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 可燃报警
                    var combusA = plc.Read("M20.3");
                    alar.AlarmName = "";
                    alar.AlarmName = "可燃气体报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(combusA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 到位报警
                    var InplaceA = plc.Read("M20.4");
                    alar.AlarmName = "鹤管连接未到位报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(InplaceA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 归位报警
                    var HomingA = plc.Read("M20.5");
                    alar.AlarmName = "鹤管未归位报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(HomingA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 气流报警
                    var AirflowA = plc.Read("M20.6");
                    alar.AlarmName = "气相流量报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(AirflowA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 气溢报警
                    var AerorrheaA = plc.Read("M20.7");
                    alar.AlarmName = "气相溢出报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(AerorrheaA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 急停报警
                    var StopA = plc.Read("M21.0");
                    alar.AlarmName = "紧急停机报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(StopA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 关阀报警
                    var ValveshutA = plc.Read("M18.0");
                    alar.AlarmName = "关阀报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(ValveshutA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 阻车器报警
                    var TrajamA = plc.Read("M21.2");
                    alar.AlarmName = "阻车器未到位报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(TrajamA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 人员在岗报警
                    var OndutyA = plc.Read("M21.3");
                    alar.AlarmName = "人员在岗报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(OndutyA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 零速报警
                    var ZerospeA = plc.Read("M21.4");
                    alar.AlarmName = "零流速报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(ZerospeA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 温度报警
                    var heatA = plc.Read("M21.5");
                    alar.AlarmName = "温度超限报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(heatA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 压力报警
                    var presA = plc.Read("M21.6");
                    alar.AlarmName = "压力超限报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(presA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 密度报警
                    var densityA = plc.Read("M21.7");
                    alar.AlarmName = "密度超限报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(densityA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 液位报警
                    var levelA = plc.Read("M21.1");
                    alar.AlarmName = "液位超限报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(levelA), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region A鹤位预留
                    ////远程急停
                    //var EMStop = plc.Read("Q2.0");
                    ////鹤位状态
                    //var status = plc.Read("DB1.DBB0");
                    #endregion
                }
                else if (dev.CraneNO == "B")
                {
                    #region 静电报警
                    var elecB = plc.Read("M22.0");
                    alar.AlarmName = "静电报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(elecB), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 溢油报警
                    var oilSpiB = plc.Read("M22.1");
                    alar.AlarmName = "溢油报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(oilSpiB), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 钥匙报警
                    var keyB = plc.Read("M22.2");
                    alar.AlarmName = "钥匙报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(keyB), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 可燃报警
                    var combusB = plc.Read("M22.3");
                    alar.AlarmName = "可燃报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(combusB), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 到位报警
                    var InplaceB = plc.Read("M22.4");
                    alar.AlarmName = "鹤管连接未到位报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(InplaceB), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 归位报警
                    var HomingB = plc.Read("M22.5");
                    alar.AlarmType = 2;
                    alar.AlarmName = "鹤管未归位报警";
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(HomingB), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 气相流量报警
                    var AirflowB = plc.Read("M22.6");
                    alar.AlarmName = "气相流量报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(AirflowB), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 气溢报警
                    var AerorrheaB = plc.Read("M22.7");
                    alar.AlarmName = "气相溢出报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(AerorrheaB), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 急停报警
                    var StopB = plc.Read("M23.0");
                    alar.AlarmName = "紧急停机报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(StopB), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 关阀报警
                    var ValveshutB = plc.Read("M19.0");
                    alar.AlarmName = "关阀报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(ValveshutB), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 阻车器报警
                    var TrajamB = plc.Read("M23.2");
                    alar.AlarmName = "阻车器未到位报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(TrajamB), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 人员在岗
                    var OndutyB = plc.Read("M23.3");
                    alar.AlarmName = "人员在岗报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(OndutyB), alar.AlarmName, alar.AlarmName);
                    #endregion
                    #region 零速报警
                    var ZerospeB = plc.Read("M23.4");
                    alar.AlarmName = "零流速报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(ZerospeB), alar.AlarmName, alar.AlarmName);
                    #endregion
                    //温度
                    var heatB = plc.Read("M23.5");
                      alar.AlarmName = "温度超限报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(heatB), alar.AlarmName, alar.AlarmName);
                    //压力
                    var presB = plc.Read("M23.6");
                    alar.AlarmName = "压力超限报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(presB), alar.AlarmName, alar.AlarmName);
                    //密度
                    var densityB = plc.Read("M23.7");
                    alar.AlarmName = "密度超限报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(densityB), alar.AlarmName, alar.AlarmName);
                    //液位
                    var levelB = plc.Read("M23.1");
                    alar.AlarmName = "液位超限报警";
                    alar.AlarmType = 2;
                    WriteAlarmInfo(dev, alar, Convert.ToBoolean(levelB), alar.AlarmName, alar.AlarmName);


                }
            }
            catch (Exception ex)
            {
                LogerHelper.Error("Alarmlog:", ex);
                isRunning = true;
                return;
            }
            finally
            {
                plc.Close();
            }
        }

        private static void WriteAlarmInfo(SYSDeviceModel dev, AlarmInfo alar, bool alarm,string alName,string msg) 
        {
            alar.AlarmName = alName;
            alar.Flag = 0;
            if (alarm == true)
            {
                alar.AlarmDes = "鹤位：" + dev.MachineNo + dev.CraneNO + msg;
                alar.Flag = 0;
            }
            int eleccount = devser.AlarmsList(alar).Count();
            if (eleccount == 1 && alarm == false)
            {
                alar.Flag = 1;
                devser.AlarmEdit(alar);
            }
            else
            {
                if (alarm == true && eleccount == 0)
                {
                    alar.AlarmValue = "1";
                    devser.AlarmAdd(alar);
                }
            }
        }

        private static void AlarmLogError(SYSDeviceModel dev) 
        {
            try
            {
                AlarmInfo alar = new AlarmInfo();
                alar.DeviceNO = dev.MachineNo;
                alar.TagNo = dev.CraneNO;
                alar.Flag = 0;
                if (devser.AlarmsList(alar).Count() == 0)
                {
                    alar.OrderNo = dev.OrderNumber;
                    alar.AlarmName = "网络故障";
                    alar.AlarmType = 1;
                    alar.AlarmDes = "鹤位：" + dev.MachineNo + dev.CraneNO + "网络连接失败。";
                    alar.UploadID = 0;
                    alar.Note = " ";
                    alar.AlarmValue = "462";
                    if (devser.AlarmAdd(alar) == 1)
                    {
                        LogerHelper.Info("鹤位:" + dev.MachineNo + dev.CraneNO + "网络连接失败,写入成功。");
                    }
                    else
                    {
                        LogerHelper.Error("鹤位:" + dev.MachineNo + dev.CraneNO + "网络连接失败，写入失败。");
                    }
                }
            }
            catch (Exception ex) 
            {
                LogerHelper.Error("AlarmLogErr", ex);
            }
        }

        public static void Dispose()
        {
            isRunning = false;
            if (TaskMain != null)
                TaskMain.ConfigureAwait(true);
        }
    }
}

