using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.WebRequestMethods;

namespace PLCSystem.Config
{
    public class ConfigInfo
    {
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(
                string section, //节名
                string key,     //键名
                string val,     //键值
                string filePath //INN文件路径
            );

      

        [System.Runtime.InteropServices.DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern long GetPrivateProfileString(
                   string section, //节名
                   string key,     //键名
                   string def,     //默认值  若指定值不存在，则该值作为默认值
                   StringBuilder retVal,  ///读取的缓冲区
                   int size,   //缓存区的大小
                   string filePath  //读取的路径
            );


        #region Ini文件操作
        public static string ReadIniString(string section, string key)
        {
            int buffer = 6000;
            StringBuilder temp = new StringBuilder(buffer);
            GetPrivateProfileString(section, key, "", temp, buffer, Path.GetFullPath("SystemParam.ini"));
            return temp.ToString();
        }
        public static void WriteIniString(string section, string key,string val)
        {
            WritePrivateProfileString(section, key, val, Path.GetFullPath("SystemParam.ini"));
        }
        #endregion
        public static void SaveLeftIniFile()
        {
            WriteIniString("LeftLayout", "温度", "1");
            WriteIniString("LeftLayout", "密度", "1");
            WriteIniString("LeftLayout", "压力", "1");
            WriteIniString("LeftLayout", "液位", "1");
        }
        public static void SaveUnitIniFile()
        {
            WriteIniString("UnitConfig", "预装量", "Kg");
            WriteIniString("UnitConfig", "实装量", "Kg");
            WriteIniString("UnitConfig", "瞬时流量", "T/h");
            WriteIniString("UnitConfig", "累计流量", "T");
            WriteIniString("UnitConfig", "温度", "℃");
            WriteIniString("UnitConfig", "密度", "Kg/m³");
            WriteIniString("UnitConfig", "压力", "MPa");
            WriteIniString("UnitConfig", "液位", "m");
        }
        public static void SaveAlarmIniFile()
        {
            WriteIniString("AlarmConfig", "第1位", "静电");
            WriteIniString("AlarmConfig", "第2位", "溢油");
            WriteIniString("AlarmConfig", "第3位", "钥匙");
            WriteIniString("AlarmConfig", "第4位", "可燃");
            WriteIniString("AlarmConfig", "第5位", "到位");
            WriteIniString("AlarmConfig", "第6位", "归位");
            WriteIniString("AlarmConfig", "第7位", "气流");
            WriteIniString("AlarmConfig", "第8位", "气溢");
            WriteIniString("AlarmConfig", "第9位", "急停");
            WriteIniString("AlarmConfig", "第10位", "关阀");
            WriteIniString("AlarmConfig", "第11位", "阻车");
            WriteIniString("AlarmConfig", "第12位", "在岗");
            WriteIniString("AlarmConfig", "第13位", "零速");
        }




    }
}
