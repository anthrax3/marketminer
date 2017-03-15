using MarketMiner.Api.Client.Common;
using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.Common.Contracts;
using MarketMiner.Api.Client.Common.Patterns.Dinapoli;
using MarketMiner.Api.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Algorithm.Common
{
   [Export("ThrustDetector01", typeof(IPatternDetector))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class ThrustDetector01 : ThrustDetectorBase
   {
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
      protected override Thrust DetectThrust()
      {
         Thrust thrust = null;

         if (_chart.Frames.Count >= 20)
         {
            Frame frame;

            bool upThrustFound = false, downThrustFound = false;
            short above3x3Count = 0, above3x3Links = 0, below3x3Count = 0, below3x3Links = 0, across3x3Count = 0;

            Func<short> sequentialAbove3x3Count = () => { return above3x3Links > 0 ? (short)(above3x3Links + 1) : (short)0; };
            Func<short> sequentialBelow3x3Count = () => { return below3x3Links > 0 ? (short)(below3x3Links + 1) : (short)0; };

            AboveBelowDMA previousCandleSide = AboveBelowDMA.Unknown, currentCandleSide = AboveBelowDMA.Unknown;
            List<AboveBelowDMA> leaderSides = new List<AboveBelowDMA>();

            // only need to sniff 20 frames to find thrust
            // if not in 20 frames, whatever thrust exists has yielded to a consolidation period .. no good
            short loopCount = 0;
            while (loopCount < 20)
            {
               loopCount++;
               int i = _chart.Frames.Count - loopCount;
               frame = _chart.Frames[i];

               IPriceBar candle = frame.Bar;
               IIndicator displaced3x3 = frame.Indicators.First(k => k.Type == IndicatorType.DisplacedMovingAverage && k.ParentOrdinal == 0);

               if (candle.lowMid >= displaced3x3.Value)
               {
                  above3x3Count++;

                  if (previousCandleSide == AboveBelowDMA.Above) above3x3Links++;

                  currentCandleSide = previousCandleSide = AboveBelowDMA.Above;
               }
               else if (candle.highMid <= displaced3x3.Value)
               {
                  below3x3Count++;

                  if (previousCandleSide == AboveBelowDMA.Below) below3x3Links++;

                  currentCandleSide = previousCandleSide = AboveBelowDMA.Below;
               }
               else
               {
                  across3x3Count++;
                  currentCandleSide = previousCandleSide = AboveBelowDMA.Across;
               }

               // save the first 3 bar sides
               if (leaderSides.Count < 3) leaderSides.Add(currentCandleSide);

               // start thrust parsing once 10 bars have been examined
               if (loopCount > 9)
               {
                  upThrustFound = sequentialAbove3x3Count() >= 6 && sequentialBelow3x3Count() <= 3
                     && CheckLeaders(leaderSides, AboveBelowDMA.Above) && !Displaced3x3HasDoubleDip(i, EPatternDirection.Up);

                  downThrustFound = sequentialBelow3x3Count() >= 6 && sequentialAbove3x3Count() <= 3
                     && CheckLeaders(leaderSides, AboveBelowDMA.Below) && !Displaced3x3HasDoubleDip(i, EPatternDirection.Down);

                  if (upThrustFound || downThrustFound)
                  {
                     EPatternDirection direction = upThrustFound ? EPatternDirection.Up : EPatternDirection.Down;
                     Frame reactionFrame = FindReactionFrame(frame, i, direction);

                     if (reactionFrame != null)
                     {
                        int checkAcrossIndex = GetCheckAcrossIndex(reactionFrame, direction);
                        AboveBelowDMA checkAcrossSide = upThrustFound ? AboveBelowDMA.Above : AboveBelowDMA.Below;

                        if (CheckAcrossFrames(checkAcrossIndex, checkAcrossSide))
                           thrust = BuildThrust(reactionFrame, direction);
                     }

                     break;
                  }
               }
            }
         }

         return thrust;
      }

      #region Utilities
      protected virtual bool IsTradeableThrust(Thrust thrust)
      {
         return false;

         //bool result = base.IsTradeableThrust(thrust);

         //if (result)
         //{
         //   // does the thrust 3x3 have too many kinks in it?        
         //   Frame reactionFrame = _chart.Frames.FirstOrDefault(f => f.Bar.time == thrust.SignalTime);
         //   if (Displaced3x3HasDoubleDip(_chart.Frames.IndexOf(reactionFrame), thrust.Direction))
         //      result = false;
         //}

         //return result;
      }

      /// <summary>
      /// Determines if the thrust displaced 3x3 is sufficiently smooth.
      /// </summary>
      /// <param name="frameIndex">The _chart index of the first frame of the newly discovered thrust.</param>
      /// <param name="direction">The direction of the thrust.</param>
      /// <returns>True if the thrust 3x3 has 2 or more dips. False if the thrust 3x3 has less than 2 dips.</returns>
      protected virtual bool Displaced3x3HasDoubleDip(int frameIndex, EPatternDirection direction)
      {
         bool result = false;

         List<Frame> thrustFrames = _chart.Frames.Skip(frameIndex).ToList();

         bool doubleDipFound = false;
         for (int i = 0; (i + 2) < thrustFrames.Count; i++)
         {
            Frame frameA = thrustFrames[i], frameB = thrustFrames[i + 1], frameC = thrustFrames[i + 2];
            IIndicator displaced3x3A = frameA.Indicators.First(k => k.Type == IndicatorType.DisplacedMovingAverage && k.ParentOrdinal == 0);
            IIndicator displaced3x3B = frameB.Indicators.First(k => k.Type == IndicatorType.DisplacedMovingAverage && k.ParentOrdinal == 0);
            IIndicator displaced3x3C = frameC.Indicators.First(k => k.Type == IndicatorType.DisplacedMovingAverage && k.ParentOrdinal == 0);

            if (direction == EPatternDirection.Up)
               doubleDipFound = displaced3x3A.Value > displaced3x3B.Value && displaced3x3B.Value > displaced3x3C.Value;
            else
               doubleDipFound = displaced3x3A.Value < displaced3x3B.Value && displaced3x3B.Value < displaced3x3C.Value;

            if (doubleDipFound)
               return true;
         }

         return result;
      }
      #endregion
   }
}
