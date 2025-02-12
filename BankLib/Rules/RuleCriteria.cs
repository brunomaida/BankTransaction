using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankLib.Rules
{
	[XmlInclude(typeof(CapitalRuleCriteria))]
	[XmlInclude(typeof(SectorRuleCriteria))]
	[XmlInclude(typeof(DaysToExpireRuleCriteria))]
	public class RuleCriteria : IRuleCriteria
	{
		private ComparisonType _comparisonType;

		public RuleCriteria()
		{
			CompareType = ComparisonType.None;
		}

		[XmlElement("comparetype")]
		public virtual ComparisonType CompareType 
		{ 
			get { return _comparisonType; } 
			set { _comparisonType = value; } 
		}

		public virtual bool Evaluate(ITrade trade) { return false; }
	}
}
