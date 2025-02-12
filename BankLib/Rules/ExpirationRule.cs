using BankLib.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BankLib.Rules
{
   [Serializable]   
   public class ExpirationRule : Rule
   {

      public ExpirationRule() { }

      public ExpirationRule(uint daysFromDate, int priority, string name, ComparisonType compType, string sector) 
         : base(priority, name, compType, daysFromDate, sector)
      {
         DaysFromDate = daysFromDate;
      }

      public ExpirationRule(string daysFromDate, int priority, string name, ComparisonType compType,  string sector) 
         : this(uint.Parse(daysFromDate), priority, name, compType, sector) { }

      [XmlIgnore]
      public override RuleType Type => RuleType.DateCompare;

      [XmlElement("expirationdate")]
      public uint DaysFromDate 
      { 
         get
         {
            return (uint)this.Value;
         }
         set
         {
            this.Value = DaysFromDate;
         }
      }

      public RuleValidation Evaluate(DateTime date, string sector)
      {
         return this.Evaluate(date.Ticks, sector);
      }

   }
}
