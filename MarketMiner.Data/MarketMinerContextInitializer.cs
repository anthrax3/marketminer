using System.Data.Entity.Migrations;

namespace MarketMiner.Data
{
   /// <summary>
   /// Used to initialize the MarketMinerContext
   /// </summary>
   public static class MarketMinerContextInitializer
   {
      public static void Init()
      {
         var migrator = new DbMigrator(new MarketMiner.Data.Migrations.Configuration());
         migrator.Update();
      }
   }
}
