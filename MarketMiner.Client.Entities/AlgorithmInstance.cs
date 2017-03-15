using N.Core.Common.Core;
using System;
using System.Collections.ObjectModel;

namespace MarketMiner.Client.Entities
{
   public class AlgorithmInstance : ObjectBase
   {
      int _algorithmInstanceID;
      int _strategyID;
      short? _status;
      DateTime? _firstTradeDateTime;
      DateTime? _lastTradeDateTime;
      DateTime? _runStartDateTime;
      DateTime? _runStopDateTime;

      #region Properties
      public int AlgorithmInstanceID 
      { 
         get { return _algorithmInstanceID; }
         set { if (_algorithmInstanceID != value) { _algorithmInstanceID = value; OnPropertyChanged(); } }
      }

      public int StrategyID
      {
         get { return _strategyID; }
         set { if (_strategyID != value) { _strategyID = value; OnPropertyChanged(); } }
      }

      public short? Status
      {
         get { return _status; }
         set { if (_status != value) { _status = value; OnPropertyChanged(); } }
      }

      public DateTime? FirstTradeDateTime
      {
         get { return _firstTradeDateTime; }
         set
         {
            if (_firstTradeDateTime != value)
            {
               _firstTradeDateTime = value;
               OnPropertyChanged();
            }
         }
      }

      public DateTime? LastTradeDateTime
      {
         get { return _lastTradeDateTime; }
         set
         {
            if (_lastTradeDateTime != value)
            {
               _lastTradeDateTime = value;
               OnPropertyChanged();
            }
         }
      }

      public DateTime? RunStartDateTime
      {
         get { return _runStartDateTime; }
         set
         {
            if (_runStartDateTime != value)
            {
               _runStartDateTime = value;
               OnPropertyChanged();
            }
         }
      }

      public DateTime? RunStopDateTime
      {
         get { return _runStopDateTime; }
         set { if (_runStopDateTime != value) { _runStopDateTime = value; OnPropertyChanged(); } }
      }
      #endregion
   }
}
