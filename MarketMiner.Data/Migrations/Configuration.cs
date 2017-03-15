namespace MarketMiner.Data.Migrations
{
   using MarketMiner.Business.Entities;
   using MarketMiner.Common;
   using MarketMiner.Data;
   using P.Core.Common.Meta;
   using System;
   using System.Collections.Generic;
   using System.Data.Entity.Infrastructure;
   using System.Data.Entity.Migrations;
   using System.Linq;
   using System.Web.Security;
   using WebMatrix.WebData;

   internal sealed class Configuration : DbMigrationsConfiguration<MarketMinerContext>
   {
      public Configuration()
      {
         AutomaticMigrationsEnabled = false;
      }

      #region Methods
      /// <summary>
      /// Populates the database with seed data
      /// </summary>
      /// <param name="context"></param>
      protected override void Seed(MarketMinerContext context)
      {
         SeedMembership();

         // refresh Account entities
         ((IObjectContextAdapter)context).ObjectContext.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, context.AccountSet);

         #region products
         context.ProductSet.AddOrUpdate(x => x.Name,
         #region fx products
 new Product() { Name = "Signal.AUDJPY.THRUST.M30", ShortDesc = "AUDJPY M30 Thrust Sgl", LongDesc = "AUDJPY 30 Minute Thrust Signal" },
            // AUDUSD
            new Product() { Name = "Signal.AUDUSD.THRUST.M5", ShortDesc = "AUDUSD M5 Thrust Sgl", LongDesc = "AUDUSD 5 Minute Thrust Signal" },
            new Product() { Name = "Signal.AUDUSD.THRUST.M15", ShortDesc = "AUDUSD M15 Thrust Sgl", LongDesc = "AUDUSD 15 Minute Thrust Signal" },
            new Product() { Name = "Signal.AUDUSD.THRUST.M30", ShortDesc = "AUDUSD M30 Thrust Sgl", LongDesc = "AUDUSD 30 Minute Thrust Signal" },
            new Product() { Name = "Signal.AUDUSD.THRUST.H1", ShortDesc = "AUDUSD H Thrust Sgl", LongDesc = "AUDUSD Hourly Thrust Signal" },
            new Product() { Name = "Signal.AUDUSD.THRUST.D", ShortDesc = "AUDUSD D Thrust Sgl", LongDesc = "AUDUSD Daily Thrust Signal" },
            new Product() { Name = "Signal.AUDUSD.THRUST.W", ShortDesc = "AUDUSD W Thrust Sgl", LongDesc = "AUDUSD Weekly Thrust Signal" },
            // EURUSD
            new Product() { Name = "Signal.EURUSD.THRUST.M5", ShortDesc = "EURUSD M5 Thrust Sgl", LongDesc = "EURUSD 5 Minute Signal" },
            new Product() { Name = "Signal.EURUSD.THRUST.M15", ShortDesc = "EURUSD M15 Thrust Sgl", LongDesc = "EURUSD 15 Minute Signal" },
            new Product() { Name = "Signal.EURUSD.THRUST.M30", ShortDesc = "EURUSD M30 Thrust Sgl", LongDesc = "EURUSD 30 Minute Thrust Signal" },
            new Product() { Name = "Signal.EURUSD.THRUST.H1", ShortDesc = "EURUSD H Thrust Sgl", LongDesc = "EURUSD Hourly Thrust Signal" },
            new Product() { Name = "Signal.EURUSD.THRUST.D", ShortDesc = "EURUSD D Thrust Sgl", LongDesc = "EURUSD Daily Thrust Signal" },
            // GBPUSD
            new Product() { Name = "Signal.GBPUSD.THRUST.M15", ShortDesc = "GBPUSD M15 Thrust Sgl", LongDesc = "GBPUSD 15 Minute Thrust Signal" },
            new Product() { Name = "Signal.GBPUSD.THRUST.H1", ShortDesc = "GBPUSD H Thrust Sgl", LongDesc = "GBPUSD Hourly Thrust Signal" },
            new Product() { Name = "Signal.GBPUSD.THRUST.D", ShortDesc = "GBPUSD D Thrust Sgl", LongDesc = "GBPUSD Daily Thrust Signal" },
            new Product() { Name = "Signal.GBPUSD.THRUST.W", ShortDesc = "GBPUSD W Thrust Sgl", LongDesc = "GBPUSD Weekly Thrust Signal" },
            new Product() { Name = "Signal.GBPUSD.THRUST.M", ShortDesc = "GBPUSD M Thrust Sgl", LongDesc = "GBPUSD Monthly Thrust Signal" },
            // NZDUSD
            new Product() { Name = "Signal.NZDUSD.THRUST.M15", ShortDesc = "NZDUSD M15 Thrust Sgl", LongDesc = "NZDUSD 15 Minute Thrust Signal" },
            new Product() { Name = "Signal.NZDUSD.THRUST.H1", ShortDesc = "NZDUSD H Thrust Sgl", LongDesc = "NZDUSD Hourly Thrust Signal" },
            new Product() { Name = "Signal.NZDUSD.THRUST.D", ShortDesc = "NZDUSD D Thrust Sgl", LongDesc = "NZDUSD Daily Thrust Signal" },
            new Product() { Name = "Signal.NZDUSD.THRUST.W", ShortDesc = "NZDUSD W Thrust Sgl", LongDesc = "NZDUSD Weekly Thrust Signal" },
            new Product() { Name = "Signal.NZDUSD.THRUST.M", ShortDesc = "NZDUSD M Thrust Sgl", LongDesc = "NZDUSD Monthly Thrust Signal" },
            // USDCAD
            new Product() { Name = "Signal.USDCAD.THRUST.M5", ShortDesc = "USDCAD M5 Thrust Sgl", LongDesc = "USDCAD 5 Minute Thrust Signal" },
            new Product() { Name = "Signal.USDCAD.THRUST.M15", ShortDesc = "USDCAD M15 Thrust Sgl", LongDesc = "USDCAD 15 Minute Thrust Signal" },
            new Product() { Name = "Signal.USDCAD.THRUST.M30", ShortDesc = "USDCAD M30 Thrust Sgl", LongDesc = "USDCAD 30 Minute Thrust Signal" },
            // USDCHF
            new Product() { Name = "Signal.USDCHF.THRUST.M15", ShortDesc = "USDCHF M15 Thrust Sgl", LongDesc = "USDCHF 15 Minute Thrust Signal" },
            // USDJPY
            new Product() { Name = "Signal.USDJPY.THRUST.M5", ShortDesc = "USDJPY M5 Thrust Sgl", LongDesc = "USDJPY 5 Minute Thrust Signal" },
            new Product() { Name = "Signal.USDJPY.THRUST.M15", ShortDesc = "USDJPY M15 Thrust Sgl", LongDesc = "USDJPY 15 Minute Thrust Signal" },
            new Product() { Name = "Signal.USDJPY.THRUST.M30", ShortDesc = "USDJPY M30 Thrust Sgl", LongDesc = "USDJPY 30 Minute Thrust Signal" },
            new Product() { Name = "Signal.USDJPY.THRUST.H1", ShortDesc = "USDJPY H Thrust Sgl", LongDesc = "USDJPY Hourly Thrust Signal" },
            new Product() { Name = "Signal.USDJPY.THRUST.D", ShortDesc = "USDJPY D Thrust Sgl", LongDesc = "USDJPY Daily Thrust Signal" },
            new Product() { Name = "Signal.USDJPY.THRUST.W", ShortDesc = "USDJPY W Thrust Sgl", LongDesc = "USDJPY Weekly Thrust Signal" },
            new Product() { Name = "Signal.USDJPY.THRUST.M", ShortDesc = "USDJPY M Thrust Sgl", LongDesc = "USDJPY Monthly Thrust Signal" }
         #endregion

         #region equities
         #endregion

         #region futures
         #endregion
);
         context.SaveChanges();
         context.ProductSet.ToList().ForEach(p => p.Active = true);
         context.SaveChanges();
         #endregion

         #region subscriptions
         int ocoAccountID = context.AccountSet.First(x => x.LoginEmail == "mrchrisok@hotmail.com").AccountID;
         List<Subscription> ocoSubscriptions = new List<Subscription>();
         foreach (var product in context.ProductSet)
         {
            ocoSubscriptions.Add(new Subscription()
            {
               AccountID = ocoAccountID,
               ProductID = product.ProductID,
               Active = true
            });
         };
         context.SubscriptionSet.AddOrUpdate(x => new { x.AccountID, x.ProductID }, ocoSubscriptions.ToArray());
         context.SaveChanges();
         #endregion

         #region funds
         context.FundSet.AddOrUpdate(x => x.Name,
            new Fund()
            {
               Name = "MarketMiner.FX.Alpha",
               OpenDate = new DateTime(2016, 8, 1).ToUniversalTime(),
               OpenToNew = true,
               OpenToAdd = true,
               OpenToRedeem = false
            }
         );
         context.SaveChanges();
         #endregion

         #region strategies
         context.StrategySet.AddOrUpdate(x => x.Name,
            new Strategy()
            {
               Name = "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01",
               FundID = context.FundSet.First(f => f.Name == "MarketMiner.FX.Alpha").FundID,
               ShortDesc = "Identify and trade single entries Dinapoli thrust signals discovered on DM FX pairs.",
               LongDesc = "Identify and trade single entries Dinapoli thrust signals discovered on DM FX pairs."
            },
            new Strategy()
            {
               Name = "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter02",
               FundID = context.FundSet.First(f => f.Name == "MarketMiner.FX.Alpha").FundID,
               ShortDesc = "Identify and trade wedge entries Dinapoli thrust signals discovered on DM FX pairs.",
               LongDesc = "Identify and trade wedge entries Dinapoli thrust signals discovered on DM FX pairs."
            },
            new Strategy()
            {
               Name = "MarketMiner.Algorithm.OANDA.Patterns.PriceGap01",
               FundID = context.FundSet.First(f => f.Name == "MarketMiner.FX.Alpha").FundID,
               ShortDesc = "Identify and trade inter-session FX pair price gaps.",
               LongDesc = "Identify and trade inter-session FX pair price gaps."
            }
         );
         context.SaveChanges();
         #endregion

         #region parameters
         context.AlgorithmParameterSet.AddOrUpdate(x => new { x.StrategyID, x.ParameterName },

            #region MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "acrossframescoefficient",
               ParameterType = "double",
               ParameterValue = "0.382"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "focusbreakoutcoefficient",
               ParameterType = "double",
               ParameterValue = "0.25"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "minthrustpercentrange",
               ParameterType = "double",
               ParameterValue = "0.003"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "minreactiontofocusspan",
               ParameterType = "short",
               ParameterValue = "8"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "mintradevalueatrisk",
               ParameterType = "double",
               ParameterValue = "0.01"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "maxtradevalueatrisk",
               ParameterType = "double",
               ParameterValue = "0.02"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "minsequentialpositive3x3count",
               ParameterType = "short",
               ParameterValue = "6"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "maxsequentialnegative3x3count",
               ParameterType = "short",
               ParameterValue = "3"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "leadersidescount",
               ParameterType = "integer",
               ParameterValue = "3"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "chartrefreshmilliseconds",
               ParameterType = "integer",
               ParameterValue = "60000"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "chartinitialframes",
               ParameterType = "short",
               ParameterValue = "200"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "chartmaximumframes",
               ParameterType = "integer",
               ParameterValue = "500"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "profitwaitpreriods",
               ParameterType = "short",
               ParameterValue = "6"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "pricespreadcoefficient",
               ParameterType = "double",
               ParameterValue = "0.75"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "thrustfillzonereachedcoefficient",
               ParameterType = "double",
               ParameterValue = "0.90"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "takeprofitzonereachedextremacoefficient",
               ParameterType = "double",
               ParameterValue = "0.5"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "thrusttakeprofitzonereachedcoefficient",
               ParameterType = "double",
               ParameterValue = "0.90"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "maxafterfocusacross3x3count",
               ParameterType = "short",
               ParameterValue = "3"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.BreadAndButter01").StrategyID,
               ParameterName = "instrumentgranularities",
               ParameterType = "string",
               ParameterValue = "USD_JPY:M15,USD_CAD:M15,EUR_USD:M15,GBP_USD:M15,AUD_USD:M15,NZD_USD:M15"
            },
            #endregion

            #region MarketMiner.Algorithm.OANDA.Patterns.PriceGap01
 new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.PriceGap01").StrategyID,
               ParameterName = "mintradevalueatrisk",
               ParameterType = "double",
               ParameterValue = "0.01"
            },
            new AlgorithmParameter()
            {
               StrategyID = context.StrategySet.First(x => x.Name == "MarketMiner.Algorithm.OANDA.Patterns.PriceGap01").StrategyID,
               ParameterName = "maxtradevalueatrisk",
               ParameterType = "double",
               ParameterValue = "0.02"
            }
         #endregion
);
         context.SaveChanges();
         #endregion

         #region settings
         context.MetaSettingSet.AddOrUpdate(x => new { x.Environment, x.Type, x.Code },
            new MetaSetting()
            {
               Environment = Constants.MetaSettings.Environments.Default,
               Type = Constants.MetaSettings.Types.Risk,
               Code = "StrategyMaximumRiskPercentPerPosition",
               Value = "2.00",
               Enabled = true
            },
            new MetaSetting()
            {
               Environment = Constants.MetaSettings.Environments.Default,
               Type = Constants.MetaSettings.Types.Risk,
               Code = "StrategyMinimumRiskPercentPerPosition",
               Value = "1.00",
               Enabled = true
            },

            #region oanda
            new MetaSetting()
            {
               Environment = Constants.MetaSettings.Environments.Development,
               Type = Constants.MetaSettings.Types.Api,
               Code = Constants.MetaSettings.Codes.OANDAEnvironment,
               Value = "Trade",
               Enabled = true
            },
            new MetaSetting()
            {
               Environment = Constants.MetaSettings.Environments.Development,
               Type = Constants.MetaSettings.Types.Api,
               Code = Constants.MetaSettings.Codes.OANDATradeAccount,
               Value = "781925",
               Enabled = true
            },
            new MetaSetting()
            {
               Environment = Constants.MetaSettings.Environments.Development,
               Type = Constants.MetaSettings.Types.Api,
               Code = Constants.MetaSettings.Codes.OANDATradeToken,
               Value = "15819c438789d8c3954633bc7adb926c-9e21c808b7d7db2a2a387378c652ba73",
               Enabled = true
            },
            new MetaSetting()
            {
               Environment = Constants.MetaSettings.Environments.Development,
               Type = Constants.MetaSettings.Types.Api,
               Code = "OANDA_Trade_Algorithm01_Account",
               Value = "781925",
               Enabled = true
            },

            new MetaSetting()
            {
               Environment = Constants.MetaSettings.Environments.Default,
               Type = Constants.MetaSettings.Types.Api,
               Code = Constants.MetaSettings.Codes.OANDAEnvironment,
               Value = "Practice",
               Enabled = true
            },
            new MetaSetting()
            {
               Environment = Constants.MetaSettings.Environments.Default,
               Type = Constants.MetaSettings.Types.Api,
               Code = Constants.MetaSettings.Codes.OANDAPracticeAccount,
               Value = "9586325",
               Enabled = true
            },
            new MetaSetting()
            {
               Environment = Constants.MetaSettings.Environments.Default,
               Type = Constants.MetaSettings.Types.Api,
               Code = Constants.MetaSettings.Codes.OANDAPracticeToken,
               Value = "251d3bc72c70e0e4c673566b17a69b36-f79a5e3ffa3d2aadd82d0e25a31284e5",
               Enabled = true
            },
            #endregion

            #region email
            new MetaSetting()
            {
               Environment = Constants.MetaSettings.Environments.Default,
               Type = Constants.MetaSettings.Types.Communication,
               Code = Constants.MetaSettings.Codes.EmailSender,
               Value = "MarketMiner.Business.Common.OutlookMailer, MarketMiner.Business",
               Enabled = false
            },
            new MetaSetting()
            {
               Environment = Constants.MetaSettings.Environments.Development,
               Type = Constants.MetaSettings.Types.Communication,
               Code = Constants.MetaSettings.Codes.EmailSender,
               Value = "MarketMiner.Business.Common.SendGridMailer, MarketMiner.Business",
               Enabled = true
            },
            new MetaSetting()
            {
               Environment = Constants.MetaSettings.Environments.Default,
               Type = Constants.MetaSettings.Types.Api,
               Code = Constants.MetaSettings.Codes.SendGridApiKey,
               Value = "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",
               Enabled = true
            }
            #endregion
            );
         context.SaveChanges();
         #endregion

         #region brokers
         context.BrokerSet.AddOrUpdate(x => x.Name,
            new Broker() { Name = "OANDA" },
            new Broker() { Name = "ETrade" },
            new Broker() { Name = "InteractiveBrokers" }
         );
         context.SaveChanges();
         #endregion

      }
      /// <summary>
      /// Populates the database with seed membership data
      /// </summary>
      private void SeedMembership()
      {
         if (!WebSecurity.Initialized)
         {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Account", "AccountID", "LoginEmail", autoCreateTables: true);
         }

         var roles = (SimpleRoleProvider)Roles.Provider;
         var membership = (SimpleMembershipProvider)Membership.Provider;

         if (!roles.RoleExists(OCOApp.Security.Admin))
         {
            roles.CreateRole(OCOApp.Security.Admin);
         }

         if (!roles.RoleExists(OCOApp.Security.User))
         {
            roles.CreateRole(OCOApp.Security.User);
         }

         if (membership.GetUser("mrchrisok@hotmail.com", false) == null)
         {
            WebSecurity.CreateUserAndAccount("mrchrisok@hotmail.com", "Mocha4578",
               new
               {
                  FirstName = "Chris",
                  LastName = "Okonkwo",
                  Address = "9637 Timber Wagon Drive",
                  City = "Mckinney",
                  State = "TX",
                  ZipCode = "75070",
                  CreditCard = "1111222233334444",
                  ExpDate = new DateTime(2017, 1, 31).ToUniversalTime(),
                  DateCreated = DateTime.UtcNow,
                  DateModified = DateTime.UtcNow
               });
         }

         if (!roles.GetRolesForUser("mrchrisok@hotmail.com").Contains(OCOApp.Security.Admin))
         {
            roles.AddUsersToRoles(new[] { "mrchrisok@hotmail.com" }, new[] { OCOApp.Security.Admin });
         }

         if (!roles.GetRolesForUser("mrchrisok@hotmail.com").Contains(OCOApp.Security.User))
         {
            roles.AddUsersToRoles(new[] { "mrchrisok@hotmail.com" }, new[] { OCOApp.Security.User });
         }
      }
      #endregion
   }
}
