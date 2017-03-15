using N.Core.Common.Data;
using P.Core.Common.Meta;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IMetaResourceRepository : IDbContextRepository<MetaResource>
   {
      MetaResource GetResource(string set, string type, string key, string cultureCode = "en-US", bool enabled = true);
      IEnumerable<MetaResource> GetResources(string set, bool enabledOnly = true);
      IEnumerable<MetaResource> GetResources(string set, string type, bool enabledOnly = true);
      IEnumerable<MetaResource> GetResourcesByCulture(string set, string cultureCode = "en-US", bool enabledOnly = true);
      IEnumerable<MetaResource> GetResourcesByCulture(string set, string type, string cultureCode = "en-US", bool enabledOnly = true);
   }
}
