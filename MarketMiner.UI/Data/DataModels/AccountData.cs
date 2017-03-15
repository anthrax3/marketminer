using System;
using Windows.UI.Xaml;
using MAOC = MarketMiner.Api.Client.OANDA;

namespace MarketMiner.UI.Data.DataModels
{
   public class MUIAccountData : MAOC.Data.DataModels.AccountData
   {
      DispatcherTimer _transTimer;

      public MUIAccountData(int id) : base(id) { }

      public override void EnableUpdates()
      {
         if (_transTimer == null)
         {
            _transTimer = new DispatcherTimer();
            _transTimer.Tick += base.Refresh;
            _transTimer.Interval = new TimeSpan(0, 0, 0, 1);
            _transTimer.Start();
         }
      }
   }
}
