using N.Core.Common.Core;
using System;

namespace MarketMiner.Client.Entities
{
   public class Signal : ObjectBase
   {
      int _signalID;
      int _productID;
      int? _strategyTransactionID;
      string _type;
      string _instrument;
      string _granularity;
      string _side;
      double _signalPrice;
      string _signalTime;
      DateTime? _sendPostmark;
      bool _active;

      #region Properties
      public int SignalID
      {
         get { return _signalID; }
         set { if (_signalID != value) { _signalID = value; OnPropertyChanged(); } }
      }

      public int ProductID
      {
         get { return _productID; }
         set { if (_productID != value) { _productID = value; OnPropertyChanged(); } }
      }

      public int? StrategyTransactionID
      {
         get { return _strategyTransactionID; }
         set { if (_strategyTransactionID != value) { _strategyTransactionID = value; OnPropertyChanged(); } }
      }

      public string Type
      {
         get { return _type; }
         set { if (_type != value) { _type = value; OnPropertyChanged(); } }
      }

      public string Instrument
      {
         get { return _instrument; }
         set { if (_instrument != value) { _instrument = value; OnPropertyChanged(); } }
      }

      public string Granularity
      {
         get { return _granularity; }
         set { if (_granularity != value) { _granularity = value; OnPropertyChanged(); } }
      }

      public string Side
      {
         get { return _side; }
         set { if (_side != value) { _side = value; OnPropertyChanged(); } }
      }

      public double SignalPrice
      {
         get { return _signalPrice; }
         set { if (_signalPrice != value) { _signalPrice = value; OnPropertyChanged(); } }
      }

      public string SignalTime
      {
         get { return _signalTime; }
         set { if (_signalTime != value) { _signalTime = value; OnPropertyChanged(); } }
      }

      public DateTime? SendPostmark
      {
         get { return _sendPostmark; }
         set { if (_sendPostmark != value) { _sendPostmark = value; OnPropertyChanged(); } }
      }

      public bool Active
      {
         get { return _active; }
         set { if (_active != value) { _active = value; OnPropertyChanged(); } }
      }
      #endregion
   }
}
