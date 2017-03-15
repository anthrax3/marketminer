using MarketMiner.Api.OANDA.REST.TradeLibrary;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes.Communications.Requests;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketMiner.Api.OANDA.Extensions
{
   public static class Helpers
   {
      public static async Task<bool> IsMarketHalted(string instr = Constants.Instruments.EURUSD)
      {
         var accountId = Credentials.GetDefaultCredentials().DefaultAccountId;
         var instruments = await Rest.GetInstrumentsAsync(accountId);
         var test = instruments.Where(x => x.instrument == instr).ToList();
         var rates = await Rest.GetRatesAsync(test);
         return rates[0].status == "halted";
      }

      public static double GranularityToSeconds(string granularity, int? year = null, int? month = null)
      {
         switch (granularity)
         {
            case Constants.Granularity.Second05: return 5;
            case Constants.Granularity.Second10: return 10;
            case Constants.Granularity.Second15: return 15;
            case Constants.Granularity.Second30: return 30;
            case Constants.Granularity.Minute01: return 60;
            case Constants.Granularity.Minute02: return 120;
            case Constants.Granularity.Minute03: return 180;
            case Constants.Granularity.Minute05: return 300;
            case Constants.Granularity.Minute10: return 600;
            case Constants.Granularity.Minute15: return 900;
            case Constants.Granularity.Minute30: return 1800;
            case Constants.Granularity.Hour01: return 3600;
            case Constants.Granularity.Hour02: return 3600 * 2;
            case Constants.Granularity.Hour03: return 3600 * 3;
            case Constants.Granularity.Hour04: return 3600 * 4;
            case Constants.Granularity.Hour06: return 3600 * 6;
            case Constants.Granularity.Hour08: return 3600 * 8;
            case Constants.Granularity.Hour12: return 3600 * 12;
            case Constants.Granularity.Day: return 3600 * 24;
            case Constants.Granularity.Week: return 3600 * 24 * 7;
            case Constants.Granularity.Month:
               if (year == null)
                  throw new ArgumentException("Year is required.");
               else if (month == null)
                  throw new ArgumentException("Month name is required.");
               else
                  return DateTime.DaysInMonth(year.Value, month.Value) * 24 * 60 * 60;
            default: throw new ArgumentException(string.Format("Invalid OANDA granularity: ", granularity));
         }
      }

      public static string AsString(this EGranularity granularity)
      {
         switch (granularity)
         {
            case EGranularity.S5: return Constants.Granularity.Second05;
            case EGranularity.S10: return Constants.Granularity.Second10;
            case EGranularity.S15: return Constants.Granularity.Second15;
            case EGranularity.S30: return Constants.Granularity.Second30;
            case EGranularity.M1: return Constants.Granularity.Minute01;
            case EGranularity.M2: return Constants.Granularity.Minute02;
            case EGranularity.M3: return Constants.Granularity.Minute03;
            case EGranularity.M5: return Constants.Granularity.Minute05;
            case EGranularity.M10: return Constants.Granularity.Minute10;
            case EGranularity.M15: return Constants.Granularity.Minute15;
            case EGranularity.M30: return Constants.Granularity.Minute30;
            case EGranularity.H1: return Constants.Granularity.Hour01;
            case EGranularity.H2: return Constants.Granularity.Hour02;
            case EGranularity.H3: return Constants.Granularity.Hour03;
            case EGranularity.H4: return Constants.Granularity.Hour04;
            case EGranularity.H6: return Constants.Granularity.Hour06;
            case EGranularity.H8: return Constants.Granularity.Hour08;
            case EGranularity.H12: return Constants.Granularity.Hour12;
            case EGranularity.D: return Constants.Granularity.Day;
            case EGranularity.W: return Constants.Granularity.Week;
            case EGranularity.M: return Constants.Granularity.Month;
            default: return "Invalid EGranularity";
         }
      }


      public static EGranularity AsEGranularity(this string granularity)
      {
         switch (granularity)
         {
            case Constants.Granularity.Second05: return EGranularity.S5;
            case Constants.Granularity.Second10: return EGranularity.S10;
            case Constants.Granularity.Second15: return EGranularity.S15;
            case Constants.Granularity.Second30: return EGranularity.S30;
            case Constants.Granularity.Minute01: return EGranularity.M1;
            case Constants.Granularity.Minute02: return EGranularity.M2;
            case Constants.Granularity.Minute03: return EGranularity.M3;
            case Constants.Granularity.Minute05: return EGranularity.M5;
            case Constants.Granularity.Minute10: return EGranularity.M10;
            case Constants.Granularity.Minute15: return EGranularity.M15;
            case Constants.Granularity.Minute30: return EGranularity.M30;
            case Constants.Granularity.Hour01: return EGranularity.H1;
            case Constants.Granularity.Hour02: return EGranularity.H2;
            case Constants.Granularity.Hour03: return EGranularity.H3;
            case Constants.Granularity.Hour04: return EGranularity.H4;
            case Constants.Granularity.Hour06: return EGranularity.H6;
            case Constants.Granularity.Hour08: return EGranularity.H8;
            case Constants.Granularity.Hour12: return EGranularity.H12;
            case Constants.Granularity.Day: return EGranularity.D;
            case Constants.Granularity.Week: return EGranularity.W;
            case Constants.Granularity.Month: return EGranularity.M;
            default: return EGranularity.S5;
         }
      }
   }
}
