using System.Collections.Generic;

namespace MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes.Communications
{
   public class TransactionsResponse : Response
    {
        public List<Transaction> transactions;
        public string nextPage;
    }
}
