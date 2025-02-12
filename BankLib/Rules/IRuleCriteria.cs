using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankLib.Rules
{
	/// <summary>
	/// Interface to define the main signature of a Criteria.
	/// </summary>
	public interface IRuleCriteria
	{
		/// <summary>
		/// Gets or Sets the type of Comparison
		/// </summary>
		ComparisonType CompareType { get; set; }
		
		/// <summary>
		/// Evaluates the trade according to the criteria.
		/// </summary>
		/// <param name="trade">The trade to be evaluated.</param>
		/// <returns>True if valid, otherwise False.</returns>
		bool Evaluate(ITrade trade);
	}
}
