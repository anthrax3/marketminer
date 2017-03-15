using N.Core.Common.Core;
using System.Runtime.Serialization;

namespace MarketMiner.Client.Entities
{
   public class Subscription : ObjectBase
   {
      int _subscriptionID;
      int _productID;
      int _accountID;
      bool _active;
      Account _account;

      #region Properties
      public int SubscriptionID 
      { 
         get { return _subscriptionID; }
         set { if (_subscriptionID != value) { _subscriptionID = value; OnPropertyChanged(); } }
      }

      public int ProductID
      {
         get { return _productID; }
         set { if (_productID != value) { _productID = value; OnPropertyChanged(); } }
      }

      public int AccountID
      {
         get { return _accountID; }
         set { if (_accountID != value) { _accountID = value; OnPropertyChanged(); } }
      }

      public bool Active
      {
         get { return _active; }
         set { if (_active != value) { _active = value; OnPropertyChanged(); } }
      }

      public virtual Account Account
      {
         get { return _account; }
         set { if (_account != value) { _account = value; OnPropertyChanged(); } }
      }
      #endregion
   }
}
