using MarketMiner.Api.Client.Common;
using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.Common.Contracts;
using MarketMiner.Api.Client.Common.Patterns.Dinapoli;
using MarketMiner.Api.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using MACC = MarketMiner.Api.Client.Common;

namespace MarketMiner.Algorithm.Common.PatternManagers
{
   [Export("ThrustFactory", typeof(IPatternFactory))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class ThrustFactory : PatternFactoryBase
   {
      #region Declarations
      protected double _acrossFramesCoefficient;
      protected double _fillZoneReachedCoefficient;
      protected double _focusBreakoutCoefficient;
      protected double _minThrustPercentRange;
      protected double _takeProfitZoneReachedCoefficient;
      protected short _maxAfterFocusAcross3x3Count;
      protected short _minReactionToFocusSpan;
      #endregion

      public override void Initialize(IParameters parameters)
      {
         base.Initialize(parameters);

         _acrossFramesCoefficient = _parameters.GetDouble("acrossframescoefficient") ?? 0.386;
         _fillZoneReachedCoefficient = _parameters.GetDouble("thrustFillZoneReachedCoefficient") ?? 0.9;
         _focusBreakoutCoefficient = _parameters.GetDouble("focusBreakoutCoefficient") ?? 0.25;
         _maxAfterFocusAcross3x3Count = _parameters.GetShort("maxAfterFocusAcross3x3Count") ?? 3;
         _minThrustPercentRange = _parameters.GetDouble("minThrustPercentRange") ?? 0.003;
         _minReactionToFocusSpan = _parameters.GetShort("minReactionToFocusSpan") ?? 8;
         _takeProfitZoneReachedCoefficient = _parameters.GetDouble("takeProfitZoneReachedCoefficient") ?? 0.9;
      }

      public override IPattern BuildPattern(Chart chart, IPattern pattern)
      {
         Thrust thrust = (Thrust)pattern;

         SetThrustFocus(chart, thrust);
         SetThrustActive(chart, thrust, true);

         return thrust;
      }

      public override IPattern UpdatePattern(Chart chart, IPattern pattern)
      {
         Thrust thrust = (Thrust)pattern;

         SetThrustRetracementExtrema(chart, thrust);
         SetThrustFillZoneReached(chart, thrust);
         SetThrustTakeProfitZoneReached(chart, thrust);
         SetThrustFocus(chart, thrust);
         SetThrustActive(chart, thrust, false);

         return thrust;
      }

      protected virtual Thrust SetThrustFocus(Chart chart, Thrust thrust)
      {
         Frame reactionFrame = chart.Frames.Where(f => f.Bar.time == thrust.SignalTime).FirstOrDefault();
         int reactionIndex = chart.Frames.IndexOf(reactionFrame);

         IPriceBar focusBar = null;
         double focusPrice = thrust.Direction == EPatternDirection.Up ? reactionFrame.Bar.highMid : reactionFrame.Bar.lowMid;

         List<Frame> afterFocusFrames = new List<Frame>();

         for (int i = reactionIndex + 1; i < chart.Frames.Count; i++)
         {
            Frame frame = chart.Frames[i];

            if (thrust.Direction == EPatternDirection.Up)
            {
               if (frame.Bar.highMid > focusPrice)
               {
                  thrust.ReactionToFocusSpan = i - reactionIndex;

                  focusBar = frame.Bar;
                  focusPrice = frame.Bar.highMid;
                  afterFocusFrames.Clear();
               }
               else
               {
                  afterFocusFrames.Add(frame);
               }
            }
            else
            {
               if (frame.Bar.lowMid < focusPrice)
               {
                  thrust.ReactionToFocusSpan = i - reactionIndex;

                  focusBar = frame.Bar;
                  focusPrice = frame.Bar.lowMid;
                  afterFocusFrames.Clear();
               }
               else
               {
                  afterFocusFrames.Add(frame);
               }
            }
         }

         // update FocusTime and FocusPrice
         bool reactionBarIsFocusBar = focusBar == null;
         thrust.FocusTime = reactionBarIsFocusBar ? reactionFrame.Bar.time : focusBar.time;
         thrust.FocusPrice = focusPrice;

         return thrust;
      }

      /// <summary>
      /// Sets the extrema price of the thrust Fibonacci retracement
      /// </summary>
      /// <param name="chart">The chart object that contains the retracement.</param>
      /// <param name="side">The direction of the entry signal associated with the retracement.</param>
      /// <param name="focusIndex">The chart index of the retracement focus bar.</param>
      /// <returns></returns>
      protected virtual Thrust SetThrustRetracementExtrema(Chart chart, Thrust thrust)
      {
         int focusIndex = GetFocusIndex(chart, thrust);

         FibonacciRetracement retracement = thrust.Study as FibonacciRetracement;

         double? extremaPrice = null;

         // only if the chart has post-focus frames
         int i = focusIndex + 1;
         if (i < chart.Frames.Count)
         {
            // initialize
            if (thrust.Side == MACC.Constants.SignalSide.Buy)
               extremaPrice = chart.Frames[focusIndex].Bar.highMid;
            else
               extremaPrice = chart.Frames[focusIndex].Bar.lowMid;

            // discover       
            for (; i < chart.Frames.Count; i++)
            {
               if (thrust.Side == MACC.Constants.SignalSide.Buy)
                  extremaPrice = Math.Min(extremaPrice.Value, chart.Frames[i].Bar.lowMid);
               else
                  extremaPrice = Math.Max(extremaPrice.Value, chart.Frames[i].Bar.highMid);
            }
         }

         retracement.ExtremaPrice = extremaPrice;
         if (retracement.ExtremaChanged()) retracement.ExtremaIndex = i;

         return thrust;
      }

      protected virtual Thrust SetThrustFillZoneReached(Chart chart, Thrust thrust)
      {
         // if already set, exit.
         if (thrust.FillZoneReached())
            return thrust;

         #region did price reach the fill zone?
         List<Frame> afterFocusFrames = chart.Frames.Skip(GetFocusIndex(chart, thrust) + 1).ToList();

         Frame fillZoneReachedFrame = afterFocusFrames.FirstOrDefault(frame =>
         {
            double focusToFillZoneDelta = _fillZoneReachedCoefficient * Math.Abs(thrust.FocusPrice - thrust.EntryPrice.Value);

            if (thrust.Direction == EPatternDirection.Up)
            {
               if (frame.Bar.lowMid <= thrust.FocusPrice - focusToFillZoneDelta)
               {
                  thrust.FillZoneReachedIndex = chart.Frames.IndexOf(frame);
                  return true;
               }
            }
            else
            {
               if (frame.Bar.highMid >= focusToFillZoneDelta + thrust.FocusPrice)
               {
                  thrust.FillZoneReachedIndex = chart.Frames.IndexOf(frame);
                  return true;
               }
            }

            return false;
         });
         #endregion

         return thrust;
      }

      protected virtual Thrust SetThrustTakeProfitZoneReached(Chart chart, Thrust thrust)
      {
         FibonacciRetracement retracement = thrust.Study as FibonacciRetracement;

         #region did price reach the takeProfit zone?
         if (thrust.FillZoneReached())
         {
            double? searchTakeProfitPrice = null;
            List<Frame> searchFrames = null;

            if (!thrust.TakeProfitZoneReached())
            {
               searchTakeProfitPrice = thrust.TakeProfitPrice.Value;
               searchFrames = chart.Frames.Skip(thrust.FillZoneReachedIndex.Value + 1).ToList();
            }
            else
            {
               if (retracement.ExtremaChanged())
               {
                  int multiplier = thrust.Side == MACC.Constants.SignalSide.Buy ? 1 : -1;
                  double r382Price = retracement.LevelPrice(FibonacciLevel.R382);
                  searchTakeProfitPrice = r382Price + (0.618 * Math.Abs(retracement.FocusPrice - r382Price) * multiplier);

                  searchFrames = chart.Frames.Skip(retracement.ExtremaIndex.Value + 1).ToList();
               }
            }

            Frame takeProfitZoneReachedFrame = searchFrames.FirstOrDefault(frame =>
            {
               double extremaToTakeProfitZoneDelta = _takeProfitZoneReachedCoefficient * Math.Abs(searchTakeProfitPrice.Value - retracement.ExtremaPrice.Value);

               bool takeProfitZoneReached = false;

               if (thrust.Direction == EPatternDirection.Up)
                  takeProfitZoneReached = (frame.Bar.highMid >= retracement.ExtremaPrice.Value + extremaToTakeProfitZoneDelta);
               else
                  takeProfitZoneReached = (frame.Bar.lowMid <= retracement.ExtremaPrice.Value - extremaToTakeProfitZoneDelta);

               if (takeProfitZoneReached)
               {
                  thrust.TakeProfitZoneReachedIndex = chart.Frames.IndexOf(frame);
                  thrust.TakeProfitZoneReachedFocusPrice = thrust.FocusPrice;
                  return true;
               }

               return false;
            });
         }
         #endregion

         // is it a breakout?
         if (thrust.TakeProfitZoneReached())
         {
            double breakoutDelta = thrust.SignalPrice * _focusBreakoutCoefficient * _minThrustPercentRange;

            if (thrust.Direction == EPatternDirection.Up)
               thrust.Breakout = thrust.FocusPrice >= thrust.TakeProfitZoneReachedFocusPrice.Value + breakoutDelta;
            else
               thrust.Breakout = thrust.FocusPrice <= thrust.TakeProfitZoneReachedFocusPrice.Value - breakoutDelta;

            if (thrust.Breakout)
            {
               thrust.FillZoneReachedIndex = null;
               thrust.TakeProfitZoneReachedIndex = null;
               thrust.TakeProfitZoneReachedFocusPrice = null;
            }
         }

         return thrust;
      }

      protected Thrust SetThrustActive(Chart chart, Thrust thrust, bool isNewThrust)
      {
         thrust.Active = true;

         List<Frame> afterFocusFrames = chart.Frames.Skip(GetFocusIndex(chart, thrust) + 1).ToList();

         if (isNewThrust)
         {
            // should an order be placed?

            // thrust range must be >= _percentRange
            //    should some consideration of the time frame happen?
            //    .006 is for hourly
            //    use a lower value for lower time frame scalping?
            if (Math.Abs(thrust.FocusPrice - thrust.SignalPrice) / thrust.SignalPrice < _minThrustPercentRange)
            {
               thrust.Active = false;
               return thrust;
            }

            if (thrust.ReactionToFocusSpan < _minReactionToFocusSpan)
            {
               thrust.Active = false;
               return thrust;
            }

            if (afterFocusFrames.Count() == 0)
               return thrust;
            else
            {
               FibonacciRetracement retracement = thrust.Study as FibonacciRetracement;

               Frame r382ReachedOrExceededFrame = afterFocusFrames.FirstOrDefault(frame =>
               {
                  if (thrust.Direction == EPatternDirection.Up)
                  {
                     if (frame.Bar.lowMid <= retracement.LevelPrice(FibonacciLevel.R382))
                        return true;
                  }
                  else
                  {
                     if (frame.Bar.highMid >= retracement.LevelPrice(FibonacciLevel.R382))
                        return true;
                  }

                  return false;
               });

               thrust.Active = r382ReachedOrExceededFrame == null;
               return thrust;
            }
         }
         else
         {
            // should the order be cancelled if not yet filled? 
            // if filled, a 'false' from this doesn't matter
            #region afterFocusAcross3x3Count function
            Func<EPatternDirection, short> afterFocusAcross3x3Count = (direction) =>
               {
                  short across3x3Count = 0;
                  Frame firstConfirmedAcross3x3 = null;

                  foreach (Frame frame in afterFocusFrames)
                  {
                     IIndicator displaced3x3 = frame.Indicators.First(k => k.Type == IndicatorType.DisplacedMovingAverage && k.ParentOrdinal == 0);

                     if (direction == EPatternDirection.Up)
                     {
                        // grab the first bar below the 3x3 on close
                        if (frame.Bar.closeMid < displaced3x3.Value && firstConfirmedAcross3x3 == null)
                        {
                           firstConfirmedAcross3x3 = frame;
                           continue;
                        }

                        if (firstConfirmedAcross3x3 != null && frame.Bar.lowMid < displaced3x3.Value)
                           across3x3Count++;
                     }
                     else
                     {
                        // grab the first bar above the 3x3 on close
                        if (frame.Bar.closeMid > displaced3x3.Value && firstConfirmedAcross3x3 == null)
                        {
                           firstConfirmedAcross3x3 = frame;
                           continue;
                        }

                        if (firstConfirmedAcross3x3 != null && frame.Bar.highMid > displaced3x3.Value)
                           across3x3Count++;
                     }
                  }

                  return across3x3Count;
               };
            #endregion

            // enforce a reactionToFocusSpan of 9 bars to filter out 'turn' thrusts
            //    turn thrusts happen detection triggers on bars at turns in price
            //    these are new thrusts but need 'seasoning' (ie. >= 9 bars) to be trade worthy
            // enforce Dinapoli rule on no more than 3 bars across 3x3 after initial closing bar across 3x3
            thrust.Active = afterFocusAcross3x3Count(thrust.Direction) <= _maxAfterFocusAcross3x3Count;
            return thrust;
         }
      }

      #region utilities
      protected virtual int GetFocusIndex(Chart chart, Thrust thrust)
      {
         Frame focusFrame = chart.Frames.Where(f => f.Bar.time == thrust.FocusTime).FirstOrDefault();
         int focusIndex = chart.Frames.IndexOf(focusFrame);
         return focusIndex;
      }
      #endregion
   }
}
