using System.Formats.Asn1;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Xml.Serialization;

namespace BankLib
{   
   /// <summary>
   /// A class object that Implements ITrade and has all basic trade inputs.
   /// </summary>
   public class Trade : ITrade
   {
      private readonly CultureInfo _cultureInfo = new CultureInfo("en-US");

      /// <summary>
      /// Gets or Sets the Value of the Trade.
      /// </summary>
      public double Value { get; set; }

		/// <summary>
		/// Gets or Sets the ClientSector of the Trade.
		/// </summary>
		public string ClientSector { get; set; }

		/// <summary>
		/// Gets or Sets the NextPaymentDate of the Trade.
		/// </summary>
		public DateTime NextPaymentDate { get; set; }

		/// <summary>
		/// Gets or Sets the Reference Data to compare this trade's NextPaymentDate.
		/// </summary>
		public DateTime ReferenceDate { get; set; }

      /// <summary>
      /// Returns if the current trade has Valid inputs or Not.
      /// </summary>
		public bool IsValid { get; private set; }

      /// <summary>
      /// Gets or Sets the DateFormat to convert Strings into DateTime objects.
      /// </summary>
      public string DateFormat { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="referenceDate">The Reference date to compare Payment dates.</param>
		/// <param name="trade">The trade info the will be stored.</param>
		/// <param name="dateFormat">The date format to convert Strins into DateTime objects.</param>
		public Trade(DateTime referenceDate, string trade, string dateFormat)
      {
         ReferenceDate = referenceDate;
         DateFormat = dateFormat;
			UpdateTradeInfo(trade);
      }

		/// <summary>
		/// Updates the trade information stored.
		/// </summary>
		/// <param name="trade">The string containing trade informations.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void UpdateTradeInfo(string trade)
      {
         trade.ThrowIfNull("trade");

         bool b1, b2;

         if (!TradeHelper.ValidateTradeInputFormat(trade))
         {
            IsValid = false;
            return;
         }

         string[] data = trade.Split(' ');

         //Valida o valor : data[0]
         double d0;
         b1 = Double.TryParse(data[0], out d0);
         Value = d0;

         //Valida o valor : data[1]
         ClientSector = data[1];

         //Valida o valor : data[2]
         DateTime d2;
         b2 = DateTime.TryParseExact(data[2], DateFormat, _cultureInfo, DateTimeStyles.None, out d2);
         NextPaymentDate = d2;

         LogHelper.Logger.Debug(@"UpdateTradeInfo - Value: {0}, Sector: {1} PaymentDate: {2}", Value, ClientSector, NextPaymentDate);

         IsValid = b1 && b2;
      }
   }   
}
