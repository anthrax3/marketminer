using N.Core.Common.Core;

namespace MarketMiner.Client.Entities
{
   public class AlgorithmParameter : ObjectBase
   {
      int _algorithmParameterID;
      int _strategyID;
      string _parameterName;
      string _parameterValue;
      string _parameterType;

      #region Properties
      public int AlgorithmParameterID
      { 
         get { return _algorithmParameterID; }
         set { if (_algorithmParameterID != value) { _algorithmParameterID = value; OnPropertyChanged(); } }
      }

      public int StrategyID
      {
         get { return _strategyID; }
         set { if (_strategyID != value) { _strategyID = value; OnPropertyChanged(); } }
      }

      public string ParameterName
      {
         get { return _parameterName; }
         set { if (_parameterName != value) { _parameterName = value; OnPropertyChanged(); } }
      }

      public string ParameterValue
      {
         get { return _parameterValue; }
         set { if (_parameterValue != value) { _parameterValue = value; OnPropertyChanged(); } }
      }

      public string ParameterType
      {
         get { return _parameterType; }
         set { if (_parameterType != value) { _parameterType = value; OnPropertyChanged(); } }
      }
      #endregion
   }
}
