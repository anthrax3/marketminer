using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMiner.Api.OANDA.REST20.TradeLibrary.DataTypes
{
   public class Account
   {
      public bool HasAccountId;
      private int _id;
      public int id
      {
         get { return _id; }
         set
         {
            _id = value;
            HasAccountId = true;
         }
      }

      public bool HasAlias;
      private string _alias;
      public string alias
      {
         get { return _alias; }
         set
         {
            _alias = value;
            HasAlias = true;
         }
      }

      public bool HasCurrency;
      private string _currency;
      public string currency
      {
         get { return _currency; }
         set
         {
            _currency = value;
            HasCurrency = true;
         }
      }

      public bool HasBalance;
      private string _balance;
      public string balance
      {
         get { return _balance; }
         set
         {
            _balance = value;
            HasBalance = true;
         }
      }

      public bool HasCreatedByUserId;
      private string _createdByUserId;
      public string createdByUserId
      {
         get { return _createdByUserId; }
         set
         {
            _createdByUserId = value;
            HasCreatedByUserId = true;
         }
      }

      public bool HasCreatedTime;
      private string _createdTime;
      public string createdTime
      {
         get { return _createdTime; }
         set
         {
            _createdTime = value;
            HasCreatedTime = true;
         }
      }

      public bool HasPL;
      private string _pl;
      public string pl
      {
         get { return _pl; }
         set
         {
            _pl = value;
            HasPL = true;
         }
      }

      public bool HasResettablePL;
      private string _resettablePL;
      public string resettablePL
      {
         get { return _resettablePL; }
         set
         {
            _resettablePL = value;
            HasResettablePL = true;
         }
      }

      public bool HasResettablePLTime;
      private string _resettablePLTime;
      public string resettablePLTime
      {
         get { return _resettablePLTime; }
         set
         {
            _resettablePLTime = value;
            HasResettablePLTime = true;
         }
      }

      [IsOptional]
      public bool HasMarginRate;
      private string _marginRate;
      public string marginRate
      {
         get { return _marginRate; }
         set
         {
            _marginRate = value;
            HasMarginRate = true;
         }
      }

      [IsOptional]
      public bool HasMarginCallEnterTime;
      private string _marginCallEnterTime;
      public string marginCallEnterTime
      {
         get { return _marginCallEnterTime; }
         set
         {
            _marginCallEnterTime = value;
            HasMarginCallEnterTime = true;
         }
      }

      public bool HasMarginCallExtensionCount;
      private string _marginCallExtensionCount;
      public string marginCallExtensionCount
      {
         get { return _marginCallExtensionCount; }
         set
         {
            _marginCallExtensionCount = value;
            HasMarginCallExtensionCount = true;
         }
      }

      public bool HasLastMarginCallExtensionTime;
      private string _lastMarginCallExtensionTime;
      public string lastMarginCallExtensionTime
      {
         get { return _lastMarginCallExtensionTime; }
         set
         {
            _lastMarginCallExtensionTime = value;
            HasLastMarginCallExtensionTime = true;
         }
      }

      public bool HasOpenTradeCount;
      private string _openTradeCount;
      public string openTradeCount
      {
         get { return _openTradeCount; }
         set
         {
            _openTradeCount = value;
            HasOpenTradeCount = true;
         }
      }

      public bool HasOpenPositionCount;
      private string _openPositionCount;
      public string openPositionCount
      {
         get { return _openPositionCount; }
         set
         {
            _openPositionCount = value;
            HasOpenPositionCount = true;
         }
      }

      public bool HasPendingOrderCount;
      private string _pendingOrderCount;
      public string pendingOrderCount
      {
         get { return _pendingOrderCount; }
         set
         {
            _pendingOrderCount = value;
            HasPendingOrderCount = true;
         }
      }

      public bool HasHedgingEnabled;
      private string _hedgingEnabled;
      public string hedgingEnabled
      {
         get { return _hedgingEnabled; }
         set
         {
            _hedgingEnabled = value;
            HasHedgingEnabled = true;
         }
      }

      public bool HasUnrealizedPL;
      private string _unrealizedPL;
      public string unrealizedPL
      {
         get { return _unrealizedPL; }
         set
         {
            _unrealizedPL = value;
            HasUnrealizedPL = true;
         }
      }

      public bool HasNAV;
      private string _NAV;
      public string NAV
      {
         get { return _NAV; }
         set
         {
            _NAV = value;
            HasNAV = true;
         }
      }

      public bool HasMarginUsed;
      private string _marginUsed;
      public string marginUsed
      {
         get { return _marginUsed; }
         set
         {
            _marginUsed = value;
            HasMarginUsed = true;
         }
      }














      [IsOptional]
      public bool HasRealizedPl;
      private string _realizedPl;
      public string realizedPl
      {
         get { return _realizedPl; }
         set
         {
            _realizedPl = value;
            HasRealizedPl = true;
         }
      }



      [IsOptional]
      public bool HasMarginAvail;
      private string _marginAvail;
      public string marginAvail
      {
         get { return _marginAvail; }
         set
         {
            _marginAvail = value;
            HasMarginAvail = true;
         }
      }

      [IsOptional]
      public bool HasOpenTrades;
      private string _openTrades;
      public string openTrades
      {
         get { return _openTrades; }
         set
         {
            _openTrades = value;
            HasOpenTrades = true;
         }
      }

      [IsOptional]
      public bool HasOpenOrders;
      private string _openOrders;
      public string openOrders
      {
         get { return _openOrders; }
         set
         {
            _openOrders = value;
            HasOpenOrders = true;
         }
      }
   }
}
