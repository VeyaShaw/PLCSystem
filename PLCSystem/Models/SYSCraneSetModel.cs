using PLCSystem.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSystem.Models
{

	/// <summary>
	/// 机号及鹤位设置
	/// </summary>

    [Serializable]
    public class SYSCraneSetModel:NotifyBase
    {
		public SYSCraneSetModel()
		{ }
		#region Model
		private int _cranecode;
		private int _machineno;
		private string _craneNO;
		private int _status = 1;
		/// <summary>
		/// 
		/// </summary>
		public int CraneCode
		{
			set { _cranecode = value; }
			get { return _cranecode; }
		}
		/// <summary>
		/// 机号
		/// </summary>
		public int MachineNo
		{
			set { _machineno = value; this.NotifyChanged(); }
			get { return _machineno; }
		}
		/// <summary>
		/// 鹤位号
		/// </summary>
		public string CraneNO
		{
			set { _craneNO = value; this.NotifyChanged(); }
			get { return _craneNO; }
		}
		/// <summary>
		/// 状态（1.启用  2.禁用）
		/// </summary>
		public int Status
		{
			set { _status = value; this.NotifyChanged(); }
			get { return _status; }
		}
		#endregion Model



	}
}
