using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankLib.Rules
{
	/// <summary>
	/// Serializable Inherited Capital Rule Class from RuleCriteria
	/// </summary>
	[Serializable]	
	public class CapitalRuleCriteria : RuleCriteria
	{
		/// <summary>
		/// The value of the trade.
		/// </summary>
		[XmlElement("capital")]
		public double Capital { get; set; }

		/// <summary>
		/// Evaluates the trade acording to the Comparison and Capital.
		/// </summary>
		/// <param name="trade">Trade to be evaluated.</param>
		/// <returns>True if matches, otherwise false.</returns>
		public override bool Evaluate(ITrade trade)
		{
			switch (CompareType)
			{
				case ComparisonType.None: return false;
				case ComparisonType.GreaterOrEqualTo: return (trade.Value >= Capital);
				case ComparisonType.GreaterThan: return (trade.Value > Capital);
				case ComparisonType.LowerThan: return (trade.Value < Capital);
				case ComparisonType.LowerOrEqualTo: return (trade.Value <= Capital);
				case ComparisonType.EqualTo: return (trade.Value == Capital);
				case ComparisonType.NotEqualTo: return (trade.Value != Capital);
			}
			return false;
		}
	}
}
