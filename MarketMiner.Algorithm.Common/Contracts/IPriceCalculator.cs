using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.Common.Contracts;
using System.Collections.Generic;

namespace MarketMiner.Algorithm.Common.Contracts
{
   public interface IPriceCalculator
   {
      //
   }

   public interface ITakeProfitPriceCalculator : IPriceCalculator
   {
      double? GetEntryTakeProfitPrice(Chart chart, IParameters parameters, IDictionary<string, object> state);
      double? GetAdjustedTakeProfitPrice(Chart chart, IParameters parameters, IDictionary<string, object> state);
   }

   public interface IStopLossPriceCalculator : IPriceCalculator
   {
      double? GetEntryStopLossPrice(Chart chart, IParameters parameters, IDictionary<string, object> state);
      double? GetAdjustedStopLossPrice(Chart chart, IParameters parameters, IDictionary<string, object> state);
   }
}