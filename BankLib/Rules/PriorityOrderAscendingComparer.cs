using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLib.Rules
{
   /// <summary>
   /// Ascending Comparer to order the Rules.
   /// </summary>
   public class PriorityOrderAscendingComparer : IComparer<Rule>
   {
      /// <summary>
      /// Compares the Priority of 2 Rules.
      /// </summary>
      /// <param name="x">Rule Instance.</param>
      /// <param name="y">Rule Value.</param>
      /// <returns>Less than 0: Rule instance is less than Rule Value / Zero: Values are equal. / Larger than 0: Rule instance is more than Rule value.</returns>
      public int Compare(Rule x, Rule y)
      {
         return x.Priority.CompareTo(y.Priority);
      }
   }
}
