using MarketMiner.Api.OANDA;
using MarketMiner.Api.Client.OANDA.Data.DataModels;
using MarketMiner.Api.OANDA.TradeLibrary;
using MarketMiner.UI.Common;
using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using MAOC = MarketMiner.Api.Client.OANDA;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace MarketMiner.UI.Data.DataModels
{
   /// <summary>
   /// Base class for <see cref="DataItem"/> and <see cref="DataGroup"/> that
   /// defines properties common to both.
   /// </summary>
   [Windows.Foundation.Metadata.WebHostHidden]
   public abstract class MUIDataCommon : MAOC.Data.DataModels.DataCommon
   {
      public MUIDataCommon() : base()
      { }

      public MUIDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description) 
         : base(uniqueId, title, subtitle, imagePath, description)
      {
      }
   }

   /// <summary>
   /// Generic group data model.
   /// </summary>
   public class MUIDataGroup : MAOC.Data.DataModels.DataGroup
   {
      public MUIDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
         : base(uniqueId, title, subtitle, imagePath, description)
      {
      }

      public override void UpdateItems<T>(List<T> list)
      {
         var dataList = MAOC.Data.Factory.GetDataItems(list, this);
         // Note: it would be nice if this was smarter (updating existing entries rather than always wiping it out)
         CentralDispatcher.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
             {
                base._items.Clear();

                foreach (var item in dataList)
                {
                   var data = item;
                   _items.Add(data);
                }
             }
         );
      }
   }

   public class MUIHistoryDataGroup : MAOC.Data.DataModels.HistoryDataGroup
   {
      public MUIHistoryDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description, int maxEntries)
         : base(uniqueId, title, subtitle, imagePath, description, maxEntries)
      {
      }

      public override void UpdateItems<T>(List<T> list)
      {
         if (base._firstUpdate)
         {
            Items.Clear();
            base._firstUpdate = false;
         }
         var dataList = MAOC.Data.Factory.GetDataItems(list, this);

         // prevent marshalling errors by using the cebtral dispatcher
         CentralDispatcher.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
             {
                for (int t = 0; t < dataList.Count && t < base._maxEntries; t++)
                {
                   Items.Insert(t, dataList[t]);
                   if (Items.Count > base._maxEntries)
                   {
                      Items.RemoveAt(base._maxEntries);
                   }
                }
             }
         );
      }
   }

   public class MUIRatesGroup : MAOC.Data.DataModels.RatesGroup
   {
      DispatcherTimer _transTimer = null;

      public MUIRatesGroup(String uniqueId, String title, String subtitle, String imagePath, String description, AccountDataSource source)
         : base(uniqueId, title, subtitle, imagePath, description, source)
      {
      }

      protected override void EnableUpdates()
      {
         if (_transTimer == null)
         {
            _transTimer = new DispatcherTimer();
            _transTimer.Tick += Refresh;
            _transTimer.Interval = new TimeSpan(0, 0, 0, 1);
            _transTimer.Start();
         }
      }

      protected override void Refresh(object sender, object e)
      {
         _transTimer.Stop();
         Refresh();
      }
   }

   /// <summary>
   /// Creates a collection of groups and items with hard-coded content.
   /// 
   /// DataSource initializes with placeholder data rather than live production
   /// data so that  data is provided at both design-time and run-time.
   /// </summary>
   public sealed class MUIAccountDataSource : MAOC.Data.DataModels.AccountDataSource
   {
      public MUIAccountDataSource()
         : base()
      {
      }

      public MUIAccountDataSource(int accountId)
         : base(accountId)
      {
      }

      DispatcherTimer _transTimer = null;
      private EventsSession _currentSession;
      public override void EnableUpdates()
      {
         if (Credentials.GetDefaultCredentials().HasServer(EServer.StreamingEvents))
         {
            _currentSession = new EventsSession(Id);
            _currentSession.DataReceived += CurrentSessionOnDataReceived;
            InternalRefresh();
            _currentSession.StartSession();
         }
         else
         {	// If we're not getting streamed the notifications, we'll have to poll
            if (_transTimer == null)
            {
               _transTimer = new DispatcherTimer();
               _transTimer.Tick += Refresh;
               _transTimer.Interval = new TimeSpan(0, 0, 0, 1);
               _transTimer.Start();
            }
         }
      }

      protected override void Refresh(object sender, object e)
      {
         _transTimer.Stop();
         Refresh();
      }

      public override async void Refresh()
      {
         await InternalRefresh();
         _transTimer.Start();
      }
   }
}
