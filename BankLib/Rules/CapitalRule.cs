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
   public class CapitalRule : Rule
   {
      private DateTime _expiration;

      public CapitalRule() { }

      public CapitalRule(double capital, int priority, string name, ComparisonType compType, string sector) 
         : base(priority, name, compType, capital, sector)
      {         
      }

      [XmlIgnore]
      public override RuleType Type => RuleType.CapitalCompare;

      [XmlElement("capital")]
      public double Capital 
      { 
         get
         {
            return this.Value;
         }
         set
         {
            this.Value = value;
         }
      }

      public RuleValidation Evaluate(double capital, DateTime referenceDate, string sector)
      {
         //Validates the Capital Reference
         bool r1 = false;
         switch (CompareType)
         {
            case ComparisonType.None:
               {
                  Status = RuleStatus.Error_ComparisonNotDefined;
                  break;
               }
            case ComparisonType.GreaterOrEqualTo:
               {
                  r1 = (Value >= value);
                  break;
               }
            case ComparisonType.GreaterThan:
               {
                  r1 = (Value > value);
                  break;
               }
            case ComparisonType.LowerThan:
               {
                  r1 = (Value < value);
                  break;
               }
            case ComparisonType.LowerOrEqualTo:
               {
                  r1 = (Value <= value);
                  break;
               }
            case ComparisonType.EqualTo:
               {
                  r1 = (Value == value);
                  break;
               }
            case ComparisonType.NotEqualTo:
               {
                  r1 = (Value != value);
                  break;
               }
         }

         //Validates the Customer SECTOR
         bool r2 = true;
         if (!string.IsNullOrEmpty(Sector))
         {
            r2 = Sector.ToUpper().Equals(sector.ToUpper());
         }

         //Returns the if the rule completely match and proper RESULT
         if (r1 && r2) return new RuleValidation { IsValid = true, Result = Name };
         return new RuleValidation { IsValid = false, Result = string.Format(@"Rule {0} - Priority {1}", Name.ToUpper(), Priority) };
      }

   }
}
