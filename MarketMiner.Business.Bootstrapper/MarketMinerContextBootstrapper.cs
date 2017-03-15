using MarketMiner.Data;

namespace MarketMiner.Business.Bootstrapper
{
   /// <summary>
   /// Used to execute migrations and initialize the MarketMinerContext
   /// </summary>
   public static class MarketMinerContextBootstrapper
   {
      public static void Init()
      {
         MarketMinerContextInitializer.Init();
      }
   }
}
