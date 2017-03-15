using MarketMiner.Api.OANDA.REST.TradeLibrary;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MarketMiner.Api.OANDA.Extensions.Classes
{
   public class MarketMinerEventsSession : EventsSession
   {
      public MarketMinerEventsSession(int accountId)
         : base(accountId)
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
            OnSessionStatusChanged(false, new Exception("MarketMinerEventsSession did not get an events session.", e));
         }

         Task.Run(() =>
         {
            try
            {
               DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Event));
               StreamReader reader = new StreamReader(_response.GetResponseStream());
               while (!_shutdown)
               {
                  MemoryStream memStream = new MemoryStream();

                  string line = reader.ReadLine();
                  memStream.Write(Encoding.UTF8.GetBytes(line), 0, Encoding.UTF8.GetByteCount(line));
                  memStream.Position = 0;

                  var data = (Event)serializer.ReadObject(memStream);

                  OnSessionStatusChanged(true, null);

                  OnDataReceived(data);

                  //// Don't send heartbeats
                  //if (!data.IsHeartbeat())
                  //{
                  //   OnDataReceived(data);
                  //}             
               }
            }
            catch (Exception e)
            {
               _shutdown = true;
               OnSessionStatusChanged(false, new Exception("MarketMinerEventsSession could not start events session.", e));
            }
         });
      }
   }
}
