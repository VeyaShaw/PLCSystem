using PLCSystem.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSystem.Models
{
    /// <summary>
    /// 系统报警及机位数配置
    /// </summary>
    [Serializable]
    public class SYSConfigModel: NotifyBase
	{
		public SYSConfigModel()
		{ }
		#region Model
		private int _configcode;
		private string _configname;
		private string _configtype;
		private int _rowscount;
		private int _collcount;
		private int _priority;
		private int _status = 1;
		private string _note;
		/// <summary>
		/// 
		/// </summary>
		public int ConfigCode
		{
			set { _configcode = value; }
			get { return _configcode; }
		}
		/// <summary>
		/// 配置名称
		/// </summary>
		public string ConfigName
		{
			set { _configname = value; this.NotifyChanged(); }
			get { return _configname; }
		}
		/// <summary>
		/// 配置类型
		/// </summary>
		public string ConfigType
		{
			set { _configtype = value; this.NotifyChanged(); }
			get { return _configtype; }
		}

		public int Rowscount 
		{
			set { _rowscount = value; this.NotifyChanged(); }
			get { return _rowscount; }
		}
		public int Collcount 
		{
			set { _collcount = value; this.NotifyChanged(); }
			get { return _collcount; }
		}
		/// <summary>
		/// 优先级
		/// </summary>
		public int Priority
		{
			set { _priority = value; this.NotifyChanged(); }
			get { return _priority; }
		}
		/// <summary>
		/// 状态（1.启用 2.禁用）
		/// </summary>
		public int Status
		{
			set { _status = value; this.NotifyChanged(); }
			get { return _status; }
		}
		
		/// <summary>
		/// 备注
		/// </summary>
		public string Note
		{
			set { _note = value; }
			get { return _note; }
		}
		#endregion 

	}
}
