using System.Collections.Generic;

namespace MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes.Communications
{
    public class OrdersResponse
    {
        public List<Order> orders;
        public string nextPage;
    }
}
