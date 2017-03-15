using MarketMiner.Api.Client.Common;
using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.Common.Contracts;
using MarketMiner.Api.Client.Common.Patterns.Dinapoli;
using MarketMiner.Api.Common.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Algorithm.Common
{
   [Export("ThrustDetector02", typeof(IPatternDetector))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class ThrustDetector02 : ThrustDetectorBase
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
            short above3x3Count = 0, below3x3Count = 0, across3x3Count = 0;

            AboveBelowDMA currentCandleSide = AboveBelowDMA.Unknown;
            List<AboveBelowDMA> leaderSides = new List<AboveBelowDMA>();
            List<Frame> acrossFrames = new List<Frame>();

            // only need to sniff 20 frames to find thrust
            // if not in 20 frames, whatever thrust exists has yielded to a consolidation period .. no good
            for (int loopCount = 1; true; loopCount++)
            {
               int i = _chart.Frames.Count - loopCount;
               frame = _chart.Frames[i];

               IPriceBar candle = frame.Bar;
               IIndicator displaced3x3 = frame.Indicators.First(k => k.Type == IndicatorType.DisplacedMovingAverage && k.ParentOrdinal == 0);

               if (candle.lowMid >= displaced3x3.Value)
               {
                  above3x3Count++;
                  currentCandleSide = AboveBelowDMA.Above;
               }
               else if (candle.highMid <= displaced3x3.Value)
               {
                  below3x3Count++;
                  currentCandleSide = AboveBelowDMA.Below;
               }
               else
               {
                  across3x3Count++;
                  currentCandleSide = AboveBelowDMA.Across;
                  acrossFrames.Add(frame);
               }

               // save the first 3 bar sides
               if (leaderSides.Count < 3) leaderSides.Add(currentCandleSide);

               // start thrust parsing once 10 bars have been examined
               if (loopCount == 7)
               {
                  upThrustFound = above3x3Count >= 5 && below3x3Count == 0 && CheckLeaders(leaderSides, AboveBelowDMA.Above) && CheckAcrossFrames(acrossFrames, AboveBelowDMA.Above);

                  downThrustFound = below3x3Count >= 5 && above3x3Count == 0 && CheckLeaders(leaderSides, AboveBelowDMA.Below) && CheckAcrossFrames(acrossFrames, AboveBelowDMA.Below);

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
                  }

                  break;
               }
            }
         }

         return thrust;
      }
   }
}
