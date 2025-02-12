using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankLib
{
   public class TradeHelper
   {
		private static string datePattern = @"^(0[1-9]|1[0-2])\/(0[1-9]|[12][0-9]|3[01])\/\d{4}$";

      private static string tradePattern = @"^(-?\d{1,10})\s+([a-zA-Z0-9]+)\s+(0[1-9]|1[0-2])\/(0[1-9]|[12][0-9]|3[01])\/\d{4}$";

      /// <summary>
      /// Validates if an input date string has MM/dd/yyyy date format;
      /// </summary>
      /// <param name="dateInput">The input date string</param>
      /// <returns>True if satisfies. Otherwise, False.</returns>
		public static bool ValidateDateInputFormat(string dateInput)
      {
         return Regex.IsMatch(dateInput, datePattern);
      }

		/// <summary>
		/// Validates if an input Trade string has the expected format with spaces;
		/// </summary>
		/// <param name="dateInput">The input Trade string.</param>
		/// <returns>True if satisfies. Otherwise, False.</returns>
		public static bool ValidateTradeInputFormat(string tradeInput)
      {
         return Regex.IsMatch(tradeInput, tradePattern);
      }

      /// <summary>
      /// Splits a trade string sequence with \r, \r\n or \n. 
      /// </summary>
      /// <param name="tradeInput">The input Trade string.</param>
      /// <returns>An array with all trades.</returns>
      public static string[] Split(string input)
      {         
			return input.Split(new[] { "\r", "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		}
   }
}
