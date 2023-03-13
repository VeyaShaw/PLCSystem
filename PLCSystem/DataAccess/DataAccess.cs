using Microsoft.VisualBasic;
using PLCSystem.Common;
using PLCSystem.Common.Log;
using PLCSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PLCSystem.DAL
{
    public class DataAccess
    {
        private string connStr =DESEncrypt.Decrypt(ConfigurationManager.ConnectionStrings["Connstr"].ConnectionString);

        public SqlCommand Comm { get; set; }

        public int DeviceCount()
        {
            int count = 0;
            using (SqlConnection Conn = new SqlConnection())
            {


                try
                {
                    Conn.ConnectionString = connStr;
                    Conn.Open();
                    Comm = new SqlCommand();
                    Comm.Connection = Conn;
                    Comm.CommandText = string.Format("Select  Count(*) from  SYSCraneSettings  where  Status = '1'");
                    count = (int)Comm.ExecuteScalar();
                    LogerHelper.Info("initializeInDeviceCount:" + count);
                }
                catch (Exception ex)
                {
                    LogerHelper.Error("DeviceCountError", ex);
                    MessageBox.Show("DeviceCountError:" + ex.Message, "温馨提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    //Application.Current.Shutdown();

                }
                finally
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
            return count;
        }

        public List<SYSConfigModel> ConfigList()
        {
            List<SYSConfigModel> configList = new List<SYSConfigModel>();
            using (SqlConnection Conn = new SqlConnection())
            {
                try
                {
                    Conn.ConnectionString = connStr;
                    Conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = Conn;
                        StringBuilder sql = new StringBuilder("select * from   [dbo].[SYSConfig]  where ConfigType = '报警'   and Status = '1' order by   Priority ");
                        cmd.CommandText = sql.ToString();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SYSConfigModel conf = new SYSConfigModel();
                                if (dr["ConfigCode"] != DBNull.Value)
                                {
                                    conf.ConfigCode = (int)dr["ConfigCode"];
                                }
                                if (dr["ConfigName"] != DBNull.Value)
                                {
                                    conf.ConfigName = (string)dr["ConfigName"];
                                }
                                if (dr["ConfigType"] != DBNull.Value)
                                {
                                    conf.ConfigType = (string)dr["ConfigType"];
                                }
                                if (dr["Rowscount"] != DBNull.Value)
                                {
                                    conf.Rowscount = (int)dr["Rowscount"];
                                }
                                if (dr["CollCount"] != DBNull.Value)
                                {
                                    conf.Collcount = (int)dr["CollCount"];
                                }
                                if (dr["Priority"] != DBNull.Value)
                                {
                                    conf.Priority = (int)dr["Priority"];
                                }
                                if (dr["Status"] != DBNull.Value)
                                {
                                    conf.Status = (int)dr["Status"];
                                }
                               
                                if (dr["Note"] != DBNull.Value)
                                {
                                    conf.Note = (string)dr["Note"];
                                }
                                configList.Add(conf);
                            }
                            dr.Close();

                        }
                    }
                }
                catch (Exception ex)
                {
                    LogerHelper.Error("Configitem：", ex);
                }
                finally
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
            return configList;
        }

        public List<SYSConfigModel> SearchConfig(string AlarmA, string AlarmB)
        {
            List<SYSConfigModel> configList = new List<SYSConfigModel>();
            using (SqlConnection Conn = new SqlConnection())
            {
                Conn.ConnectionString = connStr;
                Conn.Open();
                try
                {

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = Conn;
                        StringBuilder sql = new StringBuilder("select * from[dbo].[SYSConfig]   order by   Priority ");


                        if (AlarmA != null && AlarmB != null)
                        {
                            sql.Append("  where ConfigType = '报警'   and Status = '1'     and   ConfigName  in (' " + AlarmA + "','" + AlarmB + "')     order by  Priority  ");
                        }


                        cmd.CommandText = sql.ToString();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SYSConfigModel conf = new SYSConfigModel();
                                if (dr["ConfigCode"] != DBNull.Value)
                                {
                                    conf.ConfigCode = (int)dr["ConfigCode"];
                                }
                                if (dr["ConfigName"] != DBNull.Value)
                                {
                                    conf.ConfigName = (string)dr["ConfigName"];
                                }
                                if (dr["ConfigType"] != DBNull.Value)
                                {
                                    conf.ConfigType = (string)dr["ConfigType"];
                                }
                                if (dr["Priority"] != DBNull.Value)
                                {
                                    conf.Priority = (int)dr["Priority"];
                                }
                                if (dr["Status"] != DBNull.Value)
                                {
                                    conf.Status = (int)dr["Status"];
                                }
                                if (dr["Note"] != DBNull.Value)
                                {
                                    conf.Note = (string)dr["Note"];
                                }
                                configList.Add(conf);
                            }
                            dr.Close();

                        }
                    }

                }
                catch (Exception ex)
                {
                    LogerHelper.Error("Configitem：", ex);
                }
                finally
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
            return configList;
        }

        public List<SYSCraneSetModel> CraneSetCount()
        {
            List<SYSCraneSetModel> cranList = new List<SYSCraneSetModel>();
            using (SqlConnection Conn = new SqlConnection())
            {

                try
                {
                    Conn.ConnectionString = connStr;
                    Conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = Conn;
                        StringBuilder sql = new StringBuilder("Select  *   from  SYSCraneSettings  where    Status = '1'  order by  CraneCode ");
                        cmd.CommandText = sql.ToString();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SYSCraneSetModel set = new SYSCraneSetModel();
                                if (dr["CraneCode"] != DBNull.Value)
                                {
                                    set.CraneCode = (int)dr["CraneCode"];
                                }
                                if (dr["MachineNo"] != DBNull.Value)
                                {
                                    set.MachineNo = (int)dr["MachineNo"];
                                }
                                if (dr["CraneNO"] != DBNull.Value)
                                {
                                    set.CraneNO = (string)dr["CraneNO"];
                                }
                                if (dr["Status"] != DBNull.Value)
                                {
                                    set.Status = (int)dr["Status"];
                                }
                                cranList.Add(set);
                            }
                            dr.Close();
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogerHelper.Error("CraneSetitem", ex);

                }
                finally
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
            return cranList;
        }

        public List<SYSCraneSetModel> CraneSetList(int StartPage, int EndPage)
        {
            List<SYSCraneSetModel> cranList = new List<SYSCraneSetModel>();
            using (SqlConnection Conn = new SqlConnection())
            {
                Conn.ConnectionString = connStr;
                Conn.Open();
                try
                {

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = Conn;
                        StringBuilder sql = new StringBuilder("Select *  From  SYSCraneSettings ");

                        if (StartPage != 0 && EndPage != 0)
                        {
                            sql.Append(" where CraneCode between " + StartPage + "  and  " + EndPage + "  and Status = '1'   order by  MachineNo  ");
                        }


                        cmd.CommandText = sql.ToString();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SYSCraneSetModel set = new SYSCraneSetModel();
                                if (dr["CraneCode"] != DBNull.Value)
                                {
                                    set.CraneCode = (int)dr["CraneCode"];
                                }
                                if (dr["MachineNo"] != DBNull.Value)
                                {
                                    set.MachineNo = (int)dr["MachineNo"];
                                }
                                if (dr["CraneNO"] != DBNull.Value)
                                {
                                    set.CraneNO = (string)dr["CraneNO"];
                                }
                                if (dr["Status"] != DBNull.Value)
                                {
                                    set.Status = (int)dr["Status"];
                                }
                                cranList.Add(set);
                            }
                            dr.Close();
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogerHelper.Error("CraneSetitem", ex);

                }
                finally
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }


            return cranList;
        }

        public List<SYSDeviceModel> deviceList(SYSDeviceModel devm, int bustype)
        {
            List<SYSDeviceModel> devivcelist = new List<SYSDeviceModel>();
            using (SqlConnection Conn = new SqlConnection())
            {

                try
                {
                    Conn.ConnectionString = connStr;
                    Conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = Conn;
                        StringBuilder sql = new StringBuilder("select * from [dbo].[SYSDeviceInfo]      ");
                        List<string> wheres = new List<string>();
                        if (bustype == 1)
                        {
                            if (devm.MachineNo != 0)
                            {
                                wheres.Add(" MachineNo='" + devm.MachineNo + "'");
                            }
                            if (devm.CraneNO != null)
                            {
                                wheres.Add(" CraneNO='" + devm.CraneNO.Split('|')[0] + "'");
                            }

                            if (wheres.Count > 0)
                            {
                                string wh = string.Join(" and ", wheres.ToArray());
                                sql.Append(" where " + wh + " and ThreadGroup='0'   order by  MachineNo  ");
                                cmd.CommandText = sql.ToString();
                            }
                        }
                        else if (bustype == 2)
                        {

                            if (devm.MachineNo != 0)
                            {
                                wheres.Add(" MachineNo='" + devm.MachineNo + "'");
                            }
                            if (devm.CraneNO != null)
                            {
                                wheres.Add(" CraneNO='" + "A" + "'");
                            }

                            if (wheres.Count > 0)
                            {
                                string wh = string.Join(" and ", wheres.ToArray());
                                sql.Append(" where " + wh + " and ThreadGroup='0'   order by  MachineNo  ");
                                cmd.CommandText = sql.ToString();
                            }
                        }
                        else if (bustype == 3) 
                        {
                            if (devm.MachineNo != 0)
                            {
                                wheres.Add(" MachineNo='" + devm.MachineNo + "'");
                            }
                            if (devm.CraneNO != null)
                            {
                                wheres.Add(" CraneNO='" + "B" + "'");
                            }

                            if (wheres.Count > 0)
                            {
                                string wh = string.Join(" and ", wheres.ToArray());
                                sql.Append(" where " + wh + " and ThreadGroup='0'   order by  MachineNo  ");
                                cmd.CommandText = sql.ToString();
                            }
                        }




                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SYSDeviceModel dev = new SYSDeviceModel();

                                if (dr["DeviceNO"] != DBNull.Value)
                                {
                                    dev.DeviceNO = (int)dr["DeviceNO"];
                                }
                                if (dr["MachineNo"] != DBNull.Value)
                                {
                                    dev.MachineNo = (int)dr["MachineNo"];
                                }
                                if (dr["MaName"] != DBNull.Value)
                                {
                                    dev.MaName = (string)dr["MaName"];
                                }
                                if (dr["CraneNO"] != DBNull.Value)
                                {
                                    dev.CraneNO = (string)dr["CraneNO"];
                                }
                                if (dr["CrName"] != DBNull.Value)
                                {
                                    dev.CrName = (string)dr["CrName"];
                                }
                                if (dr["MaNameal"] != DBNull.Value)
                                {
                                    dev.MaNameal = (string)dr["MaNameal"];
                                }
                                if (dr["IP"] != DBNull.Value)
                                {
                                    dev.IP = (string)dr["IP"];
                                }
                                if (dr["constituen"] != DBNull.Value)
                                {
                                    dev.constituen = (string)dr["constituen"];
                                }
                                if (dr["CardNo"] != DBNull.Value)
                                {
                                    dev.CardNo = (string)dr["CardNo"];
                                }
                                if (dr["OrderNumber"] != DBNull.Value)
                                {
                                    dev.OrderNumber = (string)dr["OrderNumber"];
                                }
                                if (dr["OrName"] != DBNull.Value)
                                {
                                    dev.OrName = (string)dr["OrName"];
                                }
                                if (dr["CarNumber"] != DBNull.Value)
                                {
                                    dev.CarNumber = (string)dr["CarNumber"];
                                }
                                if (dr["CNName"] != DBNull.Value)
                                {
                                    dev.CNName = (string)dr["CNName"];
                                }
                                if (dr["PlannedVolume"] != DBNull.Value)
                                {
                                    dev.PlannedVolume = (decimal)dr["PlannedVolume"];
                                }
                                if (dr["PVName"] != DBNull.Value)
                                {
                                    dev.PVName = (string)dr["PVName"];
                                }
                                if (dr["AmountInstall"] != DBNull.Value)
                                {
                                    dev.AmountInstall = (decimal)dr["AmountInstall"];
                                }
                                if (dr["AIName"] != DBNull.Value)
                                {
                                    dev.AIName = (string)dr["AIName"];
                                }
                                if (dr["InsTraffic"] != DBNull.Value)
                                {
                                    dev.InsTraffic = (decimal)dr["InsTraffic"];
                                }
                                if (dr["ITName"] != DBNull.Value)
                                {
                                    dev.ITName = (string)dr["ITName"];
                                }
                                if (dr["Accumulate"] != DBNull.Value)
                                {
                                    dev.Accumulate = (decimal)dr["Accumulate"];
                                }
                                if (dr["AName"] != DBNull.Value)
                                {
                                    dev.AName = (string)dr["AName"];
                                }

                                if (dr["Temperature"] != DBNull.Value)
                                {
                                    dev.Temperature = (decimal)dr["Temperature"];
                                }

                                if (dr["TName"] != DBNull.Value)
                                {
                                    dev.TName = (string)dr["TName"];
                                }

                                if (dr["Denser"] != DBNull.Value)
                                {
                                    dev.Denser = (decimal)dr["Denser"];
                                }
                                if (dr["DName"] != DBNull.Value)
                                {
                                    dev.DName = (string)dr["DName"];
                                }
                                if (dr["Pressure"] != DBNull.Value)
                                {
                                    dev.Pressure = (decimal)dr["Pressure"];
                                }
                                if (dr["PName"] != DBNull.Value)
                                {
                                    dev.PName = (string)dr["PName"];
                                }
                                if (dr["LiName"] != DBNull.Value)
                                {
                                    dev.LiName = (string)dr["LiName"];
                                }
                                if (dr["AlarmA"] != DBNull.Value)
                                {
                                    dev.AlarmA = (int)dr["AlarmA"];
                                }
                                if (dr["AlarmAName"] != DBNull.Value)
                                {
                                    dev.AlarmAName = (string)dr["AlarmAName"];
                                }
                                if (dr["AlarmB"] != DBNull.Value)
                                {
                                    dev.AlarmB = (int)dr["AlarmB"];
                                }
                                if (dr["AlarmBName"] != DBNull.Value)
                                {
                                    dev.AlarmBName = (string)dr["AlarmBName"];
                                }
                                if (dr["AlarmC"] != DBNull.Value)
                                {
                                    dev.AlarmC = (int)dr["AlarmC"];
                                }
                                if (dr["AlarmCName"] != DBNull.Value)
                                {
                                    dev.AlarmCName = (string)dr["AlarmCName"];
                                }
                                if (dr["AlarmD"] != DBNull.Value)
                                {
                                    dev.AlarmD = (int)dr["AlarmD"];
                                }
                                if (dr["AlarmDName"] != DBNull.Value)
                                {
                                    dev.AlarmDName = (string)dr["AlarmDName"];
                                }
                                if (dr["AlarmE"] != DBNull.Value)
                                {
                                    dev.AlarmE = (int)dr["AlarmE"];
                                }
                                if (dr["AlarmEName"] != DBNull.Value)
                                {
                                    dev.AlarmEName = (string)dr["AlarmEName"];
                                }
                                if (dr["AlarmF"] != DBNull.Value)
                                {
                                    dev.AlarmF = (int)dr["AlarmF"];
                                }
                                if (dr["AlarmFName"] != DBNull.Value)
                                {
                                    dev.AlarmFName = (string)dr["AlarmFName"];
                                }
                                if (dr["AlarmG"] != DBNull.Value)
                                {
                                    dev.AlarmG = (int)dr["AlarmG"];
                                }
                                if (dr["AlarmGName"] != DBNull.Value)
                                {
                                    dev.AlarmGName = (string)dr["AlarmGName"];
                                }
                                if (dr["AlarmH"] != DBNull.Value)
                                {
                                    dev.AlarmH = (int)dr["AlarmH"];
                                }
                                if (dr["AlarmHName"] != DBNull.Value)
                                {
                                    dev.AlarmHName = (string)dr["AlarmHName"];
                                }
                                if (dr["AlarmI"] != DBNull.Value)
                                {
                                    dev.AlarmI = (int)dr["AlarmI"];
                                }
                                if (dr["AlarmI"] != DBNull.Value)
                                {
                                    dev.AlarmI = (int)dr["AlarmI"];
                                }
                                if (dr["AlarmI"] != DBNull.Value)
                                {
                                    dev.AlarmI = (int)dr["AlarmI"];
                                }
                                if (dr["AlarmIName"] != DBNull.Value)
                                {
                                    dev.AlarmIName = (string)dr["AlarmIName"];
                                }

                                if (dr["AlarmJ"] != DBNull.Value)
                                {
                                    dev.AlarmJ = (int)dr["AlarmJ"];
                                }
                                if (dr["AlarmJName"] != DBNull.Value)
                                {
                                    dev.AlarmJName = (string)dr["AlarmJName"];
                                }
                                if (dr["AlarmK"] != DBNull.Value)
                                {
                                    dev.AlarmK = (int)dr["AlarmK"];
                                }
                                if (dr["AlarmKName"] != DBNull.Value)
                                {
                                    dev.AlarmKName = (string)dr["AlarmKName"];
                                }
                                if (dr["AlarmL"] != DBNull.Value)
                                {
                                    dev.AlarmL = (int)dr["AlarmL"];
                                }
                                if (dr["AlarmLName"] != DBNull.Value)
                                {
                                    dev.AlarmLName = (string)dr["AlarmLName"];
                                }
                                if (dr["AlarmQ"] != DBNull.Value)
                                {
                                    dev.AlarmQ = (int)dr["AlarmQ"];
                                }
                                if (dr["AlarmQName"] != DBNull.Value)
                                {
                                    dev.AlarmQName = (string)dr["AlarmQName"];
                                }
                                if (dr["AlarmW"] != DBNull.Value)
                                {
                                    dev.AlarmW = (int)dr["AlarmW"];
                                }
                                if (dr["AlarmWName"] != DBNull.Value)
                                {
                                    dev.AlarmWName = (string)dr["AlarmWName"];
                                }
                                if (dr["AlarmR"] != DBNull.Value)
                                {
                                    dev.AlarmR = (int)dr["AlarmR"];
                                }
                                if (dr["AlarmRName"] != DBNull.Value)
                                {
                                    dev.AlarmRName = (string)dr["AlarmRName"];
                                }
                                if (dr["AlarmT"] != DBNull.Value)
                                {
                                    dev.AlarmT = (int)dr["AlarmT"];
                                }
                                if (dr["AlarmTName"] != DBNull.Value)
                                {
                                    dev.AlarmTName = (string)dr["AlarmTName"];
                                }
                                if (dr["AlarmO"] != DBNull.Value)
                                {
                                    dev.AlarmO = (int)dr["AlarmO"];
                                }
                                if (dr["AlarmOName"] != DBNull.Value)
                                {
                                    dev.AlarmOName = (string)dr["AlarmOName"];
                                }
                                if (dr["CraneStatus"] != DBNull.Value)
                                {
                                    dev.CraneStatus = (string)dr["CraneStatus"];
                                }
                                if (dr["StatusName"] != DBNull.Value)
                                {
                                    dev.StatusName = (string)dr["StatusName"];
                                }
                                if (dr["ItemCode"] != DBNull.Value)
                                {
                                    dev.ItemCode = (int)dr["ItemCode"];
                                }
                                if (dr["ItemName"] != DBNull.Value)
                                {
                                    dev.ItemName = (string)dr["ItemName"];
                                }
                                if (dr["ItemName"] != DBNull.Value)
                                {
                                    dev.ItemName = (string)dr["ItemName"];
                                }
                                if (dr["DeviceStatus"] != DBNull.Value)
                                {
                                    dev.DeviceStatus = (string)dr["DeviceStatus"];
                                }
                                if (dr["Pace"] != DBNull.Value)
                                {
                                    dev.Pace = (double)dr["Pace"];
                                }
                                if (dr["PaceName"] != DBNull.Value)
                                {
                                    dev.PaceName = (string)dr["PaceName"];
                                }
                                if (dr["Operate"] != DBNull.Value)
                                {
                                    dev.Operate = (int)dr["Operate"];
                                }
                                if (dr["OperateName"] != DBNull.Value)
                                {
                                    dev.OperateName = (string)dr["OperateName"];
                                }
                                if (dr["OperateName"] != DBNull.Value)
                                {
                                    dev.OperateName = (string)dr["OperateName"];
                                }
                                if (dr["EmeStop"] != DBNull.Value)
                                {
                                    dev.EmeStop = (int)dr["EmeStop"];
                                }

                                if (dr["EmeStopName"] != DBNull.Value)
                                {
                                    dev.EmeStopName = (string)dr["EmeStopName"];
                                }
                                devivcelist.Add(dev);
                            }
                            dr.Close();
                        }

                    }
                }
                catch (Exception ex)
                {
                    LogerHelper.Error("device", ex);
                    //MessageBox.Show("网络通讯故障："+ex.Message,"温馨提示",MessageBoxButton.OK,MessageBoxImage.Error);
                    //Application.Current.Shutdown();
                }
                finally
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
            return devivcelist;
        }

        public List<SYSDeviceModel> Commdata(string IP)
        {
            List<SYSDeviceModel> devivcelist = new List<SYSDeviceModel>();
            using (SqlConnection Conn = new SqlConnection())
            {

                try
                {
                    Conn.ConnectionString = connStr;
                    Conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = Conn;
                        StringBuilder sql = new StringBuilder("select  *   from [dbo].[SYSDeviceInfo]     ");


                        if (IP != null)
                        {
                            sql.Append(" where IP= '" + IP + "'   and DeviceStatus not in('0')   order by  MachineNo  ");
                        }

                        cmd.CommandText = sql.ToString();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SYSDeviceModel dev = new SYSDeviceModel();

                                if (dr["DeviceNO"] != DBNull.Value)
                                {
                                    dev.DeviceNO = (int)dr["DeviceNO"];
                                }
                                if (dr["MachineNo"] != DBNull.Value)
                                {
                                    dev.MachineNo = (int)dr["MachineNo"];
                                }
                                if (dr["MaName"] != DBNull.Value)
                                {
                                    dev.MaName = (string)dr["MaName"];
                                }
                                if (dr["CraneNO"] != DBNull.Value)
                                {
                                    dev.CraneNO = (string)dr["CraneNO"];
                                }
                                if (dr["CrName"] != DBNull.Value)
                                {
                                    dev.CrName = (string)dr["CrName"];
                                }
                                if (dr["IP"] != DBNull.Value)
                                {
                                    dev.IP = (string)dr["IP"];
                                }

                                if (dr["constituen"] != DBNull.Value)
                                {
                                    dev.constituen = (string)dr["constituen"];
                                }
                                if (dr["OrderNumber"] != DBNull.Value)
                                {
                                    dev.OrderNumber = (string)dr["OrderNumber"];
                                }
                                if (dr["OrName"] != DBNull.Value)
                                {
                                    dev.OrName = (string)dr["OrName"];
                                }
                                if (dr["CarNumber"] != DBNull.Value)
                                {
                                    dev.CarNumber = (string)dr["CarNumber"];
                                }
                                if (dr["CNName"] != DBNull.Value)
                                {
                                    dev.CNName = (string)dr["CNName"];
                                }
                                if (dr["PlannedVolume"] != DBNull.Value)
                                {
                                    dev.PlannedVolume = (decimal)dr["PlannedVolume"];
                                }
                                if (dr["PVName"] != DBNull.Value)
                                {
                                    dev.PVName = (string)dr["PVName"];
                                }
                                if (dr["AmountInstall"] != DBNull.Value)
                                {
                                    dev.AmountInstall = (decimal)dr["AmountInstall"];
                                }
                                if (dr["AIName"] != DBNull.Value)
                                {
                                    dev.AIName = (string)dr["AIName"];
                                }
                                if (dr["InsTraffic"] != DBNull.Value)
                                {
                                    dev.InsTraffic = (decimal)dr["InsTraffic"];
                                }
                                if (dr["ITName"] != DBNull.Value)
                                {
                                    dev.ITName = (string)dr["ITName"];
                                }
                                if (dr["Accumulate"] != DBNull.Value)
                                {
                                    dev.Accumulate = (decimal)dr["Accumulate"];
                                }
                                if (dr["AName"] != DBNull.Value)
                                {
                                    dev.AName = (string)dr["AName"];
                                }

                                if (dr["Temperature"] != DBNull.Value)
                                {
                                    dev.Temperature = (decimal)dr["Temperature"];
                                }

                                if (dr["TName"] != DBNull.Value)
                                {
                                    dev.TName = (string)dr["TName"];
                                }

                                if (dr["Denser"] != DBNull.Value)
                                {
                                    dev.Denser = (decimal)dr["Denser"];
                                }
                                if (dr["DName"] != DBNull.Value)
                                {
                                    dev.DName = (string)dr["DName"];
                                }
                                if (dr["Pressure"] != DBNull.Value)
                                {
                                    dev.Pressure = (decimal)dr["Pressure"];
                                }
                                if (dr["PName"] != DBNull.Value)
                                {
                                    dev.PName = (string)dr["PName"];
                                }
                                if (dr["AlarmA"] != DBNull.Value)
                                {
                                    dev.AlarmA = (int)dr["AlarmA"];
                                }
                                if (dr["AlarmAName"] != DBNull.Value)
                                {
                                    dev.AlarmAName = (string)dr["AlarmAName"];
                                }
                                if (dr["AlarmB"] != DBNull.Value)
                                {
                                    dev.AlarmB = (int)dr["AlarmB"];
                                }
                                if (dr["AlarmBName"] != DBNull.Value)
                                {
                                    dev.AlarmBName = (string)dr["AlarmBName"];
                                }
                                if (dr["AlarmC"] != DBNull.Value)
                                {
                                    dev.AlarmC = (int)dr["AlarmC"];
                                }
                                if (dr["AlarmCName"] != DBNull.Value)
                                {
                                    dev.AlarmCName = (string)dr["AlarmCName"];
                                }
                                if (dr["AlarmD"] != DBNull.Value)
                                {
                                    dev.AlarmD = (int)dr["AlarmD"];
                                }
                                if (dr["AlarmDName"] != DBNull.Value)
                                {
                                    dev.AlarmDName = (string)dr["AlarmDName"];
                                }
                                if (dr["AlarmE"] != DBNull.Value)
                                {
                                    dev.AlarmE = (int)dr["AlarmE"];
                                }
                                if (dr["AlarmEName"] != DBNull.Value)
                                {
                                    dev.AlarmEName = (string)dr["AlarmEName"];
                                }
                                if (dr["AlarmF"] != DBNull.Value)
                                {
                                    dev.AlarmF = (int)dr["AlarmF"];
                                }
                                if (dr["AlarmFName"] != DBNull.Value)
                                {
                                    dev.AlarmFName = (string)dr["AlarmFName"];
                                }
                                if (dr["AlarmG"] != DBNull.Value)
                                {
                                    dev.AlarmG = (int)dr["AlarmG"];
                                }
                                if (dr["AlarmGName"] != DBNull.Value)
                                {
                                    dev.AlarmGName = (string)dr["AlarmGName"];
                                }
                                if (dr["AlarmH"] != DBNull.Value)
                                {
                                    dev.AlarmH = (int)dr["AlarmH"];
                                }
                                if (dr["AlarmHName"] != DBNull.Value)
                                {
                                    dev.AlarmHName = (string)dr["AlarmHName"];
                                }
                                if (dr["AlarmI"] != DBNull.Value)
                                {
                                    dev.AlarmI = (int)dr["AlarmI"];
                                }
                                if (dr["AlarmI"] != DBNull.Value)
                                {
                                    dev.AlarmI = (int)dr["AlarmI"];
                                }
                                if (dr["AlarmI"] != DBNull.Value)
                                {
                                    dev.AlarmI = (int)dr["AlarmI"];
                                }
                                if (dr["AlarmIName"] != DBNull.Value)
                                {
                                    dev.AlarmIName = (string)dr["AlarmIName"];
                                }

                                if (dr["AlarmJ"] != DBNull.Value)
                                {
                                    dev.AlarmJ = (int)dr["AlarmJ"];
                                }
                                if (dr["AlarmJName"] != DBNull.Value)
                                {
                                    dev.AlarmJName = (string)dr["AlarmJName"];
                                }
                                if (dr["AlarmK"] != DBNull.Value)
                                {
                                    dev.AlarmK = (int)dr["AlarmK"];
                                }
                                if (dr["AlarmKName"] != DBNull.Value)
                                {
                                    dev.AlarmKName = (string)dr["AlarmKName"];
                                }
                                if (dr["AlarmL"] != DBNull.Value)
                                {
                                    dev.AlarmL = (int)dr["AlarmL"];
                                }
                                if (dr["AlarmLName"] != DBNull.Value)
                                {
                                    dev.AlarmLName = (string)dr["AlarmLName"];
                                }
                                if (dr["AlarmQ"] != DBNull.Value)
                                {
                                    dev.AlarmQ = (int)dr["AlarmQ"];
                                }
                                if (dr["AlarmQName"] != DBNull.Value)
                                {
                                    dev.AlarmQName = (string)dr["AlarmQName"];
                                }
                                if (dr["AlarmW"] != DBNull.Value)
                                {
                                    dev.AlarmW = (int)dr["AlarmW"];
                                }
                                if (dr["AlarmWName"] != DBNull.Value)
                                {
                                    dev.AlarmWName = (string)dr["AlarmWName"];
                                }
                                if (dr["AlarmR"] != DBNull.Value)
                                {
                                    dev.AlarmR = (int)dr["AlarmR"];
                                }
                                if (dr["AlarmRName"] != DBNull.Value)
                                {
                                    dev.AlarmRName = (string)dr["AlarmRName"];
                                }
                                if (dr["AlarmT"] != DBNull.Value)
                                {
                                    dev.AlarmT = (int)dr["AlarmT"];
                                }
                                if (dr["AlarmTName"] != DBNull.Value)
                                {
                                    dev.AlarmTName = (string)dr["AlarmTName"];
                                }
                                if (dr["AlarmO"] != DBNull.Value)
                                {
                                    dev.AlarmO = (int)dr["AlarmO"];
                                }
                                if (dr["AlarmOName"] != DBNull.Value)
                                {
                                    dev.AlarmOName = (string)dr["AlarmOName"];
                                }
                                if (dr["CraneStatus"] != DBNull.Value)
                                {
                                    dev.CraneStatus = (string)dr["CraneStatus"];
                                }
                                if (dr["StatusName"] != DBNull.Value)
                                {
                                    dev.StatusName = (string)dr["StatusName"];
                                }
                                if (dr["ItemCode"] != DBNull.Value)
                                {
                                    dev.ItemCode = (int)dr["ItemCode"];
                                }
                                if (dr["ItemName"] != DBNull.Value)
                                {
                                    dev.ItemName = (string)dr["ItemName"];
                                }
                                if (dr["ItemName"] != DBNull.Value)
                                {
                                    dev.ItemName = (string)dr["ItemName"];
                                }
                                if (dr["DeviceStatus"] != DBNull.Value)
                                {
                                    dev.DeviceStatus = (string)dr["DeviceStatus"];
                                }
                                if (dr["Pace"] != DBNull.Value)
                                {
                                    dev.Pace = (double)dr["Pace"];
                                }
                                if (dr["PaceName"] != DBNull.Value)
                                {
                                    dev.PaceName = (string)dr["PaceName"];
                                }
                                if (dr["Operate"] != DBNull.Value)
                                {
                                    dev.Operate = (int)dr["Operate"];
                                }
                                if (dr["OperateName"] != DBNull.Value)
                                {
                                    dev.OperateName = (string)dr["OperateName"];
                                }
                                if (dr["OperateName"] != DBNull.Value)
                                {
                                    dev.OperateName = (string)dr["OperateName"];
                                }
                                if (dr["EmeStop"] != DBNull.Value)
                                {
                                    dev.EmeStop = (int)dr["EmeStop"];
                                }

                                if (dr["EmeStopName"] != DBNull.Value)
                                {
                                    dev.EmeStopName = (string)dr["EmeStopName"];
                                }
                                devivcelist.Add(dev);
                            }
                            dr.Close();
                        }

                    }
                }
                catch (Exception ex)
                {
                    LogerHelper.Error("device", ex);
                }
                finally
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
            return devivcelist;
        }

        public List<SYSDeviceModel> DeviceAddress(int StartPage, int EndPage)
        {
            List<SYSDeviceModel> devivcelist = new List<SYSDeviceModel>();
            using (SqlConnection Conn = new SqlConnection())
            {

                try
                {
                    Conn.ConnectionString = connStr;
                    Conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = Conn;
                        StringBuilder sql = new StringBuilder("Select DISTINCT MachineNo,IP  from   SYSDeviceInfo     ");


                        if (StartPage != 0 && EndPage != 0)
                        {
                            sql.Append(" where DeviceNO between " + StartPage + "  and  " + EndPage + "  and DeviceStatus not in( '0')   order by  MachineNo  ");
                        }

                        cmd.CommandText = sql.ToString();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                SYSDeviceModel dev = new SYSDeviceModel();
                                if (dr["MachineNo"] != DBNull.Value)
                                {
                                    dev.MachineNo = (int)dr["MachineNo"];
                                }
                                if (dr["IP"] != DBNull.Value)
                                {
                                    dev.IP = (string)dr["IP"];
                                }
                                devivcelist.Add(dev);
                            }
                            dr.Close();
                        }

                    }
                }
                catch (Exception ex)
                {
                    LogerHelper.Error("device", ex);
                }
                finally
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
            return devivcelist;
        }

        public int DeviceCallPolice(SYSDeviceModel dev)
        {
            int count = 0;
            using (SqlConnection conn = new SqlConnection())
            {
                try
                {
                    conn.ConnectionString = connStr;
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        StringBuilder sql = new StringBuilder("update SYSDeviceInfo  SET  ");
                        List<string> wheres = new List<string>();
                        if (dev.CraneStatus != "")
                        {
                            wheres.Add(" CraneStatus='" + dev.CraneStatus + "',");
                        }
                        if (dev.OrderNumber != "")
                        {
                            wheres.Add(" CardNo='" + dev.CardNo + "',");
                        }
                        if (dev.CarNumber != "")
                        {
                            wheres.Add(" CarNumber='" + dev.CarNumber + "',");
                        }
                        if (dev.PlannedVolume != -1)
                        {
                            wheres.Add(" PlannedVolume='" + dev.PlannedVolume + "',");
                        }
                        if (dev.AmountInstall != -1)
                        {
                            wheres.Add(" AmountInstall='" + dev.AmountInstall + "',");
                        }
                        if (dev.InsTraffic != -1)
                        {
                            wheres.Add(" InsTraffic='" + dev.InsTraffic + "',");
                        }
                        if (dev.Accumulate != -1)
                        {
                            wheres.Add(" Accumulate='" + dev.Accumulate + "',");
                        }
                        if (dev.Temperature != -1)
                        {
                            wheres.Add(" Temperature='" + dev.Temperature + "',");
                        }
                        if (dev.Denser != -1)
                        {
                            wheres.Add(" Denser='" + dev.Denser + "',");
                        }
                        if (dev.Pressure != -1)
                        {
                            wheres.Add(" Pressure='" + dev.Pressure + "',");
                        }
                        if (dev.Pace != -1)
                        {
                            wheres.Add(" Pace='" + dev.Pace + "',");
                        }
                        if (dev.Operate != -1)
                        {
                            wheres.Add(" Operate='" + dev.Operate + "',");
                        }
                        if (dev.EmeStop != -1)
                        {
                            wheres.Add(" EmeStop='" + dev.EmeStop + "',");
                        }
                        if (dev.AlarmA !=-1)
                        {
                            wheres.Add(" AlarmA='" + dev.AlarmA + "',");
                        }
                        if (dev.AlarmB !=-1)
                        {
                            wheres.Add(" AlarmB='" + dev.AlarmB + "',");
                        }
                        if (dev.AlarmC !=-1)
                        {
                            wheres.Add(" AlarmC='" + dev.AlarmC + "',");
                        }
                        if (dev.AlarmD !=-1)
                        {
                            wheres.Add(" AlarmD='" + dev.AlarmD + "',");
                        }
                        if (dev.AlarmE !=-1)
                        {
                            wheres.Add(" AlarmE='" + dev.AlarmE + "',");
                        }
                        if (dev.AlarmF !=-1)
                        {
                            wheres.Add(" AlarmF='" + dev.AlarmF + "',");
                        }
                        if (dev.AlarmG !=-1)
                        {
                            wheres.Add(" AlarmG='" + dev.AlarmG + "',");
                        }
                        if (dev.AlarmH !=-1)
                        {
                            wheres.Add(" AlarmH='" + dev.AlarmH + "',");
                        }
                        if (dev.AlarmI !=-1)
                        {
                            wheres.Add(" AlarmI='" + dev.AlarmI + "',");
                        }
                        if (dev.AlarmJ !=-1)
                        {
                            wheres.Add(" AlarmJ='" + dev.AlarmJ + "',");
                        }
                        if (dev.AlarmK !=-1)
                        {
                            wheres.Add(" AlarmK='" + dev.AlarmK + "',");
                        }
                        if (dev.AlarmL !=-1)
                        {
                            wheres.Add(" AlarmL='" + dev.AlarmL + "',");
                        }
                        if (dev.AlarmQ !=-1)
                        {
                            wheres.Add(" AlarmQ='" + dev.AlarmQ + "',");
                        }
                        if (dev.AlarmW !=-1)
                        {
                            wheres.Add(" AlarmW='" + dev.AlarmW + "',");
                        }
                        if (dev.AlarmR !=-1)
                        {
                            wheres.Add(" AlarmR='" + dev.AlarmR + "',");
                        }
                        if (dev.AlarmT !=-1)
                        {
                            wheres.Add(" AlarmT='" + dev.AlarmT + "',");
                        }
                        if (dev.AlarmO !=-1)
                        {
                            wheres.Add(" AlarmO='" + dev.AlarmO + "',");
                        }
                        if (dev.ThreadGroup !=-1)
                        {
                            wheres.Add(" ThreadGroup='" + dev.ThreadGroup + "'");
                        }
                        if (wheres.Count > 0)
                        {
                            wheres.Add(" where  DeviceNO='" + dev.DeviceNO + "'");
                            string wh = string.Join("", wheres.ToArray());
                            sql.Append(wh);
                            cmd.CommandText = sql.ToString();
                        }
                        count = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    LogerHelper.Error("Devicestatus:", ex);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return count;
        }

        public int DeviceExecuteOrder(SYSDeviceModel dev) 
        {
            int count = 0;
            using (SqlConnection conn = new SqlConnection())
            {
                try
                {
                    conn.ConnectionString = connStr;
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        StringBuilder sql = new StringBuilder("update SYSDeviceInfo  SET  ");
                        List<string> wheres = new List<string>();
                        if (dev.CraneStatus !="")
                        {
                            wheres.Add(" CraneStatus='" + dev.CraneStatus + "',");
                        }
                        if (dev.OrderNumber !="")
                        {
                            wheres.Add(" OrderNumber='" + dev.OrderNumber + "',");
                        }
                        if (dev.CarNumber !="")
                        {
                            wheres.Add(" CarNumber='" + dev.CarNumber + "',");
                        }
                        if (dev.PlannedVolume != -1)
                        {
                            wheres.Add(" PlannedVolume='" + dev.PlannedVolume + "',");
                        }
                        if (dev.AmountInstall != -1)
                        {
                            wheres.Add(" AmountInstall='" + dev.AmountInstall + "',");
                        }
                        if (dev.InsTraffic != -1)
                        {
                            wheres.Add(" InsTraffic='" + dev.InsTraffic + "',");
                        }
                        if (dev.Accumulate != -1)
                        {
                            wheres.Add(" Accumulate='" + dev.Accumulate + "',");
                        }
                        if (dev.Temperature != -1)
                        {
                            wheres.Add(" Temperature='" + dev.Temperature + "',");
                        }
                        if (dev.Denser != -1)
                        {
                            wheres.Add(" Denser='" + dev.Denser + "',");
                        }
                        if (dev.Pressure != -1)
                        {
                            wheres.Add(" Pressure='" + dev.Pressure + "',");
                        }
                        if (dev.Pace != -1)
                        {
                            wheres.Add(" Pace='" + dev.Pace + "',");
                        }
                        if (dev.Operate != -1)
                        {
                            wheres.Add(" Operate='" + dev.Operate + "',");
                        }
                        if (dev.EmeStop != -1)
                        {
                            wheres.Add(" EmeStop='" + dev.EmeStop + "',");
                        }
                        if (wheres.Count > 0)
                        {
                            wheres.Add(" where  DeviceNO='" + dev.DeviceNO + "'");
                            string wh = string.Join("", wheres.ToArray());
                            sql.Append(wh);
                        }
                        cmd.CommandText = sql.ToString();
                        count = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    LogerHelper.Error("Devicestatus:", ex);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return count;
        }

        public List<OrderInfoModels> OrderListQuery(OrderInfoModels order)
        {
            List<OrderInfoModels> orderList = new List<OrderInfoModels>();
            using (SqlConnection conn = new SqlConnection())
            {

                conn.ConnectionString = connStr;
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    StringBuilder sql = new StringBuilder("select [OrderCoding],[OrderNumber],[LicensePlate] from [OrderInfo]    ");
                    List<string> wheres = new List<string>();
                 
                    if (order.OrderNo != null && order.OrderNo != "")
                    {
                        wheres.Add(" OrderNumber like '%" + order.OrderNo + "%'");
                    }
                    if (wheres.Count > 0)
                    {
                        wheres.Add("IsDelte = 1");
                        string wh = string.Join(" and ", wheres.ToArray());
                        sql.Append(" where " + wh + "  Order  by  CrateDate  DESC");
                    }
                    cmd.CommandText = sql.ToString();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            OrderInfoModels or = new OrderInfoModels();
                            if (dr["OrderCoding"] != DBNull.Value)
                            {
                                or.OrderCode = Convert.ToInt32(dr["OrderCoding"]);
                            }
                            if (dr["OrderNumber"] != DBNull.Value)
                            {
                                or.OrderNo = Convert.ToString(dr["OrderNumber"]);
                            }
                           
                            if (dr["LicensePlate"] != DBNull.Value)
                            {
                                or.CarNo = Convert.ToString(dr["LicensePlate"]);
                            }
                            orderList.Add(or);
                        }
                        dr.Close();
                    }
                }

                conn.Close();
                conn.Dispose();

            }
            return orderList;
        }


        public List<AlarmInfo> AlarmsList(AlarmInfo Alar) 
        {
            List<AlarmInfo>  AlarList=new List<AlarmInfo>();
            using (SqlConnection conn = new SqlConnection()) 
            {
                conn.ConnectionString=connStr;
                conn.Open();
                using (SqlCommand cmd = new SqlCommand()) 
                {
                    StringBuilder sql = new StringBuilder("select [AlarmCode],[DeviceNO],[TagNo],[OrderNo],[AlarmName],[AlarmType],[AlarmDes],[StartTime],[EndTime],[Flag],[UploadID],[ProTime],[ProUser],[Note] from [AlarmInfo]    ");
                    List<string> wheres = new List<string>();

                    if (Alar.DeviceNO != 0 && Alar.DeviceNO != -1)
                    {
                        wheres.Add(" DeviceNO like '%" + Alar.DeviceNO + "%'");
                    }

                    if (Alar.TagNo != null && Alar.TagNo !="")
                    {
                        wheres.Add(" TagNo like '%" + Alar.TagNo + "%'");
                    }
                    if (Alar.AlarmName != null && Alar.AlarmName != "")
                    {
                        wheres.Add(" AlarmName like '%" + Alar.AlarmName + "%'");
                    }

                    if (Alar.OrderNo != null && Alar.OrderNo != "")
                    {
                        wheres.Add(" OrderNo like '%" + Alar.OrderNo + "%'");
                    }
                    if (Alar.StartTime.ToString()!= "0001/1/1 0:00:00")
                    {
                        wheres.Add(" StartTime like '%" + Alar.StartTime + "%'");
                    }
                    if (Alar.EndTime.ToString() != "0001/1/1 0:00:00")
                    {
                        wheres.Add(" EndTime like '%" + Alar.EndTime + "%'");
                    }
                    if (Alar.Flag != -1)
                    {
                        wheres.Add(" Flag like '%" + Alar.Flag + "%'");
                    }
                  
             
                    if (Alar.UploadID!=-1)
                    {
                        wheres.Add(" UploadID like '%" + Alar.UploadID + "%'");
                    }
                    if (wheres.Count > 0)
                    {
                        string wh = string.Join(" and ", wheres.ToArray());
                        sql.Append(" where " + wh + "     Order  by  EndTime  DESC");
                    }

                    cmd.CommandText= sql.ToString();
                    cmd.Connection = conn;
                    using (SqlDataReader dr = cmd.ExecuteReader()) 
                    {
                        while (dr.Read()) 
                        {
                            AlarmInfo alarm=new AlarmInfo();
                            if (dr["AlarmCode"]!=DBNull.Value) 
                            {
                                alarm.AlarmCode = Convert.ToInt32(dr["AlarmCode"]);
                            }
                            if (dr["DeviceNO"]!=DBNull.Value) 
                            {
                                alarm.DeviceNO = Convert.ToInt32(dr["DeviceNO"]);
                            }
                            if (dr["TagNo"] != DBNull.Value)
                            {
                                alarm.TagNo = Convert.ToString(dr["TagNo"]);
                            }
                            if (dr["OrderNo"] != DBNull.Value)
                            {
                                alarm.OrderNo = Convert.ToString(dr["OrderNo"]);
                            }
                            if (dr["AlarmName"] != DBNull.Value)
                            {
                                alarm.AlarmName = Convert.ToString(dr["AlarmName"]);
                            }
                            if (dr["AlarmType"] != DBNull.Value)
                            {
                                alarm.AlarmType = Convert.ToInt32(dr["AlarmType"]);
                            }

                            if (dr["AlarmDes"] != DBNull.Value)
                            {
                                alarm.AlarmDes = Convert.ToString(dr["AlarmDes"]);
                            }

                            if (dr["StartTime"] != DBNull.Value)
                            {
                                alarm.StartTime = Convert.ToDateTime(dr["StartTime"]);
                            }
                            if (dr["EndTime"] != DBNull.Value)
                            {
                                alarm.EndTime = Convert.ToDateTime(dr["EndTime"]);
                            }
                            if (dr["Flag"] != DBNull.Value)
                            {
                                alarm.Flag = Convert.ToInt32(dr["Flag"]);
                            }
                            if (dr["UploadID"] != DBNull.Value)
                            {
                                alarm.UploadID = Convert.ToInt32(dr["UploadID"]);
                            }
                            if (dr["ProTime"] != DBNull.Value)
                            {
                                alarm.ProTime = Convert.ToDateTime(dr["ProTime"]);
                            }
                            if (dr["ProUser"] != DBNull.Value)
                            {
                                alarm.ProUser = Convert.ToInt32(dr["ProUser"]);
                            }
                            if (dr["Note"] != DBNull.Value)
                            {
                                alarm.Note = Convert.ToString(dr["Note"]);
                            }
                            AlarList.Add(alarm);
                        }
                        dr.Close();
                    }

                }
                conn.Close();
            }

            return AlarList;
        }

        public int AlarmAdd(AlarmInfo Alar) 
        {
            int msg = 0;
            using (SqlConnection conn = new SqlConnection()) 
            {
                conn.ConnectionString = connStr;
                conn.Open();
                using (SqlCommand cmd = new SqlCommand()) 
                {
                    cmd.Connection = conn;
                    StringBuilder sql = new StringBuilder("INSERT INTO [AlarmInfo] ([DeviceNO] ,[TagNo] ,[OrderNo] ,[AlarmName] ,[AlarmType] ,[AlarmDes] ,[StartTime],[Flag] ,[UploadID],[ProUser] ,AlarmValue,[Note] ) VALUES(");
                    List<string> wheres = new List<string>();
                    if (Alar.DeviceNO != -1)
                    {
                        wheres.Add("'" + Alar.DeviceNO + "',");
                    }
                   
                    if (Alar.TagNo !="")
                    {
                        wheres.Add("'" + Alar.TagNo + "',");
                    }
                    if (Alar.OrderNo != "")
                    {
                        wheres.Add("'" + Alar.OrderNo + "',");
                    }
                    else 
                    {
                        wheres.Add("'" + "',");
                    }
                    if (Alar.AlarmName != "")
                    {
                        wheres.Add("'" + Alar.AlarmName + "',");
                    }
                    else 
                    {
                        wheres.Add("'"+ "',");
                    }
                    if (Alar.AlarmType != -1)
                    {
                        wheres.Add("'" + Alar.AlarmType + "',");
                    }
                    else
                    {
                        wheres.Add("'" + "',");
                    }
                    if (Alar.AlarmDes !="")
                    {
                        wheres.Add("'" + Alar.AlarmDes + "',");
                    }
                    else
                    {
                        wheres.Add("'" + "',");
                    }
                    if (Alar.StartTime.ToString() == "0001/1/1 0:00:00")
                    {
                        wheres.Add("" + "Getdate()" + ",");
                    }
                    
                    if (Alar.Flag != -1)
                    {
                        wheres.Add("'" + Alar.Flag + "',");
                    }
                    else 
                    {
                        wheres.Add("'"  + "',");
                    }
                    if (Alar.UploadID != -1)
                    {
                        wheres.Add("'" + Alar.UploadID + "',");
                    }
                    else 
                    {
                        wheres.Add("'" + "',");
                    }
                    if (Alar.ProUser != -1)
                    {
                        wheres.Add("'" + Alar.ProUser + "',");
                    }
                    else 
                    {
                        wheres.Add("'" + "',");
                    }
                    if (Alar.AlarmValue !="")
                    {
                        wheres.Add("'" + Alar.AlarmValue + "',");
                    }
                    else
                    {
                        wheres.Add("'" + "',");
                    }
                    if (Alar.Note !="")
                    {
                        wheres.Add("'" + Alar.Note + "'");
                    }
                    if (wheres.Count > 0)
                    {
                        wheres.Add(")");
                        string wh = string.Join("", wheres.ToArray());
                        sql.Append(wh);
                    }
                    cmd.CommandText = sql.ToString();
                    msg = cmd.ExecuteNonQuery();
                }
                conn.Close();
                conn.Dispose();
            }
            return msg;
        }

        public int AlarmEdit(AlarmInfo Alar)
        {
            int count = 0;
            using (SqlConnection conn = new SqlConnection()) 
            {
                conn.ConnectionString = connStr;
                conn.Open();
                using (SqlCommand cmd = new SqlCommand()) 
                {
                    cmd.Connection = conn;
                    StringBuilder sql = new StringBuilder("update [AlarmInfo] set    ");
                    List<string> wheres = new List<string>();
                    if (Alar.EndTime.ToString() == "0001/1/1 0:00:00")
                    {
                        wheres.Add(" EndTime=" +"Getdate()" + ",");
                    }
                    if (Alar.Flag !=-1)
                    {
                        wheres.Add(" Flag='" + Alar.Flag + "',");
                    }
                    if (Alar.UploadID != -1)
                    {
                        wheres.Add(" UploadID='" + Alar.UploadID + "',");
                    }
                    if (Alar.ProTime.ToString() == "0001/1/1 0:00:00")
                    {
                        wheres.Add(" ProTime=" +"GetDate()"+ ",");
                    }
                    if (Alar.ProUser != -1)
                    {
                        wheres.Add(" ProUser='" + Alar.ProUser + "',");
                    }
                    if (Alar.Note != "")
                    {
                        wheres.Add(" Note='" + Alar.Note + "'");
                    }
                    if (wheres.Count > 0)
                    {
                        wheres.Add(" where  DeviceNO='" + Alar.DeviceNO + "' and   TagNo='"+ Alar.TagNo + "'");
                        string wh = string.Join("", wheres.ToArray());
                        sql.Append(wh);
                    }
                    cmd.CommandText = sql.ToString();
                    count = cmd.ExecuteNonQuery();
                }
                 conn.Close();
                conn.Dispose();
            }
            return count;
        }

    }
}
