using PLCSystem.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSystem.Models
{
    [Serializable]
    public class OrderInfoModel: NotifyBase
    {
        public OrderInfoModel() { }
        #region Model
        private int _ordercoding;
        private string _ordernumber;
        private int _bustype;
        private string _cardnumber;
        private int _cuscode;
        private string _cusname;
        private string _licenseplate;
        private string _driver;
        private string _phone;
        private string _crane;
        private string _craneposition;
        private decimal _plannedvolume;
        private int _itemcode;
        private string _itemname;
        private int _crateuser;
        private DateTime _cratedate = DateTime.Now;
        private int _edituser;
        private DateTime _editdate;
        private int _cardmaker;
        private DateTime _carddate;
        private int _cardclearers;
        private DateTime _clecarddate;
        private int _isdelte;
        private decimal _weighonce;
        private string _oneweightunit;
        private DateTime _oneweighingtime;
        private int _onceoverweightman;
        private decimal _weightwice;
        private string _secweighingunits;
        private DateTime _secweighingtime;
        private int _secoverweightman;
        private decimal _netweight;
        private decimal _actualquantity;
        private DateTime _writetime;
        private int _printsnum;
        private int _printuser;
        private DateTime _printtime;
        private int _status;
        private int _exuser;
        private string _exusername;
        private DateTime _exdate;
        private string _note;
        /// <summary>
        /// 
        /// </summary>
        public int OrderCoding
        {
            set { _ordercoding = value; }
            get { return _ordercoding; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderNumber
        {
            set { _ordernumber = value; this.NotifyChanged(); }
            get { return _ordernumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BusType
        {
            set { _bustype = value; this.NotifyChanged(); }
            get { return _bustype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CardNumber
        {
            set { _cardnumber = value; this.NotifyChanged(); }
            get { return _cardnumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CusCode
        {
            set { _cuscode = value; this.NotifyChanged(); }
            get { return _cuscode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CusName
        {
            set { _cusname = value; this.NotifyChanged(); }
            get { return _cusname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LicensePlate
        {
            set { _licenseplate = value; this.NotifyChanged(); }
            get { return _licenseplate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Driver
        {
            set { _driver = value; this.NotifyChanged(); }
            get { return _driver; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; this.NotifyChanged(); }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Crane
        {
            set { _crane = value; this.NotifyChanged(); }
            get { return _crane; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CranePosition
        {
            set { _craneposition = value; this.NotifyChanged(); }
            get { return _craneposition; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal PlannedVolume
        {
            set { _plannedvolume = value; this.NotifyChanged(); }
            get { return _plannedvolume; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ItemCode
        {
            set { _itemcode = value; this.NotifyChanged(); }
            get { return _itemcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ItemName
        {
            set { _itemname = value; this.NotifyChanged(); }
            get { return _itemname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CrateUser
        {
            set { _crateuser = value; this.NotifyChanged(); }
            get { return _crateuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CrateDate
        {
            set { _cratedate = value; this.NotifyChanged(); }
            get { return _cratedate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int EditUser
        {
            set { _edituser = value; this.NotifyChanged(); }
            get { return _edituser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EditDate
        {
            set { _editdate = value; this.NotifyChanged(); }
            get { return _editdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CardMaker
        {
            set { _cardmaker = value; this.NotifyChanged(); }
            get { return _cardmaker; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CardDate
        {
            set { _carddate = value; this.NotifyChanged(); }
            get { return _carddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CardClearers
        {
            set { _cardclearers = value; this.NotifyChanged(); }
            get { return _cardclearers; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CleCardDate
        {
            set { _clecarddate = value; this.NotifyChanged(); }
            get { return _clecarddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsDelte
        {
            set { _isdelte = value; this.NotifyChanged(); }
            get { return _isdelte; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal WeighOnce
        {
            set { _weighonce = value; this.NotifyChanged(); }
            get { return _weighonce; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OneweightUnit
        {
            set { _oneweightunit = value; this.NotifyChanged(); }
            get { return _oneweightunit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime OneWeighingTime
        {
            set { _oneweighingtime = value; this.NotifyChanged(); }
            get { return _oneweighingtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int OnceOverweightMan
        {
            set { _onceoverweightman = value; this.NotifyChanged(); }
            get { return _onceoverweightman; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Weightwice
        {
            set { _weightwice = value; this.NotifyChanged(); }
            get { return _weightwice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SecWeighingUnits
        {
            set { _secweighingunits = value; this.NotifyChanged(); }
            get { return _secweighingunits; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime SecWeighingTime
        {
            set { _secweighingtime = value; this.NotifyChanged(); }
            get { return _secweighingtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SecoverweightMan
        {
            set { _secoverweightman = value; this.NotifyChanged(); }
            get { return _secoverweightman; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal NetWeight
        {
            set { _netweight = value; this.NotifyChanged(); }
            get { return _netweight; }
        }
        /// <summary>
        /// 回写实装量
        /// </summary>
        public decimal ActualQuantity
        {
            set { _actualquantity = value; this.NotifyChanged(); }
            get { return _actualquantity; }
        }
        /// <summary>
        /// 回写时间
        /// </summary>
        public DateTime WriteTime
        {
            set { _writetime = value; this.NotifyChanged(); }
            get { return _writetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PrintsNum
        {
            set { _printsnum = value; this.NotifyChanged(); }
            get { return _printsnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PrintUser
        {
            set { _printuser = value; this.NotifyChanged(); }
            get { return _printuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime PrintTime
        {
            set { _printtime = value; this.NotifyChanged(); }
            get { return _printtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; this.NotifyChanged(); }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Exuser
        {
            set { _exuser = value; this.NotifyChanged(); }
            get { return _exuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExuserName
        {
            set { _exusername = value; this.NotifyChanged(); }
            get { return _exusername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Exdate
        {
            set { _exdate = value; this.NotifyChanged(); }
            get { return _exdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Note
        {
            set { _note = value; this.NotifyChanged(); }
            get { return _note; }
        }
        #endregion Model


    }
}
