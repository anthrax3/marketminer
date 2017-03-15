using N.Core.Common.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MarketMiner.Client.Entities
{
   public class Strategy : ObjectBase
   {
      public Strategy()
      {
         _transactions = new ObservableCollection<StrategyTransaction>();
      }

      #region Declarations
      int _strategyID;
      string _name;
      string _shortDesc;
      string _longDesc;
      decimal _initialAUM;
      decimal _currentAUM;
      decimal _maximumAUM;
      string _algorithmClass;
      string _algorithmAssembly;
      int _fundID;
      Fund _fund;
      List<AlgorithmParameter> _algorithmParameters;
      ObservableCollection<StrategyTransaction> _transactions;
      ObservableCollection<AlgorithmMessage> _messages;
      #endregion

      #region Properties
      public int StrategyID
      {
         get { return _strategyID; }
         set { if (_strategyID != value) { _strategyID = value; OnPropertyChanged(); } }
      }

      public string Name
      {
         get { return _name; }
         set { if (_name != value) { _name = value; OnPropertyChanged(); } }
      }

      public string ShortDesc
      {
         get { return _shortDesc; }
         set { if (_shortDesc != value) { _shortDesc = value; OnPropertyChanged(); } }
      }

      public string LongDesc
      {
         get { return _longDesc; }
         set { if (_longDesc != value) { _longDesc = value; OnPropertyChanged(); } }
      }

      public decimal InitialAUM
      {
         get { return _initialAUM; }
         set { if (_initialAUM != value) { _initialAUM = value; OnPropertyChanged(); } }
      }

      public decimal CurrentAUM
      {
         get { return _currentAUM; }
         set { if (_currentAUM != value) { _currentAUM = value; OnPropertyChanged(); } }
      }

      public decimal MaximumAUM
      {
         get { return _maximumAUM; }
         set { if (_maximumAUM != value) { _maximumAUM = value; OnPropertyChanged(); } }
      }

      public string AlgorithmClass
      {
         get { return _algorithmClass; }
         set { if (_algorithmClass != value) { _algorithmClass = value; OnPropertyChanged(); } }
      }

      public string AlgorithmAssembly
      {
         get { return _algorithmAssembly; }
         set { if (_algorithmAssembly != value) { _algorithmAssembly = value; OnPropertyChanged(); } }
      }

      public int FundID
      {
         get { return _fundID; }
         set { if (_fundID != value) { _fundID = value; OnPropertyChanged(); } }
      }

      public Fund Fund
      {
         get { return _fund; }
         set { if (_fund != value) { _fund = value; OnPropertyChanged(); } }
      }

      public List<AlgorithmParameter> AlgorithmParameters
      {
         get { return _algorithmParameters; }
         set { if (_algorithmParameters != value) { _algorithmParameters = value; OnPropertyChanged(); } }
      }

      public ObservableCollection<StrategyTransaction> Transactions 
      { 
         get { return _transactions; }
         set { if (_transactions != value) { _transactions = value; OnPropertyChanged(); } }
      }

      public ObservableCollection<AlgorithmMessage> Messages
      {
         get { return _messages; }
         set { if (_messages != value) { _messages = value; OnPropertyChanged(); } }
      }
      #endregion
   }
}
