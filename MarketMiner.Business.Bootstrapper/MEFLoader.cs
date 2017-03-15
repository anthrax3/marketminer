using MarketMiner.Data.Repositories;
using N.Core.Common.Core;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace MarketMiner.Business.Bootstrapper
{
   public static class MEFLoader
   {
      public static CompositionContainer Init()
      {
         return Init(null);
      }

      public static CompositionContainer Init(ICollection<ComposablePartCatalog> catalogParts)
      {
         AggregateCatalog catalog = new AggregateCatalog();

         catalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountRepository).Assembly));
         catalog.Catalogs.Add(new AssemblyCatalog(typeof(StrategyEngine).Assembly));
         catalog.Catalogs.Add(new AssemblyCatalog(typeof(MicrosoftPracticesLogger).Assembly));

         if (catalogParts != null)  // discover objects in passed in assembly catalog
            foreach (var part in catalogParts)
               catalog.Catalogs.Add(part);

         CompositionContainer container = new CompositionContainer(catalog);
         return container;
      }
   }
}
