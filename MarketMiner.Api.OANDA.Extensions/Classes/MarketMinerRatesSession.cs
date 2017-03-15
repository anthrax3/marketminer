using MarketMiner.Api.OANDA.REST.TradeLibrary;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MarketMiner.Api.OANDA.Extensions.Classes
{
   public class MarketMinerRatesSession : RatesSession
   {
      public MarketMinerRatesSession(int accountId, List<Instrument> instruments)
         : base(accountId, instruments)
      {
         _shutdown = true;
      }

      public override async void StartSession()
      {
         _shutdown = false;

         try
         {
            _response = await GetSession();
         }
         catch (Exception e)
         {
            _shutdown = true;
            OnSessionStatusChanged(false, new Exception("MarketMinerRatesSession did not get a rate session.", e));
         }

         Task.Run(() =>
         {
            try
            {
               DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RateStreamResponse));
               StreamReader reader = new StreamReader(_response.GetResponseStream());
               while (!_shutdown)
               {
                  MemoryStream memStream = new MemoryStream();

                  string line = reader.ReadLine();
                  memStream.Write(Encoding.UTF8.GetBytes(line), 0, Encoding.UTF8.GetByteCount(line));
                  memStream.Position = 0;

                  var data = (RateStreamResponse)serializer.ReadObject(memStream);

                  OnSessionStatusChanged(true, null);

                  OnDataReceived(data);
               }
            }
            catch (Exception e)
            {
               _shutdown = true;
               OnSessionStatusChanged(false, new Exception("MarketMinerRatesSession could not start a rate session.", e));
            }
         });
      }
   }
}
