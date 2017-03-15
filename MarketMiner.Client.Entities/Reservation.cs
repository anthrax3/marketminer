using N.Core.Common.Core;

namespace MarketMiner.Client.Entities
{
   public class Reservation : ObjectBase
   {
      int _reservationID;
      int _accountID;
      double _amount;
      bool _open;
      bool _cancelled;

      #region Properties
      public int ReservationID
      {
         get { return _reservationID; }
         set { if (_reservationID != value) { _reservationID = value; OnPropertyChanged(); } }
      }
      public int AccountID
      {
         get { return _accountID; }
         set { if (_accountID != value) { _accountID = value; OnPropertyChanged(); } }
      }
      public double Amount    
      {
         get { return _amount; }
         set { if (_amount != value) { _amount = value; OnPropertyChanged(); } }
      }  
      public bool Open   
      {
         get { return _open; }
         set { if (_open != value) { _open = value; OnPropertyChanged(); } }
      }   
      public bool Cancelled    
      {
         get { return _cancelled; }
         set { if (_cancelled != value) { _cancelled = value; OnPropertyChanged(); } }
      }  
      #endregion
   }
}
