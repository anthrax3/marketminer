using MarketMiner.Common;
using MarketMiner.Data.Contracts;
using P.Core.Common.Faults;
using P.Core.Common.Meta;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using P.Core.Common.Extensions;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IMetaSettingRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class MetaSettingRepository : MarketMinerRepositoryBase<MetaSetting>, IMetaSettingRepository
   {
      #region Members.Override
      protected override MetaSetting AddEntity(MarketMinerContext entityContext, MetaSetting entity)
      {
         return entityContext.MetaSettingSet.Add(entity);
      }

      protected override MetaSetting UpdateEntity(MarketMinerContext entityContext, MetaSetting entity)
      {
         return (from e in entityContext.MetaSettingSet
                 where e.MetaSettingID == entity.MetaSettingID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<MetaSetting> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.MetaSettingSet
                select e;
      }

      protected override MetaSetting GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.MetaSettingSet
                      where e.MetaSettingID == id
                      select e);

         var result = query.FirstOrDefault();

         return result;
      }
      #endregion

      #region Members.IMetaSettingRepository
      public IEnumerable<MetaSetting> GetSettings(string type, bool enabled = true)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var defaults = from s in entityContext.MetaSettingSet
                           where s.Environment == Constants.MetaSettings.Environments.Default
                              && s.Type == type
                              && s.Enabled == enabled
                           select s;

            var environments = from s in entityContext.MetaSettingSet
                               where s.Environment == _environment
                                  && s.Type == type
                                  && s.Enabled == enabled
                               select s;

            // remove defaults with environment-specific equivalents
            defaults.ToList().RemoveAll(d => environments.FirstOrDefault(e => e.Type == d.Type && e.Code == d.Code) != null);
            environments = environments.Concat(defaults);

            return environments.ToFullyLoaded();
         }
      }

      public MetaSetting GetSetting(string type, string code, bool enabled = true)
      {
         IEnumerable<MetaSetting> settings = GetSettings(type, enabled).Where(s => s.Code == code);

         if (settings.Count() > 2)
         {
            MetaSettingFault fault = new MetaSettingFault(string.Format("MetaSetting count for Environment:{0}, Type:{1}, Code:{2} is more than 2.", _environment, type, code));
            throw new FaultException<MetaSettingFault>(fault, fault.Message);
         }
         else if (settings.Count().Equals(2) && settings.Distinct().Count().Equals(1))
         {
            MetaSettingFault fault = new MetaSettingFault(string.Format("MetaSettings for Environment:{0}, Type:{1}, Code:{2} are invalid.", _environment, type, code));
            throw new FaultException<MetaSettingFault>(fault, fault.Message);
         }
         else if (settings.Distinct().Count().Equals(2))
            return settings.FirstOrDefault(s => s.Environment != Constants.MetaSettings.Environments.Default);
         else
            return settings.FirstOrDefault();
      }
      #endregion
   }
}
