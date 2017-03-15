using N.Core.Common.Data;
using P.Core.Common.Meta;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IMetaLookupRepository : IDbContextRepository<MetaLookup>
   {
      MetaLookup GetLookup(string type, string code, bool enabled = true);
      IEnumerable<MetaLookup> GetLookups(string type, bool enabledOnly = true);
   }
}
