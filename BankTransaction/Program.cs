using BankLib;
using BankLib.Configuration;
using BankLib.Rules;
using NLog;
using System.ComponentModel;
using System.Linq;

class Program
{
   static void Main()
   {
      try
      {
         //local settings
         string logconfig = "logconfig.xml";
         string fileName = "bankconfig.xml";
         string configPath = AppDomain.CurrentDomain.BaseDirectory + fileName;
         bool createConfig = false;

         //Initializes the BankLib logger
         LogHelper.InitializeConfiguration(logconfig);
	 LogHelper.Logger.Info("****************************************************");
	 LogHelper.Logger.Info("*** Initilizing application.");

         //==============================================================================================
         //Creation Sample of the bankconfig.xml in the current directory - based on the test
         if (createConfig)
         {
            BankConfiguration bc = new BankConfiguration(configPath);
            bc.DateFormat = "MM/dd/yyyy";

            Rule r1 = new Rule(1, "EXPIRED", new List<RuleCriteria>());
            Rule r2 = new Rule(1, "HIGHRISK", new List<RuleCriteria>());
            Rule r3 = new Rule(1, "MEDIUMRISK", new List<RuleCriteria>());

            r1.Criterias.Add(new DaysToExpireRuleCriteria { DaysToExpire = 30 });
            r2.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
            r2.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PRIVATE" });
            r3.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
            r3.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PUBLIC" });

            bc.Rules.Add(r1);
            bc.Rules.Add(r2);
            bc.Rules.Add(r3);
            bc.Save();
         }

         //===============================================================================================
         // Requests the complete Input
         Console.WriteLine("Input Reference Date, Number of Records and Trades (then press ESC to Validate):");
         ConsoleKeyInfo key;
         string input = "";
         do
         {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Escape)
            {
               input = string.Concat(input, key.KeyChar);
               if (key.KeyChar == '\r' || key.KeyChar == '\n') Console.WriteLine("");
               Console.Write(key.KeyChar);
            }
         } while (key.Key != ConsoleKey.Escape);

         Console.WriteLine("");
	 LogHelper.Logger.Info(@"Input completed: {0}", input);         

         //===============================================================================================
         //Initiazes Bank configuration
         LogHelper.Logger.Info(@"Configuration file path: {0}", configPath);
         Bank bank = Bank.Instance;
         bank.Configuration.FilePath = configPath;
         bank.Configuration.Load();

         //Validates bank configuration
         string messageValidation;
         if (bank.Configuration.ValidateConfiguration(out messageValidation))
            LogHelper.Logger.Info(messageValidation);
         else
            LogHelper.Logger.Error(messageValidation);

         //Sets the inputs parameters receveid in the console
         bank.SetInputParametersAndTrades(input);

         //===============================================================================================
         //Checks if all important inputs are ok to proceed
         if (bank.HasValidInputs)
         {
            var ret = bank.ValidateTransactions();

            foreach (var transaction in ret)
            {
               Console.WriteLine(transaction);
               LogHelper.Logger.Info(transaction);
            }
         }

         LogHelper.Logger.Info("*** Terminating application.");
      }
      catch (Exception ex)
      {
         LogHelper.Logger.Error(ex);
      }
   }
}
