using MarketMiner.Api.Common.Contracts;

namespace MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes
{
    public class Price : IPrice
    {
        //public enum State
        //{
        //    Default,
        //    Increasing,
        //    Decreasing
        //};

        public Price()
        {
            state = EPriceState.Default;
        }

        public string instrument { get; set; }
        public string time { get; set; }
        public double bid { get; set; }
        public double ask { get; set; }
        public string status { get; set; }
        public EPriceState state { get; set; }

        public void update(IPrice update)
        {
            if (this.bid > update.bid)
            {
                state = EPriceState.Decreasing;
            }
            else if (this.bid < update.bid)
            {
                state = EPriceState.Increasing;
            }
            else
            {
                state = EPriceState.Default;
            }

            this.bid = update.bid;
            this.ask = update.ask;
            this.time = update.time;
        }
    }
}
