using BankLib;
using BankLib.Configuration;
using BankLib.Rules;

namespace Tests
{
   public class BankLibTests
   {
		Bank _bank = Bank.Instance;

		readonly string _fileName = "bankconfig.xml";
		readonly string _configPath = AppDomain.CurrentDomain.BaseDirectory;

		readonly string _dateFormat1 = "MM/dd/yyyy";
		readonly string _dateFormat2 = "dd/MM/yyyy";

		readonly DateTime _now = DateTime.Now;
		readonly DateTime _referenceDate1 = new DateTime(2024, 12, 01);
		readonly DateTime _referenceDate2 = new DateTime(2023, 12, 01);
		readonly DateTime _referenceDate3 = new DateTime(2022, 12, 01);

		readonly ITrade _defaultTrade = new Trade(DateTime.Now, "", "MM/dd/yyyy");

		readonly string _trade_f1 = "400000-Public-07/01/2020"; //fail
		readonly string _trade_f2 = "4x000-3455-07/01/20"; //fail
		readonly string _trade_f3 = "12/12/2024- Private 12"; //fail

		readonly string _trade0 = "400000 Public 07/29/2020";
		readonly string _trade00 = "40340 Private 12/27/2020";
		readonly string _trade000 = "1220340 Private 10/26/2023";

		readonly string _trade1 = "2000000 Private 12/29/2025\r\n" +
										  "400000 Public 07/01/2020\r\n" +
										  "5000000 Public 01/02/2024\r\n" +
										  "3000000 Public 10/26/2023";

		readonly List<string> _result1 = new List<string> { "", "", "", "" };

		readonly string _trade_1 = "2000000-Private-12/29/2025\r\n" +
										  "400000 Public 07/01/2020\r\n" +
										  "5000000-Public-01/02/2024\r\n" +
										  "3000000 Public 10/26/2023";

		readonly List<string> _result2 = new List<string> { "", "", "", "", "", "", "" };

		readonly string _trade2 = "2000000 Private 12/29/2025\r\n" +
										  "400000 Public 07/01/2022\r\n" +
										  "5000000 Public 01/02/2024\r\n" +
										  "1000000 Private 10/26/2023\r\n" +
										  "10000 Public 07/01/2022\r\n" +
										  "50000000 Public 01/02/2026\r\n" +
										  "4500000 Private 10/26/2024";

		readonly List<string> _result3 = new List<string> { "HIGHRISK", "EXPIRED", "EXPIRED", "HIGHRISK", "MEDIUMRISK", "HIGHRISK", "EXPIRED", "MEDIUMRISK", "EXPIRED", "HIGHRISK", "NORISK", "MEDIUMRISK", "HIGHRISK" };

		readonly string _trade3 = "2000000 Private 12/29/2025\r\n" +
										  "400000 Public 07/01/2024\r\n" +
										  "5000 Small 01/02/2024\r\n" +
										  "1000000 Private 10/26/2027\r\n" +
										  "3400000 Public 07/01/2026\r\n" +
										  "5000000 Private 01/02/2029\r\n" +
										  "45000 Small 10/26/2022\r\n" +
										  "800000 Public 07/01/2053\r\n" +
										  "20000000 Private 01/02/2024\r\n" +
										  "40000000 Private 10/26/2025\r\n" +
										  "13000 Small 07/01/2026\r\n" +
										  "25000000 Public 01/02/2027\r\n" +
										  "450000000 Private 10/26/2026";

		readonly List<string> _resultOrig = new List<string> { "HIGHRISK", "EXPIRED", "MEDIUMRISK", "MEDIUMRISK" };

		readonly string _tradeOriginal = "2000000 Private 12/29/2025\r\n" +
													"400000 Public 07/01/2020\r\n" +
													"5000000 Public 01/02/2024\r\n" +
													"3000000 Public 10/26/2023";

		readonly List<string> _resultOrig1 = new List<string> { "HIGHRISK", "EXPIRED", "MEDIUMRISK", "HIGHRISK", "HIGHRISK", "MEDIUMRISK" };

		readonly string _tradeOriginal1 = "2000000 Private 12/29/2025\r\n" +
													"400000 Public 07/01/2020\r\n" +
													"5000000 Public 01/02/2024\r\n" +
													"5000000 Private 01/02/2024\r\n" +
													"5000000 Private 01/02/2024\r\n" +
													"3000000 Public 10/26/2023";

		[SetUp]
      public void Setup()
      {
			_bank = Bank.Instance;
			_bank.Configuration.FilePath = _configPath + _fileName;
			_bank.Configuration.DateFormat = "MM/dd/yyyy";
		}

		#region BankConfiguration Tests

		[Test]
		public void Test_ConfigFileExists_True()
		{
			BankConfiguration bc = new BankConfiguration();
			bc.FilePath = _configPath + _fileName;
			Assert.That(bc.FileExists);
		}

		[Test]
		public void Test_ConfigFileDoesNotExists_True()
		{
			BankConfiguration bc = new BankConfiguration();
			bc.FilePath = _fileName + ".xml";
			Assert.That(!bc.FileExists);
		}

		[Test]
		public void Test_ConfigLoadOK_True()
		{
			try
			{
				BankConfiguration bc = new BankConfiguration();
				bc.FilePath = _configPath + _fileName;
				bc.Load();
				Assert.Pass();
			}
			catch (Exception ex)
			{
				if (!(ex is SuccessException))
					Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void Test_ConfigLoadNotOK_True()
		{
			try
			{
				BankConfiguration bc = new BankConfiguration();
				bc.FilePath = _fileName;
				bc.Load();
				Assert.Fail();
			}
			catch (Exception ex)
			{
				Assert.Pass(); ;
			}
		}

		[Test]
      public void Test_BankConfigurationCreate_1()
      {
			try
			{
				BankConfiguration bc = new BankConfiguration(_configPath + _fileName);
				bc.DateFormat = "MM/dd/yyyy";

				Rule r1 = new Rule(1, "EXPIRED", new List<RuleCriteria>());
				Rule r2 = new Rule(2, "HIGHRISK", new List<RuleCriteria>());
				Rule r3 = new Rule(3, "MEDIUMRISK", new List<RuleCriteria>());

				r1.Criterias.Add(new DaysToExpireRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, DaysToExpire = 30 });
				r2.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
				r2.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PRIVATE" });
				r3.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
				r3.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PUBLIC" });

				bc.Rules.Add(r1);
				bc.Rules.Add(r2);
				bc.Rules.Add(r3);

				bc.Save();
				Assert.Pass();				
			}
			catch (Exception ex) 
			{
				if (!(ex is SuccessException))
					Assert.Fail(ex.Message);
			}			
      }

		[Test]
		public void Test_BankConfigurationCreate_2()
		{
			try
			{
				BankConfiguration bc = new BankConfiguration(_configPath + _fileName);
				bc.DateFormat = "MM/dd/yyyy";

				Rule r1 = new Rule(1, "EXPIRED", new List<RuleCriteria>());
				Rule r2 = new Rule(2, "HIGHRISK", new List<RuleCriteria>());
				Rule r3 = new Rule(3, "MEDIUMRISK", new List<RuleCriteria>());
				Rule r4 = new Rule(4, "NORISK", new List<RuleCriteria>());

				r1.Criterias.Add(new DaysToExpireRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, DaysToExpire = 30 });
				r2.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
				r2.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PRIVATE" });
				r3.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
				r3.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PUBLIC" });
				r4.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.LowerOrEqualTo, Capital = 100000 });
				r4.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "SMALL" });

				bc.Rules.Add(r1);
				bc.Rules.Add(r2);
				bc.Rules.Add(r3);
				bc.Rules.Add(r4);

				bc.Save();
				Assert.Pass();
			}
			catch (Exception ex)
			{
				if (!(ex is SuccessException))
					Assert.Fail(ex.Message);
			}			
		}

		[Test]
		public void Test_BankConfigurationCreate_3()
		{
			try
			{
				BankConfiguration bc = new BankConfiguration(_configPath + _fileName);
				bc.DateFormat = "MM/dd/yyyy";

				Rule r1 = new Rule(1, "EXPIRED", new List<RuleCriteria>());

				r1.Criterias.Add(new DaysToExpireRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, DaysToExpire = 10 });
				r1.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PRIVATE" });

				bc.Rules.Add(r1);
				bc.Save();
				Assert.Pass();
			}
			catch (Exception ex)
			{
				if (!(ex is SuccessException))
					Assert.Fail(ex.Message);
			}
		}

		public void Test_BankConfiguration_CannotCreate_True()
		{
			try
			{
				BankConfiguration bc = new BankConfiguration();
				bc.DateFormat = "MM/dd/yyyy";

				Rule r1 = new Rule(1, "EXPIRED", new List<RuleCriteria>());

				r1.Criterias.Add(new DaysToExpireRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, DaysToExpire = 10 });
				r1.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PRIVATE" });

				bc.Rules.Add(r1);
				bc.Save();
			}
			catch (Exception ex)
			{
				Assert.Pass();
			}
		}

		[Test]
		public void Test_BankConfiguration_CheckRules_OK()
		{
			try
			{
				BankConfiguration bc = new BankConfiguration(_configPath + _fileName);
				bc.DateFormat = "MM/dd/yyyy";

				Rule r1 = new Rule(1, "EXPIRED", new List<RuleCriteria>());
				Rule r2 = new Rule(2, "HIGHRISK", new List<RuleCriteria>());
				Rule r3 = new Rule(3, "MEDIUMRISK", new List<RuleCriteria>());
				Rule r4 = new Rule(4, "NORISK", new List<RuleCriteria>());

				r1.Criterias.Add(new DaysToExpireRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, DaysToExpire = 30 });
				r2.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
				r2.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PRIVATE" });
				r3.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
				r3.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PUBLIC" });
				r4.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.LowerOrEqualTo, Capital = 100000 });
				r4.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "SMALL" });

				bc.Rules.Add(r1);
				bc.Rules.Add(r2);
				bc.Rules.Add(r3);
				bc.Rules.Add(r4);

				string m;
				bool v = bc.ValidateConfiguration(out m);
				Assert.That(m == BankConfiguration.BANK_CONFIG_SUCCESS && v);
			}
			catch (Exception ex)
			{
				if (!(ex is SuccessException))
					Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void Test_BankConfiguration_CheckRules_MissingSomeCriteria()
		{
			try
			{
				BankConfiguration bc = new BankConfiguration(_configPath + _fileName);
				bc.DateFormat = "MM/dd/yyyy";

				Rule r1 = new Rule(1, "EXPIRED", new List<RuleCriteria>());
				Rule r2 = new Rule(2, "HIGHRISK", new List<RuleCriteria>());
				Rule r3 = new Rule(3, "MEDIUMRISK", new List<RuleCriteria>());
				Rule r4 = new Rule(4, "NORISK", new List<RuleCriteria>());

				r1.Criterias.Add(new DaysToExpireRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, DaysToExpire = 30 });
				r2.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
				r2.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PRIVATE" });

				bc.Rules.Add(r1);
				bc.Rules.Add(r2);
				bc.Rules.Add(r3);
				bc.Rules.Add(r4);

				string m;
				bool v = bc.ValidateConfiguration(out m);
				Assert.That(m == BankConfiguration.BANK_CONFIG_ERROR_NO_CRITERIA_IN_SOMERULES && !v);
			}
			catch (Exception ex)
			{
				if (!(ex is SuccessException))
					Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void Test_BankConfiguration_CheckRules_MissingAllCriteria()
		{
			try
			{
				BankConfiguration bc = new BankConfiguration(_configPath + _fileName);
				bc.DateFormat = "MM/dd/yyyy";

				Rule r1 = new Rule(1, "EXPIRED", new List<RuleCriteria>());
				Rule r2 = new Rule(2, "HIGHRISK", new List<RuleCriteria>());
				Rule r3 = new Rule(3, "MEDIUMRISK", new List<RuleCriteria>());
				Rule r4 = new Rule(4, "NORISK", new List<RuleCriteria>());

				bc.Rules.Add(r1);
				bc.Rules.Add(r2);
				bc.Rules.Add(r3);
				bc.Rules.Add(r4);

				string m;
				bool v = bc.ValidateConfiguration(out m);
				Assert.That(m == BankConfiguration.BANK_CONFIG_ERROR_NO_CRITERIA_IN_ALLRULES && !v);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void Test_BankConfiguration_CheckRules_NoRules()
		{
			try
			{
				BankConfiguration bc = new BankConfiguration(_configPath + _fileName);
				bc.DateFormat = "MM/dd/yyyy";

				string m;
				bool v = bc.ValidateConfiguration(out m);
				Assert.That(m == BankConfiguration.BANK_CONFIG_ERROR_NO_RULES && !v);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		#endregion

		#region Rule-Criteria Validation Tests

		[Test]
		public void Test_RuleCriteria_BaseCriteria_EvaluateFalse()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new RuleCriteria());

			Assert.That(!r.Evaluate(_defaultTrade).IsValid);
		}

		#region DaysToExpire Criteria		

		[Test]
		public void Test_RuleCriteria_DaysToExpireCriteria_NotExpired_True()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new DaysToExpireRuleCriteria { DaysToExpire = 15 });
			DateTime dt1 = _referenceDate1.AddDays(18);
			_defaultTrade.ReferenceDate = _referenceDate1;
			_defaultTrade.NextPaymentDate = dt1;

			Assert.That(!r.Criterias[0].Evaluate(_defaultTrade));
		}

		[Test]
		public void Test_RuleCriteria_DaysToExpireCriteria_Expired_True()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new DaysToExpireRuleCriteria { DaysToExpire = 15 });
			DateTime dt1 = _referenceDate1.Subtract(TimeSpan.FromDays(25));
			_defaultTrade.ReferenceDate = _referenceDate1;
			_defaultTrade.NextPaymentDate = dt1;

			Assert.That(r.Criterias[0].Evaluate(_defaultTrade));
		}

		#endregion

		#region Capital Criteria

		[Test]
		public void Test_RuleCriteria_CapitalCriteria_LowerThan10000_True()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.LowerThan, Capital = 10000 });
			double cap = 9000;
			_defaultTrade.Value = cap;

			Assert.That(r.Criterias[0].Evaluate(_defaultTrade));
		}

		[Test]
		public void Test_RuleCriteria_CapitalCriteria_lowerThan10000_False()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.LowerThan, Capital = 10000 });
			double cap = 12000;
			_defaultTrade.Value = cap;

			Assert.That(!r.Criterias[0].Evaluate(_defaultTrade));
		}

		[Test]
		public void Test_RuleCriteria_CapitalCriteria_Equalto10000_True()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.EqualTo, Capital = 10000 });
			double cap = 10000;
			_defaultTrade.Value = cap;

			Assert.That(r.Criterias[0].Evaluate(_defaultTrade));
		}

		[Test]
		public void Test_RuleCriteria_CapitalCriteria_NotEqualto10000_True()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.NotEqualTo, Capital = 10000 });
			double cap = 450;
			_defaultTrade.Value = cap;

			Assert.That(r.Criterias[0].Evaluate(_defaultTrade));
		}

		[Test]
		public void Test_RuleCriteria_CapitalCriteria_LowerEqualTo10000_True()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.LowerOrEqualTo, Capital = 10000 });
			double cap = 10000;
			_defaultTrade.Value = cap;

			Assert.That(r.Criterias[0].Evaluate(_defaultTrade));
		}

		[Test]
		public void Test_RuleCriteria_CapitalCriteria_GreaterOrEqualTo10000_True()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 13000 });
			double cap = 13000;
			_defaultTrade.Value = cap;

			Assert.That(r.Criterias[0].Evaluate(_defaultTrade));
		}

		[Test]
		public void Test_RuleCriteria_CapitalCriteria_GreaterOrEqualTo11000_True()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 11000 });
			double cap = 11000;
			_defaultTrade.Value = cap;

			Assert.That(r.Criterias[0].Evaluate(_defaultTrade));
		}

		[Test]
		public void Test_RuleCriteria_CapitalCriteria_GreaterThan10000_True()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000 });
			double cap = 15000;
			_defaultTrade.Value = cap;

			Assert.That(r.Criterias[0].Evaluate(_defaultTrade));
		}

		#endregion

		#region Sector Criteria

		[Test]
		public void Test_RuleCriteria_SectorCriteria_EqualToPublic_True()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "public" });
			string sec = "public";
			_defaultTrade.ClientSector = sec;

			Assert.That(r.Criterias[0].Evaluate(_defaultTrade));
		}

		[Test]
		public void Test_RuleCriteria_SectorCriteria_EqualToPUBLICTrue()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PUBLIC" });
			string sec = "public";
			_defaultTrade.ClientSector = sec;

			Assert.That(r.Criterias[0].Evaluate(_defaultTrade));
		}

		[Test]
		public void Test_RuleCriteria_DaysToExpireCriteria_NotEqualToPublic_True()
		{
			Rule r = new Rule(1, "Name1");
			r.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.NotEqualTo, Sector = "Public" });
			string sec = "public";
			_defaultTrade.ClientSector = sec;

			Assert.That(!r.Criterias[0].Evaluate(_defaultTrade));
		}

		#endregion

		#endregion

		#region Helper Tests

		[Test]
		public void Test_StringDate_MMddyyyy_Valid_OK()
		{
			string date = "03/23/2024";
			Assert.That(TradeHelper.ValidateDateInputFormat(date));
		}

		[Test]
		public void Test_StringDate_MMddyyyy_NotValid_OK()
		{
			string date = "45/23/2024";
			Assert.That(!TradeHelper.ValidateDateInputFormat(date));
		}

		[Test]
		public void Test_StringTrade_Validate1_OK()
		{
			Assert.That(TradeHelper.ValidateTradeInputFormat(_trade0));
		}

		[Test]
		public void Test_StringTrade_Validate2_OK()
		{
			Assert.That(TradeHelper.ValidateTradeInputFormat(_trade000));
		}

		[Test]
		public void Test_StringTrade_WrongFormat1_OK()
		{
			Assert.That(!TradeHelper.ValidateTradeInputFormat(_trade_f1));
		}

		[Test]
		public void Test_StringTrade_WrongFormat2_OK()
		{
			Assert.That(!TradeHelper.ValidateTradeInputFormat(_trade_f2));
		}

		[Test]
		public void Test_StringTrade_WrongFormat3_OK()
		{
			Assert.That(!TradeHelper.ValidateTradeInputFormat(_trade_f3));
		}

		[Test]
		public void Test_StringTradeList_Format1_OK()
		{
			var lines = TradeHelper.Split(_trade1);
			foreach (var line in lines)
			{
				if (!TradeHelper.ValidateTradeInputFormat(line))
				{
					Assert.Fail();
					return;
				}
			}
			Assert.Pass();
		}

		[Test]
		public void Test_StringTradeList_Format2_OK()
		{
			var lines = TradeHelper.Split(_trade2);
			foreach (var line in lines)
			{
				if (!TradeHelper.ValidateTradeInputFormat(line))
				{
					Assert.Fail();
					return;
				}
			}
			Assert.Pass();
		}

		[Test]
		public void Test_StringTradeList_Format3_OK()
		{
			var lines = TradeHelper.Split(_trade3);
			foreach (var line in lines)
			{
				if (!TradeHelper.ValidateTradeInputFormat(line))
				{
					Assert.Fail();
					return;
				}
			}
			Assert.Pass();
		}

		#endregion

		#region Trade Tests
		[Test]
		public void Test_Trade_IsValid_OK()
		{
			Trade t = new Trade(_referenceDate1, _trade0, _dateFormat1);
			t.UpdateTradeInfo(_trade00);
			Assert.That(t.IsValid);
		}

		[Test]
		public void Test_Trade_NotValid1_OK()
		{
			Trade t = new Trade(_referenceDate1, _trade0, _dateFormat1);
			t.UpdateTradeInfo(_trade_f1);
			Assert.That(!t.IsValid);
		}

		[Test]
		public void Test_Trade_NotValid2_OK()
		{
			Trade t = new Trade(_referenceDate1, _trade0, _dateFormat1);
			t.UpdateTradeInfo(_trade_f2);
			Assert.That(!t.IsValid);
		}

		#endregion

		#region Bank Tests

		[Test]
		public void Test_Bank_NotValidInputs_Date_OK()
		{
			_bank.SetInputParametersAndTrades("143/233/212", "2", _trade0);
			Assert.That(!_bank.HasValidInputs);
		}

		[Test]
		public void Test_Bank_NotValidInputs_Count_OK()
		{
			_bank.SetInputParametersAndTrades("12/01/2024", "_", _trade0);
			Assert.That(!_bank.HasValidInputs);
		}

		[Test]
		public void Test_Bank_NotValidInputs_Trades_OK()
		{
			_bank.SetInputParametersAndTrades("04/23/2021", "2", _trade_f1);
			Assert.That(!_bank.HasValidInputs);
		}

		[Test]
		public void Test_Bank_ValidInputs_OK()
		{
			_bank.SetInputParametersAndTrades("04/23/2021", "13", _trade3);
			Assert.That(_bank.HasValidInputs);
		}

		[Test]
		public void Test_Bank_ValidateTransactions3_OK()
		{
			_bank.SetInputParametersAndTrades("12/12/2024", "13", _trade3);

			Rule r1 = new Rule(1, "EXPIRED", new List<RuleCriteria>());
			Rule r2 = new Rule(2, "HIGHRISK", new List<RuleCriteria>());
			Rule r3 = new Rule(3, "MEDIUMRISK", new List<RuleCriteria>());
			Rule r4 = new Rule(4, "NORISK", new List<RuleCriteria>());

			r1.Criterias.Add(new DaysToExpireRuleCriteria { DaysToExpire = 30 });
			r2.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
			r2.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PRIVATE" });
			r3.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 100000 });
			r3.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.LowerOrEqualTo, Capital = 30000000 });
			r4.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.LowerOrEqualTo, Capital = 100000 });
			r4.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "SMALL" });

			_bank.Configuration.Rules = new List<Rule>
			{
				r1,
				r2,
				r3,
				r4
			};
			_bank.Configuration.Save();

			var result = _bank.ValidateTransactions();
			Assert.That(result.SequenceEqual(_result3));
		}

		[Test]
		public void Test_Bank_ValidateTransactions3_LimitedListSize5_OK()
		{
			_bank.SetInputParametersAndTrades("12/12/2024", "5", _trade3);

			Rule r1 = new Rule(1, "EXPIRED", new List<RuleCriteria>());
			Rule r2 = new Rule(2, "HIGHRISK", new List<RuleCriteria>());
			Rule r3 = new Rule(3, "MEDIUMRISK", new List<RuleCriteria>());
			Rule r4 = new Rule(4, "NORISK", new List<RuleCriteria>());

			r1.Criterias.Add(new DaysToExpireRuleCriteria { DaysToExpire = 30 });
			r2.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
			r2.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PRIVATE" });
			r3.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 100000 });
			r3.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.LowerOrEqualTo, Capital = 30000000 });
			r4.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.LowerOrEqualTo, Capital = 100000 });
			r4.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "SMALL" });

			_bank.Configuration.Rules = new List<Rule>
			{
				r1,
				r2,
				r3,
				r4
			};
			_bank.Configuration.Save();

			var result = _bank.ValidateTransactions();
			Assert.That(result.Count == 5);
		}

		[Test]
		public void Test_Bank_ValidateOriginal_OK()
		{
			_bank.SetInputParametersAndTrades("12/11/2020", "4", _tradeOriginal);

			Rule r1 = new Rule(1, "EXPIRED", new List<RuleCriteria>());
			Rule r2 = new Rule(2, "HIGHRISK", new List<RuleCriteria>());
			Rule r3 = new Rule(3, "MEDIUMRISK", new List<RuleCriteria>());

			r1.Criterias.Add(new DaysToExpireRuleCriteria { DaysToExpire = 30 });
			r2.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
			r2.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PRIVATE" });
			r3.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
			r3.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PUBLIC" });

			_bank.Configuration.Rules = new List<Rule>
			{
				r1,
				r2,
				r3
			};
			_bank.Configuration.Save();

			var result = _bank.ValidateTransactions();
			Assert.That(result.SequenceEqual(_resultOrig));
		}
		[Test]
		public void Test_Bank_ValidateOriginal_Variant1_OK()
		{			
			_bank.SetInputParametersAndTrades("12/11/2020", "6", _tradeOriginal1);

			Rule r1 = new Rule(1, "EXPIRED", new List<RuleCriteria>());
			Rule r2 = new Rule(2, "HIGHRISK", new List<RuleCriteria>());
			Rule r3 = new Rule(3, "MEDIUMRISK", new List<RuleCriteria>());

			r1.Criterias.Add(new DaysToExpireRuleCriteria { DaysToExpire = 30 });
			r2.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
			r2.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PRIVATE" });
			r3.Criterias.Add(new CapitalRuleCriteria { CompareType = ComparisonType.GreaterOrEqualTo, Capital = 1000000 });
			r3.Criterias.Add(new SectorRuleCriteria { CompareType = ComparisonType.EqualTo, Sector = "PUBLIC" });

			_bank.Configuration.Rules = new List<Rule>
			{
				r1,
				r2,
				r3
			};
			_bank.Configuration.Save();

			var result = _bank.ValidateTransactions();
			Assert.That(result.SequenceEqual(_resultOrig1));
		}

		#endregion
	}
}