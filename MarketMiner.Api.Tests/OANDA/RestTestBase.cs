namespace MarketMiner.Api.Tests.OANDA
{
   public class RestTestBase
   {
      protected readonly RestTestResults _results;
      protected short _failedTests = 0;

      public RestTestResults Results { get { return _results; } }
      public short Failures { get { return _failedTests; } }

      public RestTestBase(RestTestResults results)
      {
         _results = results;
      }
   }
}
