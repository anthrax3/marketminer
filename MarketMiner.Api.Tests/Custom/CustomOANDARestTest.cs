using MarketMiner.Api.OANDA;
using MarketMiner.Api.OANDA.TradeLibrary;
using MarketMiner.Api.OANDA.TradeLibrary.DataTypes;
using MarketMiner.Api.Tests.OANDA;
using System.Threading;
using System.Threading.Tasks;

namespace MarketMiner.Api.Tests.Custom
{
   public class CustomOANDARestTest : RestTest
   {
      #region Declarations
      private short _dataCount;
      #endregion

      public override async Task RunTest(EEnvironment environment, string token, int account)
      {
         await base.RunTest(environment, token, account);

         // check for failures
         foreach (var message in _results.Messages)
         {
            if (message.Value.IndexOf("False").Equals(0))
               _failedTests++;
         }

         // only if no failures
         //if (_failedTests.Equals(0))
         //   await new DinapoliTests(_results).RunTests();
      }

      protected override void RunStreamingRatesTest()
      {
         _dataCount = 0;

         RatesSession session = new RatesSession(_accountId, _instruments);
         _tickReceived = new Semaphore(0, 100); // 100 threads max
         session.DataReceived += SessionOnDataReceived;
         session.StartSession();
         _results.Add("Starting rate stream test");

         // bespoke ///////////////////////////////////////////////////////////////////////
         while (_dataCount < 10)
         {
            // this keeps the request stream active until 10 responses are received
            // if things hang, add a timeout .. or does a WaitOne() timeout trigger SessionDataReceived()? 
         }
         //////////////////////////////////////////////////////////////////////////////////

         // allow 10 seconds for response then recycle the thread
         bool success = _tickReceived.WaitOne(10000);
         session.StopSession();
         _results.Verify(success, "Streaming rates successfully received");
      }

      protected override void SessionOnDataReceived(RateStreamResponse data)
      {
         // _results.Verify the tick data
         _results.Verify(data.tick != null, "Streaming Tick received");
         if (data.tick != null)
         {
            // bespoke ///////////////////////////////////////////////////////////////////////
            _dataCount++;

            _results.Add(string.Format("Bid-Ask data received: {0} - {1} - {2}", data.tick.instrument, data.tick.bid, data.tick.ask));
            //////////////////////////////////////////////////////////////////////////////////

            _results.Verify(data.tick.ask > 0 && data.tick.bid > 0, "Streaming tick has bid/ask");
            _results.Verify(!string.IsNullOrEmpty(data.tick.instrument), "Streaming tick has instrument");
         }
         _tickReceived.Release();
      }
   }
}