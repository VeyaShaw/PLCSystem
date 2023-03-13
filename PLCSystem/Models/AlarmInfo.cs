using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSystem.Models
{
    /// <summary>
    /// 报警日志
    /// </summary>
    [Serializable]
    public partial class AlarmInfo
    {
        public AlarmInfo()
        { }
        #region Model
        private int _alarmcode;
        private int _deviceno = -1;
        private string _tagno;
        private string _orderno;
        private string _alarmname;
        private int _alarmtype;
        private string _alarmdes;
        private DateTime _starttime;
        private DateTime _endtime;
        private int _flag=-1;
        private int _uploadid = -1;
        private DateTime _protime;
        private int _prouser=-1;
        private string _alarmValue;
        private string _note;

        /// <summary>
        /// 报警内码
        /// </summary>
        public int AlarmCode
        {
            set { _alarmcode = value; }
            get { return _alarmcode; }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public int DeviceNO
        {
            set { _deviceno = value; }
            get { return _deviceno; }
        }


        /// <summary>
        /// 位号
        /// </summary>
        public string TagNo
        {
            set { _tagno = value; }
            get { return _tagno; }
        }


        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo
        {
            set { _orderno = value; }
            get { return _orderno; }
        }


        /// <summary>
        /// 报警名称
        /// </summary>
        public string AlarmName
        {
            set { _alarmname = value; }
            get { return _alarmname; }
        }


        /// <summary>
        /// 报警类型
        /// </summary>
        public int AlarmType
        {
            set { _alarmtype = value; }
            get { return _alarmtype; }
        }

        /// <summary>
        /// 报警描述
        /// </summary>
        public string AlarmDes
        {
            set { _alarmdes = value; }
            get { return _alarmdes; }
        }

        /// <summary>
        /// 开始报警时间
        /// </summary>
        public DateTime StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }

        /// <summary>
        /// 结束报警时间
        /// </summary>
        public DateTime EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }

        /// <summary>
        /// 报警状态（0.未处理1.已处理）
        /// </summary>
        public int Flag
        {
            set { _flag = value; }
            get { return _flag; }
        }

        /// <summary>
        /// 上传标识（0.未上传1.已上传）
        /// </summary>
        public int UploadID
        {
            set { _uploadid = value; }
            get { return _uploadid; }
        }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime ProTime
        {
            set { _protime = value; }
            get { return _protime; }
        }

        /// <summary>
        /// 处理用户
        /// </summary>
        public int ProUser
        {
            set { _prouser = value; }
            get { return _prouser; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note
        {
            set { _note = value; }
            get { return _note; }
        }

        public string AlarmValue
        {
            set { _alarmValue = value; }
            get { return _alarmValue; }
        }



        #endregion Model

    }
}
