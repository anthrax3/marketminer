namespace MarketMiner.Api.Common.Contracts
{
   public interface IPrice
   {
      string instrument { get; set; }
      string time { get; set; }
      double bid { get; set; }
      double ask { get; set; }
      string status { get; set; }
      EPriceState state { get; set; }

      void update(IPrice update);
   }

   public enum EPriceState
   {
       Default,
       Increasing,
       Decreasing
   };

   //public class Price
   //{
   //    string instrument { get; set; }
   //    string time { get; set; }
   //    double bid { get; set; }
   //    double ask { get; set; }
   //    string status { get; set; }
   //    PriceState state = PriceState.Default;

   //    public void update(IPrice update);
   //}
}
