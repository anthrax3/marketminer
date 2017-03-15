using N.Core.Common.Core;
using System;
using System.Collections.ObjectModel;

namespace MarketMiner.Client.Entities
{
   public class Fund : ObjectBase
   {
      int _fundID;
      DateTime _openDate;
      DateTime _closeDate;
      bool _openToNew;
      bool _openToAdd;
      bool _openToRedeem;
      ObservableCollection<Strategy> _strategies;
      ObservableCollection<Participation> _participations;

      #region Properties
      public int FundID
      {
         get { return _fundID; }
         set { if (_fundID != value) { _fundID = value; OnPropertyChanged(); } }
      }

      public DateTime OpenDate
      {
         get { return _openDate; }
         set { if (_openDate != value) { _openDate = value; OnPropertyChanged(); } }
      }

      public DateTime CloseDate
      {
         get { return _closeDate; }
         set { if (_closeDate != value) { _closeDate = value; OnPropertyChanged(); } }
      }

      public bool OpenToNew
      {
         get { return _openToNew; }
         set { if (_openToNew != value) { _openToNew = value; OnPropertyChanged(); } }
      }

      public bool OpenToAdd
      {
         get { return _openToAdd; }
         set { if (_openToAdd != value) { _openToAdd = value; OnPropertyChanged(); } }
      }

      public bool OpenToRedeem
      {
         get { return _openToRedeem; }
         set { if (_openToRedeem != value) { _openToRedeem = value; OnPropertyChanged(); } }
      }

      public ObservableCollection<Strategy> Strategies
      {
         get { return _strategies; }
         set { if (_strategies != value) { _strategies = value; OnPropertyChanged(); } }
      }

      public ObservableCollection<Participation> Participations
      {
         get { return _participations; }
         set { if (_participations != value) { _participations = value; OnPropertyChanged(); } }
      }
      #endregion
   }
}
