using N.Core.Common.Core;

namespace MarketMiner.Client.Entities
{
   public class Participation : ObjectBase
   {
      int _participationID;
      int _accountID;
      double _initialBalance;
      double _currentBalance;
      short _fundID;

      #region Properties
      public int ParticipationId
      {
         get { return _participationID; }
         set { if (_participationID != value) { _participationID = value; OnPropertyChanged(); } }
      }

      public int AccountId
      {
         get { return _accountID; }
         set { if (_accountID != value) { _accountID = value; OnPropertyChanged(); } }
      }

      public double InitialBalance
      {
         get { return _initialBalance; }
         set { if (_initialBalance != value) { _initialBalance = value; OnPropertyChanged(); } }
      }

      public double CurrentBalance
      {
         get { return _currentBalance; }
         set { if (_currentBalance != value) { _currentBalance = value; OnPropertyChanged(); } }
      }

      public short FundID
      {
         get { return _fundID; }
         set { if (_fundID != value) { _fundID = value; OnPropertyChanged(); } }
      }
      #endregion
   }
}
