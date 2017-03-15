using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.Common.Contracts;
using P.Core.Common.Core;
using System;

namespace MarketMiner.Api.Client.Common.Patterns
{
   public abstract class Pattern : NotificationObject, IPattern
   {
      #region Declarations
      int _signalID;
      string _type;
      string _instrument;
      string _granularity;
      EPatternDirection _direction;
      string _side;
      double _signalPrice;
      string _signalTime;
      DateTime? _sendPostmark;
      bool _active;
      int? _strategyTransactionID;
      IStudy _study;
      #endregion

      public Pattern()
      {
         _direction = EPatternDirection.None;
      }

      #region Properties
      public int SignalID { get { return _signalID; } set { if (_signalID != value) { _signalID = value; OnPropertyChanged(); } } }
      public int? StrategyTransactionID { get { return _strategyTransactionID; } set { if (_strategyTransactionID != value) { _strategyTransactionID = value; OnPropertyChanged(); } } }
      public DateTime? SendPostmark { get { return _sendPostmark; } set { if (_sendPostmark != value) { _sendPostmark = value; OnPropertyChanged(); } } }
      #endregion

      #region Members.IPattern
      public string Type { get { return _type; } set { if (_type != value) { _type = value; OnPropertyChanged(); } } }
      public string Instrument { get { return _instrument; } set { if (_instrument != value) { _instrument = value; OnPropertyChanged(); } } }
      public string Granularity { get { return _granularity; } set { if (_granularity != value) { _granularity = value; OnPropertyChanged(); } } }
      public EPatternDirection Direction { get { return _direction; } set { if (_direction != value) { _direction = value; OnPropertyChanged(); } } }
      public string Side { get { return _side; } set { if (_side != value) { _side = value; OnPropertyChanged(); } } }
      public double SignalPrice { get { return _signalPrice; } set { if (_signalPrice != value) { _signalPrice = value; OnPropertyChanged(); } } }
      public string SignalTime { get { return _signalTime; } set { if (_signalTime != value) { _signalTime = value; OnPropertyChanged(); } } }
      public IStudy Study { get { return _study; } set { if (_study != value) { _study = value; OnPropertyChanged(); } } }
      public bool Active { get { return _active; } set { if (_active != value) { _active = value; OnPropertyChanged(); } } }
      #endregion

      public virtual string GetProductName()
      {
         string productName = string.Format("Signal.{0}.{1}.{2}", _instrument.Replace("_", ""), _type, _granularity);
         return productName;
      }
   }
}
