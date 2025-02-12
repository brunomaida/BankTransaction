using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLib
{
   /// <summary>
   /// Represents the basic fields of a Trade.
   /// </summary>
   public interface ITrade
   {
		/// <summary>
		/// Gets or Sets the Value of the Trade.
		/// </summary>
		double Value { get; set; }

		/// <summary>
		//// Gets or Sets the ClientSector of the Trade.
		/// </summary>
		string ClientSector { get; set; }

		/// <summary>
		/// Gets or Sets the NextPaymentDate of the Trade.
		/// </summary>
		DateTime NextPaymentDate { get; set; }

		/// <summary>
		/// Gets or Sets the Reference Data to compare this trade's NextPaymentDate.
		/// </summary>
		DateTime ReferenceDate { get; set; }

		/// <summary>
		/// Returns if the current trade has Valid inputs or Not.
		/// </summary>
		bool IsValid { get; }
   }
}
