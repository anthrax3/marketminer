using MarketMiner.Algorithm.Common.Contracts;
using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.Common.Contracts;
using MarketMiner.Api.Client.Common.Patterns.Dinapoli;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using MACC = MarketMiner.Api.Client.Common;

namespace MarketMiner.Algorithm.Common.PriceCalculators
{
   [Export("ThrustStopLossPriceCalculator", typeof(IStopLossPriceCalculator))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class ThrustStopLossPriceCalculator00 : IStopLossPriceCalculator
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="chart"></param>
      /// <param name="side"></param>
      /// <param name="state"></param>
      /// <returns></returns>
      public virtual double? GetEntryStopLossPrice(Chart chart, IParameters parameters, IDictionary<string, object> state)
      {
         double? stopLossPrice = null;

         return stopLossPrice;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="chart"></param>
      /// <param name="side"></param>
      /// <param name="state"></param>
      /// <returns></returns>
      public virtual double? GetAdjustedStopLossPrice(Chart chart, IParameters parameters, IDictionary<string, object> state)
      {
         string side;
         Thrust thrust;
         bool hasProfit;
      
         #region state validation
         string stateItem = "";
         try
         {
            stateItem = "side";
            side = (string)state[stateItem];
            stateItem = "thrust";
            thrust = (Thrust)state[stateItem];
            stateItem = "hasProfit";
            hasProfit = Convert.ToBoolean(state[stateItem]);
         }
         catch (Exception ex)
         {
            throw new ArgumentException(string.Format("State argument is missing or invalid: {0}.", stateItem), ex);
         }
         #endregion

         #region logic
         // if a buy ...
         //    move the stop to the lower of the .500 fib price or [patern lowBid price set by the post-fill price action]
         // if a sell ...
         //    move the stop to the higher of the .500 fib price or [patern highBid price set by the post-fill price action]
         #endregion

         FibonacciRetracement retracement = (FibonacciRetracement)thrust.Study;

         if (!retracement.ExtremaPrice.HasValue)
            return null;

         double? stopLossPrice = null;
         double r500Price = retracement.LevelPrice(FibonacciLevel.R500);
         double lastPrice = chart.Frames.Last().Bar.closeMid;

         double extrema500Delta = 0;
         double extremaPrice = retracement.ExtremaPrice.Value;
         double extremaTakeProfitDelta = Math.Abs(thrust.TakeProfitPrice.Value - extremaPrice);
         double takeProfitZoneReachedExtremaCoefficient = parameters.GetDouble("takeProfitZoneReachedExtremaCoefficient") ?? 0.5;

         if (side == MACC.Constants.SignalSide.Buy)
         {
            extrema500Delta = extremaPrice - r500Price;

            if (hasProfit)
            {
               if (thrust.TakeProfitZoneReached())
                  stopLossPrice = extremaPrice + (takeProfitZoneReachedExtremaCoefficient * extremaTakeProfitDelta);
               else if (extrema500Delta >= 0 && !thrust.ProfitWindowClosed)
                  stopLossPrice = null;
               else if (extrema500Delta >= 0 && thrust.ProfitWindowClosed) // set to extrema or 500?
                  stopLossPrice = r500Price;
               else if (extrema500Delta < 0 && !thrust.ProfitWindowClosed)
                  stopLossPrice = extremaPrice;
               else // extrema500Delta < 0 && profitWindowClosed .. set to r500
                  stopLossPrice = r500Price;
            }
            else
            {
               if (thrust.TakeProfitZoneReached())
                  stopLossPrice = -1;
               if (thrust.ProfitWindowClosed && lastPrice > r500Price)
                  stopLossPrice = r500Price;
               else if (thrust.ProfitWindowClosed && lastPrice <= r500Price)
                  stopLossPrice = -1; // kill trade
               else
                  stopLossPrice = null;
            }

            if (stopLossPrice.HasValue)
            {
               // stopLoss should never move down
               if (stopLossPrice <= thrust.StopLossPrice)
                  stopLossPrice = null;
               else
                  stopLossPrice -= chart.HistoricBidAskSpread;
            }
         }
         else
         {
            extrema500Delta = r500Price - extremaPrice;

            if (hasProfit)
            {
               if (thrust.TakeProfitZoneReached())
                  stopLossPrice = extremaPrice - (takeProfitZoneReachedExtremaCoefficient * extremaTakeProfitDelta);
               else if (extrema500Delta >= 0 && !thrust.ProfitWindowClosed)
                  stopLossPrice = null;
               else if (extrema500Delta >= 0 && thrust.ProfitWindowClosed)
                  stopLossPrice = extremaPrice;
               else if (extrema500Delta < 0 && !thrust.ProfitWindowClosed)
                  stopLossPrice = extremaPrice;
               else // extrema500Delta < 0 && profitWindowClosed
                  stopLossPrice = r500Price;
            }
            else
            {
               if (thrust.TakeProfitZoneReached())
                  stopLossPrice = -1;
               else if (lastPrice < r500Price && thrust.ProfitWindowClosed)
                  stopLossPrice = r500Price;
               else if (lastPrice >= r500Price && thrust.ProfitWindowClosed)
                  stopLossPrice = -1; // kill trade .. if not already stopped out
               else
                  stopLossPrice = null;
            }

            if (stopLossPrice.HasValue)
            {
               // stopLoss should never move up
               if (stopLossPrice >= thrust.StopLossPrice)
                  stopLossPrice = null;
               else
                  stopLossPrice += chart.HistoricBidAskSpread;
            }
         }

         if (stopLossPrice.HasValue)
            stopLossPrice = Math.Round(stopLossPrice.Value, retracement.LevelPlaces());

         return stopLossPrice;
      }
   }
}
