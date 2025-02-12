using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLib
{
   public static class ValidationExtensions
   {
      /// <summary>
      /// Evaluates if the current object is Null.
      /// </summary>
      /// <param name="o">The object.</param>
      /// <param name="objectName">The name for the Exception if Null.</param>
      /// <exception cref="ArgumentNullException"></exception>
      public static void ThrowIfNull(this object o, string objectName)
      {
         if (o == null)
         {
            throw new ArgumentNullException(string.Format("{0} cannot be NULL.", objectName));
         }
      }

		/// <summary>
		/// Evaluates if the current object is Null.
		/// </summary>
		/// <param name="o">The object.</param>
		/// <param name="ex">The Exception chosen to be thrown.</param>
		public static void ThrowIfNull(this object o, Exception ex)
      {
         if (o == null)
         {
            throw ex;
         }
      }
   }
}
