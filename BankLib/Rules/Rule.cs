using BankLib.Rules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankLib.Rules
{
   /// <summary>
   /// 
   /// </summary>
   [Serializable]
	[XmlType("rule")]
	public class Rule : IRule
   {
      [XmlElement("priority")]
      public int Priority { get; set; }

      [XmlElement("name")]
      public string Name { get; set; }

      [XmlArray]
      public List<RuleCriteria> Criterias { get; set; }

      internal Rule() { } //XML serialization purpose

      public Rule(int priority, string name) :
         this(priority, name, new List<RuleCriteria>())
      { }

      public Rule(int priority, string name, List<RuleCriteria> criterias)
      {
         Priority = priority;
         Name = name;
         Criterias = criterias;
      }

      public virtual RuleValidation Evaluate(ITrade trade)
      {
			RuleValidation validation = new RuleValidation { IsValid = false, Result = "NOT VALID" };
			try
         {
            foreach (RuleCriteria criteria in Criterias)
            {
               //if one of the criteria do not match the rule does not apply to the trade
               if (!criteria.Evaluate(trade)) return validation;
            }
            //the rule criterias fits all trade info
            validation.IsValid = true;
            validation.Result = Name;            
            return validation;
         }
         finally
         {
            LogHelper.Logger.Debug("RuleValidation - IsValid: {0}, Result: {1}", validation.IsValid, validation.Result);
         }
		}

   }
}
