using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using P.Core.Common.Utils;

namespace MarketMiner.Api.OANDA.Extensions.Classes
{
   public class MarketMinerAccount : Account
   {
      public MarketMinerAccount(Account account)
      {
         this.InjectWith(account);
      }

      public bool HasAccountId() { return base.HasAccountId && base.accountId > 0; }
      public bool HasAccountName() { return base.HasAccountName && !string.IsNullOrEmpty(base.accountName); }
      public bool HasAccountCurrency() { return base.HasAccountCurrency && !string.IsNullOrEmpty(base.accountCurrency); }
      public bool HasMarginRate() { return base.HasMarginRate && base.marginRate != "1.00"; }
      public bool HasBalance() { return base.HasBalance && base.balance != "0.00"; }
      public bool HasUnrealizedPl() { return base.HasUnrealizedPl && base.unrealizedPl != "0.00"; }
      public bool HasRealizedPl() { return base.HasRealizedPl && base.realizedPl != "0.00"; }
      public bool HasMarginUsed() { return base.HasMarginUsed && base.marginUsed != "0.00"; }
      public bool HasMarginAvail() { return base.HasMarginAvail && base.marginAvail != "0.00"; }
      public bool HasOpenTrades() { return base.HasOpenTrades && base.openTrades != "0"; }
      public bool HasOpenOrders() { return base.HasOpenOrders && base.openOrders != "0"; }
   }
}
