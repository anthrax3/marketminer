using MarketMiner.Algorithm.Demo;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketMiner.Host.SignalR
{
   public class CurrencyExchangeHub : Hub
   {
      private readonly MarketMinerSignalR _mmService;

      #region Constructors
      public CurrencyExchangeHub() : this(MarketMinerSignalR.Instance)
      {

      }

      public CurrencyExchangeHub(MarketMinerSignalR mmService)
      {
         _mmService = mmService;
      }
      #endregion


      public IEnumerable<Currency> GetAllCurrencies()
      {
         return _mmService.GetAllCurrencies();
      }

      public async Task<IEnumerable<Currency>> GetAllCurrenciesAsync()
      {
         IEnumerable<Currency> currencies = new List<Currency>();
         Task loadCurrenciesTask = Task.Factory.StartNew(() => LoadCurrencies(currencies));
         await loadCurrenciesTask;

         return currencies;
      }
      private void LoadCurrencies(IEnumerable<Currency> currencies)
      {
         currencies = _mmService.GetAllCurrencies();
      }

      public string GetMarketState()
      {
         return _mmService.MarketState.ToString();
      }

      public bool OpenMarket()
      {
         _mmService.OpenMarket();
         return true;
      }

      public bool CloseMarket()
      {
         _mmService.CloseMarket();
         return true;
      }

      public bool Reset()
      {
         _mmService.Reset();
         return true;
      }
   }
}