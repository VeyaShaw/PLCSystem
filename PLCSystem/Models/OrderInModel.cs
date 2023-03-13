using PLCSystem.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSystem.Models
{
	/// <summary>
	/// 订单信息
	/// </summary>
	[Serializable]
	public partial class OrderInfoModels : NotifyBase
	{
		public OrderInfoModels()
		{ }
		#region Model
		private int _ordercode;
		private int _devicecode;
		private string _orderno;
		private string _cardno;
		private int _shokaku;
		private string _craneno;
		private string _carno;
		private string _idcard;
		private string _driver;
		private string _telephone;
		private int _bustype;
		private int _datatype;
		private int _cuscode;
		private string _cusname;
		private int _suppcode;
		private string _suppname;
		private int _itemcode;
		private string _itemname;
		private decimal _invname;
		private decimal _actualquantity;
		private decimal _flowrate;
		private decimal _totalflow;
		private decimal _temperature;
		private decimal _density;
		private decimal _pressure;
		private int _speedpro;
		private int _oper;
		private int _crashstop;
		private decimal _onepweight;
		private DateTime _oneptime;
		private string _oneweigher;
		private decimal _secweight;
		private DateTime _secrtime;
		private string _secpman;
		private decimal _netweight;
		private int _alarma;
		private int _alarmb;
		private int _alarmc;
		private int _alarmd;
		private int _alarme;
		private int _alarmf;
		private int _alarmg;
		private int _alarmh;
		private int _alarmi;
		private int _alarmj;
		private int _alarmk;
		private int _alarml;
		private int _alarmq;
		private int _alarmw;
		private int _alarmr;
		private int _alarmt;
		private int _alarmo;
		private int _acccstatus;
		private int _vehinsstatus;
		private int _creausername;
		private DateTime _createtime = DateTime.Now;
		private int _editusername;
		private DateTime _edittime;
		/// <summary>
		/// 订单序号
		/// </summary>
		public int OrderCode
		{
			set { _ordercode = value; }
			get { return _ordercode; }
		}
		/// <summary>
		/// 设备序号
		/// </summary>
		public int DeviceCode
		{
			set { _devicecode = value; }
			get { return _devicecode; }
		}
		/// <summary>
		/// 订单编号
		/// </summary>
		public string OrderNo
		{
			set { _orderno = value; }
			get { return _orderno; }
		}
		/// <summary>
		/// 卡号
		/// </summary>
		public string CardNo
		{
			set { _cardno = value; }
			get { return _cardno; }
		}
		/// <summary>
		/// 鹤位
		/// </summary>
		public string CraneNo
		{
			set { _craneno = value; }
			get { return _craneno; }
		}
		/// <summary>
		/// 鹤号
		/// </summary>
		public int Shokaku
		{
			set { _shokaku = value; }
			get { return _shokaku; }
		}
		/// <summary>
		/// 车牌号
		/// </summary>
		public string CarNo
		{
			set { _carno = value; }
			get { return _carno; }
		}
		/// <summary>
		/// 身份证号
		/// </summary>
		public string IDCard
		{
			set { _idcard = value; }
			get { return _idcard; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string Driver
		{
			set { _driver = value; }
			get { return _driver; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string Telephone
		{
			set { _telephone = value; }
			get { return _telephone; }
		}
		/// <summary>
		/// 业务类型（1.装车 2.卸车）
		/// </summary>
		public int BusType
		{
			set { _bustype = value; }
			get { return _bustype; }
		}
		/// <summary>
		/// 数据类型（1.外部接口 2.内部数据）
		/// </summary>
		public int Datatype
		{
			set { _datatype = value; }
			get { return _datatype; }
		}
		/// <summary>
		/// 客户编码
		/// </summary>
		public int CusCode
		{
			set { _cuscode = value; }
			get { return _cuscode; }
		}
		/// <summary>
		/// 客户名称
		/// </summary>
		public string CusName
		{
			set { _cusname = value; }
			get { return _cusname; }
		}
		/// <summary>
		/// 供应商内码
		/// </summary>
		public int SuppCode
		{
			set { _suppcode = value; }
			get { return _suppcode; }
		}
		/// <summary>
		/// 供应商名称
		/// </summary>
		public string SuppName
		{
			set { _suppname = value; }
			get { return _suppname; }
		}
		/// <summary>
		/// 物料内码
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
		/// 计划量
		/// </summary>
		public decimal InvName
		{
			set { _invname = value; }
			get { return _invname; }
		}
		/// <summary>
		/// 实装量
		/// </summary>
		public decimal ActualQuantity
		{
			set { _actualquantity = value; }
			get { return _actualquantity; }
		}
		/// <summary>
		/// 瞬时流量
		/// </summary>
		public decimal flowRate
		{
			set { _flowrate = value; }
			get { return _flowrate; }
		}
		/// <summary>
		/// 累计流量
		/// </summary>
		public decimal TotalFlow
		{
			set { _totalflow = value; }
			get { return _totalflow; }
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
		/// 密度
		/// </summary>
		public decimal Density
		{
			set { _density = value; }
			get { return _density; }
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
		/// 进度
		/// </summary>
		public int SpeedPro
		{
			set { _speedpro = value; }
			get { return _speedpro; }
		}
		/// <summary>
		/// 操作
		/// </summary>
		public int Oper
		{
			set { _oper = value; }
			get { return _oper; }
		}
		/// <summary>
		/// 急停
		/// </summary>
		public int CrashStop
		{
			set { _crashstop = value; }
			get { return _crashstop; }
		}
		/// <summary>
		/// 一次磅重
		/// </summary>
		public decimal OnePWeight
		{
			set { _onepweight = value; }
			get { return _onepweight; }
		}
		/// <summary>
		/// 一次过磅时间
		/// </summary>
		public DateTime OnePTime
		{
			set { _oneptime = value; }
			get { return _oneptime; }
		}
		/// <summary>
		/// 一次过磅人
		/// </summary>
		public string OneWeigher
		{
			set { _oneweigher = value; }
			get { return _oneweigher; }
		}
		/// <summary>
		/// 二次磅重
		/// </summary>
		public decimal SecWeight
		{
			set { _secweight = value; }
			get { return _secweight; }
		}
		/// <summary>
		/// 二次榜时间
		/// </summary>
		public DateTime SecRTime
		{
			set { _secrtime = value; }
			get { return _secrtime; }
		}
		/// <summary>
		/// 二次榜人
		/// </summary>
		public string SecPman
		{
			set { _secpman = value; }
			get { return _secpman; }
		}
		/// <summary>
		/// 净重
		/// </summary>
		public decimal NetWeight
		{
			set { _netweight = value; }
			get { return _netweight; }
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
		public int AlarmB
		{
			set { _alarmb = value; }
			get { return _alarmb; }
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
		public int AlarmD
		{
			set { _alarmd = value; }
			get { return _alarmd; }
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
		public int AlarmF
		{
			set { _alarmf = value; }
			get { return _alarmf; }
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
		public int AlarmH
		{
			set { _alarmh = value; }
			get { return _alarmh; }
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
		public int AlarmJ
		{
			set { _alarmj = value; }
			get { return _alarmj; }
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
		public int AlarmL
		{
			set { _alarml = value; }
			get { return _alarml; }
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
		public int AlarmW
		{
			set { _alarmw = value; }
			get { return _alarmw; }
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
		public int AlarmT
		{
			set { _alarmt = value; }
			get { return _alarmt; }
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
		/// 门禁状态（0未入厂，1入厂，2出厂）
		/// </summary>
		public int AccCstatus
		{
			set { _acccstatus = value; }
			get { return _acccstatus; }
		}
		/// <summary>
		/// 车检状态（0未检，1通过，2不通过）
		/// </summary>
		public int VehInsStatus
		{
			set { _vehinsstatus = value; }
			get { return _vehinsstatus; }
		}
		/// <summary>
		/// 创建人
		/// </summary>
		public int CreaUserName
		{
			set { _creausername = value; }
			get { return _creausername; }
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateTime
		{
			set { _createtime = value; }
			get { return _createtime; }
		}
		/// <summary>
		/// 编辑人
		/// </summary>
		public int EditUserName
		{
			set { _editusername = value; }
			get { return _editusername; }
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime EditTime
		{
			set { _edittime = value; }
			get { return _edittime; }
		}
		#endregion Model

	}
}
