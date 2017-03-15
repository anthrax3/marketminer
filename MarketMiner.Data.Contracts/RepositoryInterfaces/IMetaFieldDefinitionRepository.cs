using N.Core.Common.Data;
using P.Core.Common.Meta;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IMetaFieldDefinitionRepository : IDbContextRepository<MetaFieldDefinition>
   {
      MetaFieldDefinition GetFieldDefinition(string tableName, string fieldName, bool enabled = true);
      IEnumerable<MetaFieldDefinition> GetFieldDefinitions(string tableName, bool enabled = true);
   }
}
