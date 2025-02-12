using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLib.Rules
{
   /// <summary>
   /// Struct to store result State and Message for a Rule validation.
   /// </summary>
   public struct RuleValidation
   {
      /// <summary>
      /// Gets or Sets the state of the result (True/False).
      /// </summary>
      public bool IsValid { get; set; }

      /// <summary>
      /// Gets or Sets the result message.
      /// </summary>
      public string Result { get; set; }
   }
}
