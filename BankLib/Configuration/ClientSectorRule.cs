using BankLib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankLib.Configuration
{
    [Serializable]
   public class ClientSectorRule
   {
      public ClientSectorRule(string sector, string value, string opValueRule) => IsValid = ValidateInput(sector, value, opValueRule);

      [XmlElement("Sector")]
      public string Sector { get; private set; }

      [XmlElement("Value")]
      public double Value { get; private set; }

      [XmlElement("Sector")]
      public ComparisonType ValueRule { get; private set; }

      [XmlIgnore]
      public bool IsValid { get; private set; }

      private void ValidateInput(string sector, double value) { }

      private bool ValidateInput(string sector, string value, string opValueRule)
      {
         //Valida o nome do Setor
         if (string.IsNullOrEmpty(sector))
         {
            return false;
         }
         Sector = sector;

         //Valida o valor de referência
         if (!string.IsNullOrEmpty(value))
         {
            double v = 0;
            if (!Double.TryParse(value, out v))
            {
               //throw new Exception("Valor inválido fornecido para Comparação - Regra Cliente-Setor.");
               return false;
            }
            Value = v;
         }

         //Valida o tipo de comparação



         return true;
      }
   }
}
