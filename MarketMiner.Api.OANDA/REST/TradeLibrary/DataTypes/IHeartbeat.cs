using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes
{
    /// <summary>
    /// Heartbeats are written to the implementing stream to ensure the connection remains active
    /// For more information, visit: http://developer.oanda.com/rest-live/streaming/
    /// </summary>
    public interface IHeartbeat
    {
        bool IsHeartbeat();
    }
}
