using N.Core.Common.Data;
using P.Core.Common.Meta;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IMetaSettingRepository : IDbContextRepository<MetaSetting>
   {
      MetaSetting GetSetting(string type, string code, bool enabled = true);
      IEnumerable<MetaSetting> GetSettings(string type, bool enabled = true);
   }
}
