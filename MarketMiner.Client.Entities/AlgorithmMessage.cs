using N.Core.Common.Core;
using System;
using System.Diagnostics;

namespace MarketMiner.Client.Entities
{
   public class AlgorithmMessage : ObjectBase
   {
      int _algorithmMessageID;
      int _algorithmInstanceID;
      string _message;
      TraceEventType _severity;
      Exception _exception;

      #region Properties
      public int AlgorithmMessageID
      {
         get { return _algorithmMessageID; }
         set { if (_algorithmMessageID != value) { _algorithmMessageID = value; OnPropertyChanged(); } }
      }

      public int AlgorithmInstanceID
      {
         get { return _algorithmInstanceID; }
         set { if (_algorithmInstanceID != value) { _algorithmInstanceID = value; OnPropertyChanged(); } }
      }

      public string Message
      {
         get { return _message; }
         set { if (_message != value) { _message = value; OnPropertyChanged(); } }
      }

      public TraceEventType Severity
      {
          get { return _severity; }
          set { if (_severity != value) { _severity = value; OnPropertyChanged(); } }
      }

      public Exception Exception
      {
         get { return _exception; }
         set { if (_exception != value) { _exception = value; OnPropertyChanged(); } }
      }
      #endregion
   }
}
