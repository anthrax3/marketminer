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
   [Export("ThrustDetector03", typeof(IPatternDetector))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class ThrustDetector03 : ThrustDetectorBase
   {
      #region Thrust detection guidelines
      /*
          upThrust 
          traverse >= 10 bars
          >= 7 bars have low mid above the 3x3
          >= 5 consecutive bars must have low mid above the 3x3
          sequence may not start with > 3 bars with low mid below 3x3
          if (any of?) top 3 bars of sequence have low mid below 3x3 then lowest low mid of those below the 3x3 must be > .386 ask price
          thrust range must be >= (.06 x thrust ReactionPrice)
          MACD histogram value of all thrust frames must be > 0

          downThrust
          traverse >= 10 bars
          >= 7 bars have low mid above the 3x3
          >= 5 consecutive bars must have low mid above the 3x3
          sequence may not start with > 3 bars with low mid below 3x3
          if (any of?) top 3 bars of sequence have low mid below 3x3 then lowest low mid of those below the 3x3 must be > .386 ask price
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
         _minThrustPercentRange = .003;

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
                     && CheckLeaders(leaderSides, AboveBelowDMA.Above);

                  downThrustFound = sequentialBelow3x3Count() >= 6 && sequentialAbove3x3Count() <= 3
                     && CheckLeaders(leaderSides, AboveBelowDMA.Below);

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
   }
}
