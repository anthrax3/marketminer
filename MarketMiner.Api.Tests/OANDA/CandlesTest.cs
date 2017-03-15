using MarketMiner.Api.OANDA;
using MarketMiner.Api.OANDA.TradeLibrary.DataTypes.Communications.Requests;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MarketMiner.Api.Tests.OANDA
{
   public class CandlesTest : RestTestBase
   {
      #region Constructors
      public CandlesTest(RestTestResults results)
         : base(results)
      {
      }
      #endregion
      //---------------------------------------------------------------------------------------------------

      #region Public methods
      public async Task Run()
      {
         // Setup minimum requirements
         Func<CandlesRequest> request = () => new CandlesRequest { instrument = "EUR_USD" };
         // This handles the unmodified, and all defaults
         RunBasicTests(request);

         // test count max
         TestCount(request, 5000, false, "Retrieved max candles");
         // test count max + 1
         TestCount(request, 5001, true, "Exceeded max candles");
         // test count min
         TestCount(request, 1, false, "Retrieved min candles");
         // test count min - 1
         TestCount(request, 0, true, "Below min candles");
      }

      public async Task RunBasicTests(Func<CandlesRequest> request)
      {
         // Most basic request
         var result = await Rest.GetCandlesAsync(request());
         _results.Verify(result.Count > 0, "Retrieved basic candles");

         // Test default values
         var defaultValueProps =
             request().GetType().GetTypeInfo().DeclaredFields.Where(
                 x => null != x.GetCustomAttribute(typeof(DefaultValueAttribute)));
         foreach (var defaultValueProp in defaultValueProps)
         {
            var defaultValue = (DefaultValueAttribute)defaultValueProp.GetCustomAttribute(typeof(DefaultValueAttribute));
            var newRequest = request();

            var smartProp = (ISmartProperty)defaultValueProp.GetValue(newRequest);
            smartProp.SetValue(defaultValue.Value);
            defaultValueProp.SetValue(newRequest, smartProp);

            try
            {
               var testResult = await Rest.GetCandlesAsync(newRequest);
               _results.Verify(result.SequenceEqual(testResult), "Testing default value of " + defaultValueProp.Name);
            }
            catch (Exception ex)
            {
               _results.Verify(false, "Testing default value of " + defaultValueProp.Name + "\n" + ex);
            }
         }
      }
      #endregion
      //---------------------------------------------------------------------------------------------------

      #region Private
      private async Task TestCount(Func<CandlesRequest> request, int count, bool isError, string message)
      {
         var testRequest = request();
         testRequest.count = count;
         if (!isError)
         {
            var result = await Rest.GetCandlesAsync(testRequest);
            _results.Verify(result.Count == count, message);
         }
         else
         {
            var stringResult = await Rest.MakeErrorRequest(testRequest);
            _results.Verify(stringResult != null, message);
         }
      }
      #endregion
   }
}