using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MarketMiner.Api.OANDA.REST.TradeLibrary
{
   public class EventsSession : StreamSession<Event>
   {
      public EventsSession(int accountId) : base(accountId)
      {
      }

      protected override async Task<WebResponse> GetSession()
      {
         return await Rest.StartEventsSession(new List<int> {_accountId});
      }
   }
}
