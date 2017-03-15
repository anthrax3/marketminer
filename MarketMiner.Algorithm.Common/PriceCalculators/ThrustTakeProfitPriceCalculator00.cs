using MarketMiner.Algorithm.Common.Contracts;
using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using MACC = MarketMiner.Api.Client.Common;

namespace MarketMiner.Algorithm.Common.PriceCalculators
{
   [Export("ThrustTakeProfitPriceCalculator", typeof(ITakeProfitPriceCalculator))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class ThrustTakeProfitPriceCalculator00 : ITakeProfitPriceCalculator
   {
      public virtual double? GetEntryTakeProfitPrice(Chart chart, IParameters parameters, IDictionary<string, object> state)
      {
         double? takeProfitPrice = null;

         return takeProfitPrice;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="chart"></param>
      /// <param name="retracement"></param>
      /// <param name="side"></param>
      /// <returns></returns>
      public virtual double? GetAdjustedTakeProfitPrice(Chart chart, IParameters parameters, IDictionary<string, object> state)
      {
         string side;
         FibonacciRetracement retracement;

         #region state validation
         string stateItem = "";
         try
         {
            stateItem = "side";
            side = (string)state[stateItem];
            stateItem = "retracement";
            retracement = (FibonacciRetracement)state[stateItem];
         }
         catch (Exception ex)
         {
            throw new ArgumentException(string.Format("State argument is missing or invalid: {0}.", stateItem), ex);
         }
         #endregion

         #region logic
         // if a buy ...
         //    move the profit target to 1 or 2 pips under .618 * (thrust.FocusPrice - [pattern lowMid price set by the post-fill price action])
         // if a sell ...
         //    move the profit target to 1 or 2 pips above .618 * ([pattern lowMid price set by the post-fill price action] - thrust.FocusPrice)
         #endregion

         if (!retracement.ExtremaPrice.HasValue)
            return null;

         double extremaPrice = retracement.ExtremaPrice.Value;

         double fibRatio = MACC.Constants.FibonacciRetracementLevel.R618;
         double takeProfitPrice, extremaPlusDisplacement = 0;

         if (side == MACC.Constants.SignalSide.Buy)
         {
            extremaPlusDisplacement = fibRatio * (retracement.FocusPrice - extremaPrice);

            // midpoint prices are in use so this ask price is already (0.5*spread) above the fib level bid
            // let's lower it by (1.0*spread) to get it (0.5*spread) below the fib level bid
            takeProfitPrice = extremaPrice + extremaPlusDisplacement - (chart.HistoricBidAskSpread);
         }
         else
         {
            extremaPlusDisplacement = fibRatio * (extremaPrice - retracement.FocusPrice);

            // midpoint prices are in use so this bid price is already (0.5*spread) above the fib level bid
            takeProfitPrice = extremaPrice - extremaPlusDisplacement;
         }

         takeProfitPrice = Math.Round(takeProfitPrice, retracement.LevelPlaces());

         return takeProfitPrice;
      }
   }
}
