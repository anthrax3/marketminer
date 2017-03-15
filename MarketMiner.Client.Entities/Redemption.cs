using N.Core.Common.Core;
using System;

namespace MarketMiner.Client.Entities
{
   public class Redemption : ObjectBase
   {
      int _redemptionID;
      int _amount;
      DateTime _datePaid;
      int? _accountID;
      int _participationID;
      Account _account;
      Participation _participation;

      #region Properties
      public int RedemptionID
      {
         get { return _redemptionID; }
         set { if (_redemptionID != value) { _redemptionID = value; OnPropertyChanged(); } }
      }

      public int Amount
      {
         get { return _amount; }
         set { if (_amount != value) { _amount = value; OnPropertyChanged(); } }
      }

      public DateTime DatePaid
      {
         get { return _datePaid; }
         set { if (_datePaid != value) { _datePaid = value; OnPropertyChanged(); } }
      }

      public int? AccountID
      {
         get { return _accountID; }
         set { if (_accountID != value) { _accountID = value; OnPropertyChanged(); } }
      }

      public int ParticipationID
      {
         get { return _participationID; }
         set { if (_participationID != value) { _participationID = value; OnPropertyChanged(); } }
      }

      public virtual Account Account
      {
         get { return _account; }
         set { if (_account != value) { _account = value; OnPropertyChanged(); } }
      }

      public virtual Participation Participation
      {
         get { return _participation; }
         set { if (_participation != value) { _participation = value; OnPropertyChanged(); } }
      }
      #endregion
   }
}
