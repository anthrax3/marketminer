using MarketMiner.Api.Client.OANDA.Data.DataModels;

namespace MarketMiner.Api.Client.OANDA.ViewModels
{
    class TradeRequestViewModel
    {
        public string Instrument { get; set; }

        public int Units { get; set; }

        public EOrderType Type { get; set; }

        public EDirection Direction { get; set; }

        // value
        public double Price { get; set; }
        public double LowPrice { get; set; }
        public double HighPrice { get; set; }
        public double StopLoss { get; set; }
        public double TakeProfit { get; set; }
        
        // in pippettes
        public int TrailingStop { get; set; }
    }
}
