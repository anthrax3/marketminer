using N.Core.Common.Core;

namespace MarketMiner.Client.Entities
{
   public class Broker : ObjectBase   
   {
      int _brokerID;
      string _name;

      public int BrokerID
      {
         get { return _brokerID; }
         set { if (_brokerID != value) _brokerID = value; OnPropertyChanged(); }
      }

      public string Name
      {
         get { return _name; }
         set { if (_name != value) _name = value; OnPropertyChanged(); }
      }
   }
}
