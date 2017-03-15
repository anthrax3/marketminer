namespace MarketMiner.Api.Common.Contracts
{
   public interface IOrder
   {
      long id { get; set; }
      string instrument { get; set; }
      int units { get; set; }
      string side { get; set; }
      string type { get; set; }
      string time { get; set; }
      double price { get; set; }
      double takeProfit { get; set; }
      double stopLoss { get; set; }
      string expiry { get; set; }
      //double upperBound { get; set; }
      //double lowerBound { get; set; }
      int trailingStop { get; set; }
   }
}
