using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.OANDA;
using MarketMiner.Api.OANDA.TradeLibrary.DataTypes;
using MarketMiner.Api.OANDA.TradeLibrary.DataTypes.Communications.Requests;
using MarketMiner.Api.Tests.OANDA;
using System;
using System.Threading.Tasks;

namespace MarketMiner.Algorithm.Tests
{
   public class DinapoliTests : RestTestBase
   {
      #region Constructors
      public DinapoliTests(RestTestResults results)
         : base(results)
      {
      }
      #endregion


      #region Public methods
      /// <summary>
      /// Candle retrieval tests
      /// Candle request requirements: http://developer.oanda.com/rest-live/rates/#aboutCandlestickRepresentation
      /// </summary>
      /// <returns></returns>
      public async Task RunTests(string instr = "USD_JPY")
      {
         try
         {
            _results.Add(string.Format("Starting test: {0}\n", this.ToString()));

            Func<CandlesRequest> request = () => new CandlesRequest { instrument = instr, granularity = EGranularity.H1 };
            await RunThrustTest(request);
         }
         catch (Exception e)
         {

         }
         finally
         {
            _results.Add(string.Format("Completed test: {0}\n", this.ToString()));
         }
      }

      protected async Task RunThrustTest(Func<CandlesRequest> request)
      {
         short count = 0;

         var thrustRequest = request();
         thrustRequest.count = 100;
         var thrustResult = await Rest.GetCandlesAsync(thrustRequest);

         // if candles endpoint is working then proceed
         if (thrustResult.Count > 0)
         {
            foreach (Candle candle in thrustResult)
            {
               // compute close 3x3 value
               if (count >= 5)
               {
                  var closeAverage = new SimpleMovingAverage(3)
                  {
                     Value = (thrustResult[count - 3].closeBid + thrustResult[count - 4].closeBid + thrustResult[count - 5].closeBid) / 3,
                  };

                  var candleOpenBid = Math.Round(candle.openBid, 2);
                  var candleCloseBid = Math.Round(candle.closeBid, 2);
                  var candleHighBid = Math.Round(candle.highBid, 2);
                  var candleLowBid = Math.Round(candle.lowBid, 2);

                  if (count >= 90) // only need enough to validate the 3x3 computations
                  {
                     _results.Add(string.Format("Candle {0} - {1} has open bid ({2}).", thrustRequest.instrument, count, candleOpenBid));
                     _results.Add(string.Format("Candle {0} - {1} has close bid ({2}).", thrustRequest.instrument, count, candleCloseBid));
                     _results.Add(string.Format("Candle {0} - {1} has high bid ({2}).", thrustRequest.instrument, count, candleHighBid));

                     // determine if the candle low is above or below the 3x3
                     string closePosition = candle.lowBid > closeAverage.Value ? "above" : "below";
                     char arrow = closePosition == "above" ? (char)30 : (char)31;
                     _results.Add(string.Format("Candle {0} - {1} has low bid ({2}) {3} 3x3 at {4}. {5}\n", thrustRequest.instrument, count, candleLowBid, closePosition, Math.Round(closeAverage.Value.GetValueOrDefault(), 2), arrow));
                  }
               }

               count++;
            }
         }
         else
         {
            _results.Verify(false, "Thrust tests did not receive candles.");
         }
      }
      #endregion
   }
}
