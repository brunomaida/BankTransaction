using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankLib.Rules
{
	/// <summary>
	/// Serializable Days to Expire Rule Class from RuleCriteria
	/// </summary>
	[Serializable]	
	public class DaysToExpireRuleCriteria : RuleCriteria
	{
		/// <summary>
		/// The number of days to compare to the reference and payment dates.
		/// </summary>
		[XmlElement("daystoexpire")]
		public uint DaysToExpire { get; set; }

		/// <summary>
		/// Evaluates the trade acording to the Comparison and Days to expire.
		/// </summary>
		/// <param name="trade">Trade to be evaluated.</param>
		/// <returns>True if matches, otherwise false.</returns>
		public override bool Evaluate(ITrade trade)
		{
			return trade.NextPaymentDate.AddDays(DaysToExpire) < trade.ReferenceDate;
		}
	}
}
