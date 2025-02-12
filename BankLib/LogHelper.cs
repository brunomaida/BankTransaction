using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace BankLib
{
	/// <summary>
	/// Log Helper for the Lib and App.
	/// </summary>
	public class LogHelper
	{
		/// <summary>
		/// The Logger engine
		/// </summary>
		public static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Loads the XML configuration file (with targets and formats)
		/// </summary>
		/// <param name="filePath"></param>
		public static void InitializeConfiguration(string filePath)
		{
			LogManager.Setup().LoadConfigurationFromFile(filePath);
		}
	}
}
