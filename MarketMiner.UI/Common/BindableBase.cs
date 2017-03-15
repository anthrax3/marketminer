using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using MAOC = MarketMiner.Api.Client.OANDA;

namespace MarketMiner.UI.Common
{
   /// <summary>
   /// Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
   /// </summary>
   [Windows.Foundation.Metadata.WebHostHidden]
   public abstract class MUIBindableBase : MAOC.Common.BindableBase
   {
      /// <summary>
      /// Notifies listeners that a property value has changed.
      /// </summary>
      /// <param name="propertyName">Name of the property used to notify listeners.  This
      /// value is optional and can be provided automatically when invoked from compilers
      /// that support <see cref="CallerMemberNameAttribute"/>.</param>
      protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         Delegate[] handlers = base.PropertyChangedSubscribers();
         if (handlers.Length > 0)
         {
            // Dispatch property change notifications to the primary UI thread
            CentralDispatcher.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                  foreach (PropertyChangedEventHandler handler in handlers)
                  {
                     handler(this, new PropertyChangedEventArgs(propertyName));
                  }
               });
         }
      }
   }
}
