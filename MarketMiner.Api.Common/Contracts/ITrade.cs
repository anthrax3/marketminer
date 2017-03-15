namespace MarketMiner.Api.Common.Contracts
{
   public interface ITrade
   {
      long id { get; set; }
      int units { get; set; }
      string side { get; set; }
      string instrument { get; set; }
      string time { get; set; }
      double price { get; set; }
      double takeProfit { get; set; }
      double stopLoss { get; set; }
      //int trailingStop { get; set; }
      //double trailingAmount { get; set; }
   }
}
