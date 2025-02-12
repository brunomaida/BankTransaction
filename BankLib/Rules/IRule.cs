using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLib.Rules
{   
   /// <summary>
   /// Interface to define the main signature of a Rule.
   /// </summary>
   public interface IRule
   {
      /// <summary>
      /// Gets or Sets the priority of the rule.
      /// </summary>
      int Priority { get; set; }

      /// <summary>
      /// Gets or Sets the name of the rule.
      /// </summary>
      string Name { get; set; }

      /// <summary>
      /// Gets or sets the criterias of each rule.
      /// </summary>
      List<RuleCriteria> Criterias { get; set; }

      /// <summary>
      /// Evaluates a trade according to the criterias
      /// </summary>
      /// <param name="trade">The trade to evaluate.</param>
      /// <returns>Returns the struct with IsValid state and Result message.</returns>
      RuleValidation Evaluate(ITrade trade);

   }

}
