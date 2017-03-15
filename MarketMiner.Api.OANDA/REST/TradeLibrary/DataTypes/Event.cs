using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes
{
    /// <summary>
    /// Events are authorized transactions posted to the subject account.
    /// For more information, visit: http://developer.oanda.com/rest-live/streaming/
    /// </summary>
    public class Event : IHeartbeat
    {
        public Heartbeat heartbeat { get; set; }
        public Transaction transaction { get; set; }
        public bool IsHeartbeat()
        {
            return (heartbeat != null);
        }
    }
}
