using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MarketMiner.Api.OANDA.REST.TradeLibrary
{
   public class RatesSession : StreamSession<RateStreamResponse>
   {
      private readonly List<Instrument> _instruments;

      public RatesSession(int accountId, List<Instrument> instruments) : base(accountId)
      {
         _instruments = instruments;
      }

      protected override async Task<WebResponse> GetSession()
      {
         return await Rest.StartRatesSession(_instruments, _accountId);
      }
   }
}
