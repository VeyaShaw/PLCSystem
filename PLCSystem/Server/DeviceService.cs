using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using PLCSystem.DAL;
using PLCSystem.Models;

namespace PLCSystem.Service
{
    public class DeviceService
    {

        private static DataAccess data = new DataAccess();

        private static List<string> itemList = new List<string>();



        public static int DeviceCount()
        {

            return data.DeviceCount();
        }
        public static List<SYSCraneSetModel> TotalDeviceCount()
        {
            return data.CraneSetCount();
        }
        public static List<SYSDeviceModel> deviceList(SYSDeviceModel devm, int bustype)
        {
            return data.deviceList(devm, bustype);
        }
        public static List<SYSConfigModel> ConfigList()
        {
            return data.ConfigList();
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        public int DeviceCallPolice(SYSDeviceModel dev)
        {
            return data.DeviceCallPolice(dev);
        }

        public int DeviceExecuteOrder(SYSDeviceModel dev)
        {
            return data.DeviceExecuteOrder(dev);
        }

        public static List<OrderInfoModels> OrderListQuery(OrderInfoModels order)
        {
            return data.OrderListQuery(order);
        }

        public List<SYSDeviceModel> Commdata(string IP)
        {
            return data.Commdata(IP);
        }
        public List<AlarmInfo> AlarmsList(AlarmInfo Alar)
        {
            return data.AlarmsList(Alar);
        }
        public int AlarmAdd(AlarmInfo Alar) 
        {
            return data.AlarmAdd(Alar);
        }

        public int AlarmEdit(AlarmInfo Alar) 
        {
            return data.AlarmEdit(Alar);
        }

    }
}
