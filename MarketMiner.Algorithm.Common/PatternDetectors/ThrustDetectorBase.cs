using MarketMiner.Algorithm.Common.PatternDetectors;
using MarketMiner.Api.Client.Common;
using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.Common.Contracts;
using MarketMiner.Api.Client.Common.Patterns.Dinapoli;
using MarketMiner.Api.Common.Contracts;
using MarketMiner.Client.Entities;
using MarketMiner.Client.Proxies.ServiceCallers;
using N.Core.Common.Core;
using P.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using MACC = MarketMiner.Api.Client.Common;

namespace MarketMiner.Algorithm.Common
{
   public abstract class ThrustDetectorBase : PatternDetectorBase
   {
      protected Chart _chart;
      protected double _acrossFramesCoefficient = 0.386;
      protected double _focusBreakoutCoefficient = .25;
      protected double _minThrustPercentRange = .003;

      public override void Initialize(IParameters parameters)
      {
         base.Initialize(parameters);

         _acrossFramesCoefficient = _parameters.GetDouble("acrossframescoefficient") ?? _acrossFramesCoefficient;
         _focusBreakoutCoefficient = _parameters.GetDouble("focusBreakoutCoefficient") ?? _focusBreakoutCoefficient;
         _minThrustPercentRange = _parameters.GetDouble("minThrustPercentRange") ?? _minThrustPercentRange;
      }

      public override IPattern DetectPattern(Chart chart)
      {
         _chart = chart;

         // first, check db
         Thrust thrust = GetActiveThrust();

         // second, detect
         if (thrust == null)
         {
            thrust = DetectThrust();
         }

         _chart.Patterns.RemoveAll(p => p.Type == MACC.Constants.SignalType.Thrust);

         if (thrust != null)
         {
            SetFocusPrice(thrust);

            thrust = ValidateThrust(thrust);
         }

         if (thrust != null)
            _chart.Patterns.Add(thrust);

         return thrust;
      }

      protected virtual Thrust ValidateThrust(Thrust thrust)
      {
         // consider filtering out thrusts that have the same SignalTime as previous thrusts
         // if the FocusPrice isn't a breakout

         return thrust;
      }

      protected virtual Thrust SetFocusPrice(Thrust thrust)
      {
         // set the focus price
         IPatternFactory thrustFactory = ObjectBase.Container.GetExportedValue<IPatternFactory>("ThrustFactory");
         thrustFactory.Initialize(_parameters);
         thrust = (Thrust)thrustFactory.BuildPattern(_chart, thrust);

         return thrust;
      }

      #region Thrust detection guidelines
      /*
          upThrust 
          traverse >= 10 bars
          >= 7 bars have low bid above the 3x3
          >= 5 consecutive bars must have low bid above the 3x3
          sequence may not start with > 3 bars with low bid below 3x3
          if (any of?) top 3 bars of sequence have low bid below 3x3 then lowest low bid of those below the 3x3 must be > .386 ask price
          thrust range must be >= (.06 x thrust ReactionPrice)
          MACD histogram value of all thrust frames must be > 0

          downThrust
          traverse >= 10 bars
          >= 7 bars have low bid above the 3x3
          >= 5 consecutive bars must have low bid above the 3x3
          sequence may not start with > 3 bars with low bid below 3x3
          if (any of?) top 3 bars of sequence have low bid below 3x3 then lowest low bid of those below the 3x3 must be > .386 ask price
          thrust range must be >= (.06 x thrust ReactionPrice)
          MACD histogram value of all thrust frames must be > 0
          above or below the 3x3?
       */
      #endregion

      /// <summary>
      /// Searches the chart for a thrust pattern.
      /// </summary>
      /// <param name="chart">Chart object containing at least 20 frames</param>
      /// <returns>A tradeable thrust object, if found.</returns>
      protected abstract Thrust DetectThrust();

      /// <summary>
      /// Composes a thrust object
      /// </summary>
      /// <param name="reactionFrame">Reaction frame of the thrust</param>
      /// <param name="direction">Direction of the thrust</param>
      /// <returns></returns>
      protected virtual Thrust BuildThrust(Frame reactionFrame, EPatternDirection direction)
      {
         // create the thrust
         Thrust thrust = new Thrust()
         {
            Direction = direction,
            Instrument = _chart.Instrument,
            Granularity = _chart.Granularity,
            SignalTime = reactionFrame.Bar.time,
            SignalPrice = direction == EPatternDirection.Up ? reactionFrame.Bar.lowMid : reactionFrame.Bar.highMid,
            Side = direction == EPatternDirection.Up ? MACC.Constants.SignalSide.Buy : MACC.Constants.SignalSide.Sell
         };

         return thrust;
      }

      #region Utilities
      /// <summary>
      /// Finds the reaction frame of a detected thrust
      /// </summary>
      /// <param name="pilotFrame">Frame at which a valid thrust was confirmed</param>
      /// <param name="frameIndex">The _chart index of the pilot frame</param>
      /// <param name="direction">The direction of the confirmed thrust</param>
      /// <returns>The reaction frame of the thrust</returns>
      protected virtual Frame FindReactionFrame(Frame pilotFrame, int frameIndex, EPatternDirection direction)
      {
         #region reaction frame logic
         /// for up thrust
         ///   tentative rxn frame is the one with lowBid < 3x3 and 2 preceding candles has a higher chained lowBid
         ///   do a forward search of the chart bars to confirm the rxn frame has lowest lowBid
         ///   the confirmed reaction frame is the one with the lowest lowBid
         /// for down thrust
         ///   tentative rxn frame is the one with highBid > 3x3 and 2 preceding candles has a lower chained highBid
         ///   do a forward search of the chart bars to confirm the rxn frame has highest highBid
         ///   the confirmed reaction frame is the one with the highest highBid

         /// do not sweat the breakouts from consolidation (see type G in notes)
         /// these should yield deeper rxn at worst meaning a missed trade
         /// the timekill of the trade will mop these up
         #endregion

         // find reaction candle
         Frame horizonFrame = pilotFrame;
         IPriceBar horizonCandle = horizonFrame.Bar;
         bool horizonFrameConfirmed = false;

         while (!horizonFrameConfirmed)
         {
            Frame precedingFrame = _chart.Frames[frameIndex - 1];
            IPriceBar precedingCandle01 = _chart.Frames[frameIndex - 1].Bar;
            IPriceBar precedingCandle02 = _chart.Frames[frameIndex - 2].Bar;
            IIndicator horizon3x3 = horizonFrame.Indicators.First(k => k.Type == IndicatorType.DisplacedMovingAverage);

            if (direction == EPatternDirection.Up)
            {
               if (horizonCandle.lowMid < horizon3x3.Value
                  && precedingCandle01.lowMid > horizonCandle.lowMid
                  && precedingCandle02.lowMid > precedingCandle01.lowMid)
               {
                  horizonFrameConfirmed = true;
               }
            }

            if (direction == EPatternDirection.Down)
            {
               if (horizonCandle.highMid > horizon3x3.Value
                  && precedingCandle01.highMid < horizonCandle.highMid
                  && precedingCandle02.highMid < precedingCandle01.highMid)
               {
                  horizonFrameConfirmed = true;
               }
            }

            if (!horizonFrameConfirmed)
            {
               horizonFrame = precedingFrame;
               horizonCandle = precedingCandle01;

               frameIndex--;
            }
         }

         // forward search to confirm rxn frame
         Frame reactionFrame = horizonFrame;
         for (int i = frameIndex; i < _chart.Frames.Count; i++)
         {
            Frame currentFrame = _chart.Frames[i];

            if (direction == EPatternDirection.Up)
            {
               if (currentFrame.Bar.lowMid <= reactionFrame.Bar.lowMid)
                  reactionFrame = currentFrame;
            }
            else
            {
               if (currentFrame.Bar.highMid >= reactionFrame.Bar.highMid)
                  reactionFrame = currentFrame;
            }
         }

         return reactionFrame;
      }

      protected virtual Thrust GetActiveThrust()
      {
         Thrust thrust = null;
         Signal signal = null;

         signal = SubscriptionCaller.Instance().GetActiveSignalsByType(MACC.Constants.SignalType.Thrust).FirstOrDefault(s => s.Granularity == _chart.Granularity && s.Instrument == _chart.Instrument);

         if (signal != null)
         {
            thrust = new Thrust();
            thrust.Direction = signal.Side == MACC.Constants.SignalSide.Buy ? EPatternDirection.Up : EPatternDirection.Down;
            thrust.InjectWith(signal);

            if (signal.StrategyTransactionID.HasValue)
            {
               IEnumerable<StrategyTransaction> transactions;
               StrategyTransaction orderTransaction, lastOrderUpdate, lastTradeUpdate;

               // get transaction collection from db
               int orderTransactionID = signal.StrategyTransactionID.Value;
               transactions = StrategyCaller.Instance().GetStrategyTransactionsCollectionAsync(orderTransactionID).Result;
               transactions = transactions.OrderBy(t => t.BrokerTransactionID);

               orderTransaction = transactions.FirstOrDefault(t => t.StrategyTransactionID == orderTransactionID);
               lastOrderUpdate = transactions.LastOrDefault(t => t.Type == MACC.Constants.TransactionTypes.OrderUpdate);

               // update prices
               if (lastOrderUpdate != null)
                  thrust.SetOrUpdatePrices(lastOrderUpdate.Price, lastOrderUpdate.TakeProfit, lastOrderUpdate.StopLoss, null);
               else
                  thrust.SetOrUpdatePrices(orderTransaction.Price, orderTransaction.TakeProfit, orderTransaction.StopLoss, null);

               lastTradeUpdate = transactions.LastOrDefault(t => t.Type == MACC.Constants.TransactionTypes.TradeUpdate);
               if (lastTradeUpdate != null)
                  thrust.SetOrUpdatePrices(null, lastTradeUpdate.TakeProfit, lastTradeUpdate.StopLoss, null);

               // update price times
               List<StrategyTransaction> updateTransactions = transactions.Where(t => t.Type == MACC.Constants.TransactionTypes.OrderUpdate || t.Type == MACC.Constants.TransactionTypes.TradeUpdate).ToList();

               StrategyTransaction lastTakeProfitUpdate = orderTransaction;
               StrategyTransaction lastStopLossPriceUpdate = orderTransaction;

               if (updateTransactions.Count() > 0)
               {
                  updateTransactions.OrderByDescending(t => t.BrokerTransactionID).ToList().ForEach(t =>
                  {
                     if (t.TakeProfit != lastTakeProfitUpdate.TakeProfit)
                        lastTakeProfitUpdate = t;
                     if (t.StopLoss != lastStopLossPriceUpdate.StopLoss)
                        lastStopLossPriceUpdate = t;
                  });
               }

               thrust.SetOrUpdatePriceUpdatedTimes(null, lastTakeProfitUpdate.Time, lastStopLossPriceUpdate.Time);
            }
         }

         return thrust;
      }

      /// <summary>
      /// Determines if a list of leaderSides are equal to a specified value.
      /// </summary>
      /// <param name="leaderSides">The list of AboveBelowDMA values to examine.</param>
      /// <param name="trueFor">The AboveBelowDMA side all leaderSides should equal.</param>
      /// <returns>True if all leader values equal the truefor value. 
      ///          False all leader values do not equal the truefor value.</returns>
      protected virtual bool CheckLeaders(List<AboveBelowDMA> leaderSides, AboveBelowDMA trueFor)
      {
         bool result = false;

         result = leaderSides.TrueForAll(x => x == trueFor);

         return result;
      }

      protected virtual bool CheckAcrossFrames(List<Frame> acrossFrames, AboveBelowDMA trueFor)
      {
         foreach (Frame frame in acrossFrames)
         {
            IIndicator displaced3x3 = frame.Indicators.First(k => k.Type == IndicatorType.DisplacedMovingAverage && k.ParentOrdinal == 0);

            double acrossHeight;
            double barHeight = frame.Bar.highMid - frame.Bar.lowMid;

            if (trueFor == AboveBelowDMA.Above)
               acrossHeight = displaced3x3.Value.Value - frame.Bar.lowMid;
            else
               acrossHeight = frame.Bar.highMid - displaced3x3.Value.Value;

            if ((acrossHeight / barHeight) > _acrossFramesCoefficient)
               return false;
         }

         return true;
      }

      /// <summary>
      /// Checks each frame price bar after the startIndex frame price bar to discover those that cross the 3x3.
      /// If a bar crosses the 3x3, no more than 0.382*[bar length] may be across the 3x3.
      /// </summary>
      /// <param name="reactionIndex"></param>
      /// <param name="trueFor"></param>
      /// <returns></returns>
      protected virtual bool CheckAcrossFrames(int startIndex, AboveBelowDMA trueFor)
      {
         IEnumerable<Frame> afterReactionFrames = _chart.Frames.Skip(startIndex + 1);

         return CheckAcrossFrames(afterReactionFrames.ToList(), trueFor);
      }

      protected virtual int GetCheckAcrossIndex(Frame reactionFrame, EPatternDirection direction)
      {
         int checkAcrossIndex = _chart.Frames.IndexOf(reactionFrame);

         while (checkAcrossIndex < _chart.Frames.Count)
         {
            Frame frame = _chart.Frames[checkAcrossIndex];
            IIndicator displaced3x3 = frame.Indicators.First(k => k.Type == IndicatorType.DisplacedMovingAverage && k.ParentOrdinal == 0);

            if (direction == EPatternDirection.Up && frame.Bar.lowMid >= displaced3x3.Value)
            {
               break;
            }
            else if (direction == EPatternDirection.Down && frame.Bar.highMid <= displaced3x3.Value)
            {
               break;
            }

            checkAcrossIndex++;
         }

         return checkAcrossIndex;
      }

      public enum AboveBelowDMA
      {
         Unknown,
         Above,
         Below,
         Across
      }
      #endregion
   }
}
