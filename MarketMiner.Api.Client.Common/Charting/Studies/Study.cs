using P.Core.Common.Core;
namespace MarketMiner.Api.Client.Common.Charting
{
   public abstract class Study : NotificationObject, IStudy
   {
      public Study(StudyType type)
      {
         Type = type;
      }

      public StudyType Type { get; set; }
      public int Ordinal { get; set; }

      //public int? FillZoneReachedIndex { get { return _fillZoneReachedIndex; } set { if (_fillZoneReachedIndex != value) { _fillZoneReachedIndex = value; OnPropertyChanged(); } } }
      //public int? TakeProfitZoneReachedIndex { get { return _takeProfitZoneReachedIndex; } set { if (_takeProfitZoneReachedIndex != value) { _takeProfitZoneReachedIndex = value; OnPropertyChanged(); } } }
   }
}
