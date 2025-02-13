using BankLib.Configuration;
using BankLib.Rules;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLib
{
   /// <summary>
   /// Main Lib class: Stores configuration, sets main input parameters, Receives and Process trades according to readable rules
   /// </summary>
   public class Bank
   {
      private static Bank _instance;

      private string LINE_EVALUATION_INVALID_TRADE = "INVALID TRADE INFO [Line {0}]- please check trades.";
		private string LINE_EVALUATION_NO_RULE_MATCH = "NO RULE/CRITERIA [Line {0}]- please check rules/criterias.";

      /// <summary>
      /// Returns the one and only instance allowed
      /// </summary>
      public static Bank Instance
      {
         get
         {
            if (_instance == null)
            {
               _instance = new Bank();
            }
            return _instance;
         }
      }

      /// <summary>
      /// Reference date used to compare Payment Dates
      /// </summary>
		public DateTime ReferenceDate
      {
         get;
         private set;
      }

      /// <summary>
      /// The number of Trades to process
      /// </summary>
      public uint NumberOfOperations
      {
         get;
         private set;
      }

      /// <summary>
      /// Configuration control to load and save rules with its criteria.
      /// </summary>
      public BankConfiguration Configuration
      {
         get;         
         private set;
      }

      /// <summary>
      /// List of trades created after the text input.
      /// </summary>
      public List<ITrade> Trades
      {
         get;
         private set;
      }

      /// <summary>
      /// Indicates if all data received has the expected content and format.
      /// </summary>
      public bool HasValidInputs { get; private set; }

      /// <summary>
      /// Cria uma nova instância
      /// </summary>
      private Bank()
      {
         Configuration = new BankConfiguration();
      }

      /// <summary>
      /// Initializes a new instance 
      /// </summary>
      /// <param name="rulesFilePath">The complete path of the configuration file where important params and rules are stored</param>
      private Bank(string rulesFilePath) : this()
      {
         //Faz a leitura do arquivo de regras de Clientes
         Configuration.FilePath = rulesFilePath;
         Configuration.Load();
      }

		/// <summary>
		/// Initializes a new instance 
		/// </summary>
		/// <param name="rulesFilePath">The complete path of the configuration file where important params and rules are stored</param>
		/// <param name="refDate">The Reference date to compare Payment dates.</param>
		/// <param name="count">The number of records to be processed.</param>
		/// <param name="trades">The list of trades to be processed.</param>
		private Bank(string rulesFilePath, string refDate, string count, string trades)
         :this(rulesFilePath)
      {         
         SetInputParametersAndTrades(refDate, count, trades);
      }

		/// <summary>
		/// Updates the inputs and validates all data and formats if local file was specified in bank configuration.
		/// </summary>		
		public void SetInputParametersAndTradesFromFile()
		{
         if (Configuration.InputFile.Length == 0)
            throw new FileLoadException("Input File not specified");

         string input = TradeHelper.ReadInputFile(Configuration.InputFile);
         SetInputParametersAndTrades(input);
		}

		/// <summary>
		/// Updates the inputs and validates all data and formats.
		/// </summary>
		/// <param name="all">All inputs together: The Reference date to compare Payment dates, The number of records to be processed and The list of trades to be processed.</param>
		public void SetInputParametersAndTrades(string all)
      {
         int len = all.Contains("\r\n") ? 4 : 2;         
         string[] split = TradeHelper.Split(all);
         all = all.TrimEnd().Substring(split[0].Length + split[1].Length + len);
         SetInputParametersAndTrades(split[0], split[1], all);
      }

		/// <summary>
		/// Updates the inputs and validates all data and formats.
		/// </summary>
		/// <param name="refDate">The Reference date to compare Payment dates.</param>
		/// <param name="count">The number of records to be processed.</param>
		/// <param name="trades">The list of trades to be processed.</param>
		public void SetInputParametersAndTrades(string refDate, string count, string trades)
      {
         //Initilizes the validation as if the input will be totatly correct
         HasValidInputs = true;

         //Valida a data de referência para comparação de pagamentos
         DateTime date;
         bool b1 = DateTime.TryParseExact(refDate, Configuration.DateFormat, new CultureInfo("en-US"), DateTimeStyles.None, out date);   
         ReferenceDate = date;

         //Valida o número de transações
         uint num = 0;
         bool b2 = uint.TryParse(count, out num);
         NumberOfOperations = num;

         //Valida as transações recebidas
         Trades = ValidateTrades(trades);
         bool b3 = Trades.All(t => t.IsValid);

         HasValidInputs = b1 && b2; // && b3;

         LogHelper.Logger.Debug("Bank HasValidInputs(after SetInputParametersAndTrades): {0}", HasValidInputs);
      }

      /// <summary>
      /// Validates all trades according to the priority of the rules
      /// </summary>
      /// <returns>List of rule Names for each trade (Error message for those without any match).</returns>
      public List<string> ValidateTransactions() 
      {
         int i = 1;
         var list = new List<string>();
         foreach (Trade trade in Trades)
         {
            //Evaluate all transactions in the list based on the rules
            if (!trade.IsValid)
            {
               list.Add(string.Format(LINE_EVALUATION_INVALID_TRADE, i));
            } 
            else
            {
               string res = string.Format(LINE_EVALUATION_NO_RULE_MATCH, i);

               //Evaluates each rule in the correct order of precende (previously ordered)
               foreach (IRule r in Configuration.Rules)
               {
                  RuleValidation rv = r.Evaluate(trade);
                  if (rv.IsValid)
                  {
                     res = rv.Result; //Found a match - exits the loop
                     break;
                  }                  
               }   
               list.Add(res);
               LogHelper.Logger.Debug("Line {0} result: {1}", i, res);
            }

            //Checks the list based on the Input counter - not the trade list size
            if (i++ >= NumberOfOperations) break;
         }
         return list;
      }

		/// <summary>
		/// Evaluates the string with all trades to be evaluated.
		/// </summary>
		/// <param name="trades">The complete string containing all trades to be evaluated.</param>
		/// <returns>A list of ITrade objects with proper datetime and double formats.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		private List<ITrade> ValidateTrades(string trades)
      {
         trades.ThrowIfNull("trades");
         var list = new List<ITrade>();
         var lines = TradeHelper.Split(trades); 
         foreach (string line in lines)
         {
            Trade t = new Trade(ReferenceDate, line, Configuration.DateFormat);
            list.Add(t);
			}
         return list;
      }

   }
}
