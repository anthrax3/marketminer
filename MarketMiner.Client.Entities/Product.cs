using N.Core.Common.Core;

namespace MarketMiner.Client.Entities
{
   public class Product : ObjectBase
   {
      int _productID { get; set; }
      string _name { get; set; }
      string _code { get; set; }
      string _shortDesc { get; set; }
      string _longDesc { get; set; }
      bool _active { get; set; }

      #region Properties
      public int ProductID
      {
         get { return _productID; }
         set { if (_productID != value) { _productID = value; OnPropertyChanged(); } }
      }

      public string Name
      {
         get { return _name; }
         set { if (_name != value) { _name = value; OnPropertyChanged(); } }
      }

      public string Code
      {
         get { return _code; }
         set { if (_code != value) { _code = value; OnPropertyChanged(); } }
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

      public bool Active
      {
         get { return _active; }
         set { if (_active != value) { _active = value; OnPropertyChanged(); } }
      }
      #endregion
   }
}
