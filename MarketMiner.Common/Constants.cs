using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMiner.Common
{
   public class Constants
   {
      public class MetaSettings
      {
         public class Environments
         {
            public const string Default = "DEFAULT";
            public const string Development = "DEVELOPMENT";
            public const string Production = "PRODUCTION";
         }

         public class Types
         {
            public const string Api = "API";
            public const string Authentication = "AUTHENTICATION";
            public const string Communication = "COMMUNICATION";
            public const string Risk = "RISK";
         }

         public class Codes
         {
            // communication
            public const string EmailSender = "EmailSender";

            // api
            public const string OANDAEnvironment = "OANDA_Environment";
            public const string OANDAPracticeToken = "OANDA_Practice_Token";
            public const string OANDAPracticeAccount = "OANDA_Practice_Account";
            public const string OANDATradeToken = "OANDA_Trade_Token";
            public const string OANDATradeAccount = "OANDA_Trade_Account";       
            public const string SendGridApiKey = "SENDGRID_ApiKey";
         }
      }

      public class Brokers
      {
         public const string ETrade = "ETrade";
         public const string InteractiveBrokers = "InteractiveBrokers";
         public const string OANDA = "OANDA";
         public const string TradeStation = "TradeStation";
      }
   }
}
