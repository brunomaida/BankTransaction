using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankLib.Rules
{
	/// <summary>
	/// Serializable Days to Expire Rule Class from RuleCriteria
	/// </summary>
	[Serializable]
	public class SectorRuleCriteria : RuleCriteria
	{
		
		[XmlElement("sector")]
		public string Sector { get; set; }

		/// <summary>
		/// Evaluates the trade acording to the Comparison and Sector.
		/// </summary>
		/// <param name="trade">Trade to be evaluated.</param>
		/// <returns>True if matches, otherwise false.</returns>
		public override bool Evaluate(ITrade trade)
		{
			switch (CompareType)
			{
				case ComparisonType.None: return false;
				case ComparisonType.EqualTo: return (trade.ClientSector.ToUpperInvariant().Equals(Sector.ToUpperInvariant()));
				case ComparisonType.NotEqualTo: return (!trade.ClientSector.ToUpperInvariant().Equals(Sector.ToUpperInvariant()));
			}
			return false;
		}
	}
}
