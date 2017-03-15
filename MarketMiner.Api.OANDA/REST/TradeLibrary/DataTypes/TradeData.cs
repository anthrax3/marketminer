using MarketMiner.Api.Common.Contracts;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes.Communications;

namespace MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes
{
    public class TradeData : Response, ITrade
    {
        public long id { get; set; }
        public int units { get; set; }
        public string side { get; set; }
        public string instrument { get; set; }
        public string time { get; set; }
        public double price { get; set; }
        public double takeProfit { get; set; }
        public double stopLoss { get; set; }
        public int trailingStop { get; set; }
        public double trailingAmount { get; set; }
    }
}
