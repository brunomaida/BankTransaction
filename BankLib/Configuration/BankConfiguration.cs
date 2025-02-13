using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BankLib.Rules;
using NLog;

namespace BankLib.Configuration
{
   /// <summary>
   /// Configuration Class to store all Bank relevant rules: date formats, rules and criterias.
   /// </summary>
   [Serializable]
   [XmlType("bankconfiguration")]
   public class BankConfiguration : XmlConfiguration
   {
		public static string BANK_CONFIG_SUCCESS = "SUCCESS";
		public static string BANK_CONFIG_ERROR_NO_RULES = "THERE ARE NO RULES";
		public static string BANK_CONFIG_ERROR_NO_CRITERIA_IN_SOMERULES = "SOME RULES WITHOUT CRITERIA";
		public static string BANK_CONFIG_ERROR_NO_CRITERIA_IN_ALLRULES = "ALL RULES WITHOUT CRITERIA";

      /// <summary>
      /// Initializes a new Instance of BankConfiguration.
      /// </summary>
		public BankConfiguration() : this("")
      {
		}

		/// <summary>
		/// Initializes a new Instance of BankConfiguration.
		/// </summary>
		/// <param name="filePath">The configuration file with relevant parameters.</param>
		public BankConfiguration(string filePath) : base(filePath) 
      {
         Rules = new List<Rule>();
         InputFile = "";
      }

      /// <summary>
      /// Gets or Sets the Date format used to evaluate Reference Data and Trades.
      /// </summary>
      [XmlElement("dateformat")]
      public string DateFormat { get; set; }

		/// <summary>
		/// Gets or Sets the file with parameters and trade inputs. If blank, will not be used.
		/// </summary>
		[XmlElement("inputfile")]
		public string InputFile { get; set; }

		/// <summary>
		/// Gets or sets the List of Rules (and its criteria) used to evaluate Trades.
		/// </summary>
		[XmlArray("rules")]
      public List<Rule> Rules { get; set; } 

      /// <summary>
      /// Reads the file and stores it in memory.
      /// </summary>
      public override void Load()
      {
			LogHelper.Logger.Info("Loading XML Config file...");
			var bc = XmlConfigurationHelper.Load<BankConfiguration>(this.FilePath);
			LogHelper.Logger.Info("Done!");

			Rules = bc.Rules;
         DateFormat = bc.DateFormat;
         InputFile = bc.InputFile;

         LogHelper.Logger.Debug("Reordering Rules by PRIORITY.");

         //Reorganiza a ordem das regras
         Rules.Sort(new PriorityOrderAscendingComparer());
      }

		/// <summary>
		/// Reads the file and stores it in memory.
		/// </summary>
		/// <param name="filePath">The configuration file with relevant parameters.</param>
		public void Load(string filePath)
      {         
         FilePath = filePath;
         Load();
      }

      /// <summary>
      /// Validates the configuration.
      /// </summary>
      /// <param name="message">The output success or error message.</param>
      /// <returns>True if valid, otherwise false.</returns>
      public bool ValidateConfiguration(out string message)
      {
         //1: Validates if there are any Rules
         if (Rules.Count <= 0)
         {
            message = BANK_CONFIG_ERROR_NO_RULES;
            return false;
         }

			//2: Rules with no criterias
			List<bool> noCrit = new List<bool>();
			foreach (var rule in Rules) 
         {
            if (rule.Criterias.Count > 0) noCrit.Add(true);
         }
         if (noCrit.Count == 0)
         {
				message = BANK_CONFIG_ERROR_NO_CRITERIA_IN_ALLRULES;
				return false;
         }
         if (noCrit.Count > 0 && noCrit.Count < Rules.Count)
         {
            message = BANK_CONFIG_ERROR_NO_CRITERIA_IN_SOMERULES;
            return false;
         }

         /* ------- When using Sector List (maybe not necessary)
         //n: Validates sectors in the rules
         List<bool> critList = new List<bool>();
         foreach (string sector in Sectors)
         {
            bool find = false;
            foreach (var rule in Rules)
            {
               foreach (var criteria in rule.Criterias) 
               {
                  if (criteria is SectorRuleCriteria) find = (criteria as SectorRuleCriteria).Sector == sector;
               }
               if (find) break;
            } 
            //Adds the final result to check if sector was found
            critList.Add(find);
         }
         */

         //Success in the validation of Rules and its Criterias
         message = BANK_CONFIG_SUCCESS;
         return true;
      }
   }
}
