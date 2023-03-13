using PLCSystem.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSystem.Models
{
	[Serializable]
   public class SYSDeviceModel: NotifyBase
	{
		public SYSDeviceModel()
		{ }
		#region Model
		private int _deviceno;
		private int _machineno;
        private string _maName;
        private string _craneno;
		private string _crName;
		private string _ip;
		private string _constituen;
		private string _ordernumber;
		private string _orname;
		private string _carnumber;
		private string _cnname;
		private decimal _plannedvolume;
		private string _pvname;
		private decimal _amountinstall;
		private string _ainame;
		private decimal _instraffic;
		private string _itname;
		private decimal _accumulate;
		private string _aname;
		private decimal _temperature;
		private string _tname;
		private decimal _denser;
		private string _dname;
		private decimal _pressure;
		private string _pname;
		private string _liname;
        private decimal _leveltran;
        private int _alarma;
		private string _alarmaname;
		private int _alarmb;
		private string _alarmbname;
		private int _alarmc;
		private string _alarmcname;
		private int _alarmd;
		private string _alarmdname;
		private int _alarme;
		private string _alarmename;
		private int _alarmf;
		private string _alarmfname;
		private int _alarmg;
		private string _alarmgname;
		private int _alarmh;
		private string _alarmhname;
		private int _alarmi;
		private string _alarminame;
		private int _alarmj;
		private string _alarmjname;
		private int _alarmk;
		private string _alarmkname;
		private int _alarml;
		private string _alarmlname;
		private int _alarmq;
		private string _alarmqname;
		private int _alarmw;
		private string _alarmwname;
		private int _alarmr;
		private string _alarmrname;
		private int _alarmt;
		private string _alarmtname;
		private int _alarmo;
		private string _alarmoname;
		private string _cranestatus;
		private string _statusname;
		private int _itemcode;
		private string _itemname;
		private string _devicestatus;
		private double _pace;
		private string _paceName;
		private int _operate;
		private string _operateName;
		private int _emestop;
        private DateTime _createdate = DateTime.Now;
		private int _createuser;
		private DateTime _editdate;
		private int _edituser;
        private int _threadGroup;
		private string manameal;
		private string _cardNo;

        public string CardNo
		{
            get { return _cardNo; }
            set { _cardNo = value; }
        }



        /// <summary>
        /// 线程组别
        /// </summary>
        public int ThreadGroup
		{
            get { return _threadGroup; }
            set { _threadGroup = value; }
        }


        /// <summary>
        /// 设备序号
        /// </summary>
        public int DeviceNO
		{
			set { _deviceno = value; }
			get { return _deviceno; }
		}
		/// <summary>
		/// 机号
		/// </summary>
		public int MachineNo
		{
			set { _machineno = value; }
			get { return _machineno; }
		}
		/// <summary>
		/// 机号名称
		/// </summary>
		public string MaName
		{
			get { return _maName; }
			set { _maName = value; }
		}
		/// <summary>
		/// 鹤号
		/// </summary>
		public string CraneNO
		{
			set { _craneno = value; }
			get { return _craneno; }
		}
		/// <summary>
		/// tabitem
		/// </summary>
		public string CrName 
		{
			set { _crName = value; }
			get { return _crName; }
		}


		public string MaNameal
		{
			set { manameal = value; }
			get { return manameal; }
		}

		/// <summary>
		/// IP地址
		/// </summary>
		public string IP
		{
			set { _ip = value; }
			get { return _ip; }
		}
		/// <summary>
		/// 组别
		/// </summary>
		public string constituen
		{
			set { _constituen = value; }
			get { return _constituen; }
		}
		/// <summary>
		/// 订单号
		/// </summary>
		public string OrderNumber
		{
			set { _ordernumber = value; }
			get { return _ordernumber; }
		}
		/// <summary>
		/// 订单号名称
		/// </summary>
		public string OrName
		{
			set { _orname = value; }
			get { return _orname; }
		}
		/// <summary>
		/// 车牌号
		/// </summary>
		public string CarNumber
		{
			set { _carnumber = value; }
			get { return _carnumber; }
		}
		/// <summary>
		/// 车牌号名称
		/// </summary>
		public string CNName
		{
			set { _cnname = value; }
			get { return _cnname; }
		}
		/// <summary>
		/// 预装量
		/// </summary>
		public decimal PlannedVolume
		{
			set { _plannedvolume = value; }
			get { return _plannedvolume; }
		}
		/// <summary>
		/// 预装量名称
		/// </summary>
		public string PVName
		{
			set { _pvname = value; }
			get { return _pvname; }
		}

        public string LiName
        {
            set { _liname = value; }
            get { return _liname; }
        }

        /// <summary>
        /// 实装量
        /// </summary>
        public decimal AmountInstall
		{
			set { _amountinstall = value; }
			get { return _amountinstall; }
		}
		/// <summary>
		/// 实装量名称
		/// </summary>
		public string AIName
		{
			set { _ainame = value; }
			get { return _ainame; }
		}
		/// <summary>
		/// 瞬时流量
		/// </summary>
		public decimal InsTraffic
		{
			set { _instraffic = value; }
			get { return _instraffic; }
		}
		/// <summary>
		/// 瞬时流量名称
		/// </summary>
		public string ITName
		{
			set { _itname = value; }
			get { return _itname; }
		}
		/// <summary>
		/// 累计流量
		/// </summary>
		public decimal Accumulate
		{
			set { _accumulate = value; }
			get { return _accumulate; }
		}
		/// <summary>
		/// 累计流量名称
		/// </summary>
		public string AName
		{
			set { _aname = value; }
			get { return _aname; }
		}
		/// <summary>
		/// 温度
		/// </summary>
		public decimal Temperature
		{
			set { _temperature = value; }
			get { return _temperature; }
		}
		/// <summary>
		/// 温度名称
		/// </summary>
		public string TName
		{
			set { _tname = value; }
			get { return _tname; }
		}
		/// <summary>
		/// 密度
		/// </summary>
		public decimal Denser
		{
			set { _denser = value; }
			get { return _denser; }
		}
		/// <summary>
		/// 密度名称
		/// </summary>
		public string DName
		{
			set { _dname = value; }
			get { return _dname; }
		}
		/// <summary>
		/// 压力
		/// </summary>
		public decimal Pressure
		{
			set { _pressure = value; }
			get { return _pressure; }
		}
		/// <summary>
		/// 压力名称
		/// </summary>
		public string PName
		{
			set { _pname = value; }
			get { return _pname; }
		}
		/// <summary>
		/// 报警A
		/// </summary>
		public int AlarmA
		{
			set { _alarma = value; }
			get { return _alarma; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmAName
		{
			set { _alarmaname = value; }
			get { return _alarmaname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmB
		{
			set { _alarmb = value; }
			get { return _alarmb; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmBName
		{
			set { _alarmbname = value; }
			get { return _alarmbname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmC
		{
			set { _alarmc = value; }
			get { return _alarmc; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmCName
		{
			set { _alarmcname = value; }
			get { return _alarmcname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmD
		{
			set { _alarmd = value; }
			get { return _alarmd; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmDName
		{
			set { _alarmdname = value; }
			get { return _alarmdname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmE
		{
			set { _alarme = value; }
			get { return _alarme; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmEName
		{
			set { _alarmename = value; }
			get { return _alarmename; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmF
		{
			set { _alarmf = value; }
			get { return _alarmf; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmFName
		{
			set { _alarmfname = value; }
			get { return _alarmfname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmG
		{
			set { _alarmg = value; }
			get { return _alarmg; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmGName
		{
			set { _alarmgname = value; }
			get { return _alarmgname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmH
		{
			set { _alarmh = value; }
			get { return _alarmh; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmHName
		{
			set { _alarmhname = value; }
			get { return _alarmhname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmI
		{
			set { _alarmi = value; }
			get { return _alarmi; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmIName
		{
			set { _alarminame = value; }
			get { return _alarminame; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmJ
		{
			set { _alarmj = value; }
			get { return _alarmj; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmJName
		{
			set { _alarmjname = value; }
			get { return _alarmjname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmK
		{
			set { _alarmk = value; }
			get { return _alarmk; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmKName
		{
			set { _alarmkname = value; }
			get { return _alarmkname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmL
		{
			set { _alarml = value; }
			get { return _alarml; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmLName
		{
			set { _alarmlname = value; }
			get { return _alarmlname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmQ
		{
			set { _alarmq = value; }
			get { return _alarmq; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmQName
		{
			set { _alarmqname = value; }
			get { return _alarmqname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmW
		{
			set { _alarmw = value; }
			get { return _alarmw; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmWName
		{
			set { _alarmwname = value; }
			get { return _alarmwname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmR
		{
			set { _alarmr = value; }
			get { return _alarmr; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmRName
		{
			set { _alarmrname = value; }
			get { return _alarmrname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmT
		{
			set { _alarmt = value; }
			get { return _alarmt; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmTName
		{
			set { _alarmtname = value; }
			get { return _alarmtname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int AlarmO
		{
			set { _alarmo = value; }
			get { return _alarmo; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AlarmOName
		{
			set { _alarmoname = value; }
			get { return _alarmoname; }
		}
		/// <summary>
		/// 设备当前状态（0.无效  1.待机中  2.准备中  3.运行中  4.暂停中  5.停止中）
		/// </summary>
		public string CraneStatus
		{
			set { _cranestatus = value; }
			get { return _cranestatus; }
		}
		/// <summary>
		/// 状态名称
		/// </summary>
		public string StatusName
		{
			set { _statusname = value; }
			get { return _statusname; }
		}
		/// <summary>
		/// 物料编码
		/// </summary>
		public int ItemCode
		{
			set { _itemcode = value; }
			get { return _itemcode; }
		}
		/// <summary>
		/// 物料名称
		/// </summary>
		public string ItemName
		{
			set { _itemname = value; }
			get { return _itemname; }
		}
		/// <summary>
		/// 设备状态（0.未启用 1.装车  2.卸车 3.装卸通用）
		/// </summary>
		public string DeviceStatus
		{
			set { _devicestatus = value; }
			get { return _devicestatus; }
		}
		/// <summary>
		/// 进度
		/// </summary>
		public double Pace
		{
			set { _pace = value; }
			get { return _pace; }
		}
		public string PaceName
		{
			set { _paceName = value; }
			get { return _paceName; }
		}
		public string OperateName
		{
			set { _operateName = value; }
			get { return _operateName; }
		}
	
		/// <summary>
		/// 操作
		/// </summary>
		public int Operate
		{
			set { _operate = value; }
			get { return _operate; }
		}

		
		/// <summary>
		/// 急停
		/// </summary>
		public int EmeStop
		{
			set { _emestop = value; }
			get { return _emestop; }
		}

		private string _emeStopName;

		public string EmeStopName
		{
			get { return _emeStopName; }
			set { _emeStopName = value; }
		}

		/// <summary>
		/// 创建日期
		/// </summary>
		public DateTime CreateDate
		{
			set { _createdate = value; }
			get { return _createdate; }
		}
		/// <summary>
		/// 创建用户
		/// </summary>
		public int CreateUser
		{
			set { _createuser = value; }
			get { return _createuser; }
		}
		/// <summary>
		/// 修改日期
		/// </summary>
		public DateTime EditDate
		{
			set { _editdate = value; }
			get { return _editdate; }
		}
		/// <summary>
		/// 修改用户
		/// </summary>
		public int EditUser
		{
			set { _edituser = value; }
			get { return _edituser; }
		}
        
    


        public decimal Leveltran
        {
            set { _leveltran = value; }
            get { return _leveltran; }
        }

        #endregion Model
    }
}
