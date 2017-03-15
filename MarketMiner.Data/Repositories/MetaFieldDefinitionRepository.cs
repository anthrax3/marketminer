using MarketMiner.Data.Contracts;
using P.Core.Common.Meta;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IMetaFieldDefinitionRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class MetaFieldDefinitionRepository : MarketMinerRepositoryBase<MetaFieldDefinition>, IMetaFieldDefinitionRepository
   {
      #region Members.Override
      protected override MetaFieldDefinition AddEntity(MarketMinerContext entityContext, MetaFieldDefinition entity)
      {
         return entityContext.MetaFieldDefinitionSet.Add(entity);
      }

      protected override MetaFieldDefinition UpdateEntity(MarketMinerContext entityContext, MetaFieldDefinition entity)
      {
         return (from e in entityContext.MetaFieldDefinitionSet
                 where e.MetaFieldDefinitionID == entity.MetaFieldDefinitionID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<MetaFieldDefinition> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.MetaFieldDefinitionSet
                select e;
      }

      protected override MetaFieldDefinition GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.MetaFieldDefinitionSet
                      where e.MetaFieldDefinitionID == id
                      select e);

         var result = query.FirstOrDefault();

         return result;
      }
      #endregion

      #region Members.IMetaFieldDefinitionRepository
      public MetaFieldDefinition GetFieldDefinition(string tableName, string fieldName, bool enabled = true)
      {
         var fieldDefinition = GetFieldDefinitions(tableName, enabled).Where(x => x.FieldName == fieldName).FirstOrDefault();

         return fieldDefinition;
      }

      public IEnumerable<MetaFieldDefinition> GetFieldDefinitions(string tableName, bool enabled = true)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var fieldDefinitions = GetEntities(entityContext).Where(x => x.TableName == tableName && x.Enabled == enabled);

            return fieldDefinitions;
         }
      }
      #endregion
   }
}
