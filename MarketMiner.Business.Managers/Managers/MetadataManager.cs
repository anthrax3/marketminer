using MarketMiner.Business.Contracts;
using MarketMiner.Common;
using MarketMiner.Data.Contracts;
using N.Core.Business.Contracts;
using N.Core.Common.Core;
using N.Core.Common.ServiceModel;
using P.Core.Common.Contracts;
using P.Core.Common.Faults;
using P.Core.Common.Meta;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;

namespace MarketMiner.Business.Managers
{
   [ApplyProxyDataContractResolver]
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
   public class MetadataManager : ManagerBase, IMetadataService
   {
      #region Constructors
      public MetadataManager() { }

      public MetadataManager(IDataRepositoryFactory dataRepositoryFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
      }

      public MetadataManager(IBusinessEngineFactory businessEngineFactory)
      {
         _BusinessEngineFactory = businessEngineFactory;
      }

      public MetadataManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
         _BusinessEngineFactory = businessEngineFactory;
      }
      #endregion

      #region Operations.IMetadataService
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public void ClearCache()
      {
         ExecuteFaultHandledOperation(() =>
         {
            ObjectBase.Container.GetExportedValue<IDataCache>().Purge();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public void ClearCacheItem(string key)
      {
         ExecuteFaultHandledOperation(() =>
         {
            ObjectBase.Container.GetExportedValue<IDataCache>().RemoveItem(key);
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public void ClearCacheSets(string[] entitySets)
      {
         ExecuteFaultHandledOperation(() =>
         {
            ObjectBase.Container.GetExportedValue<IDataCache>().RemoveSets(entitySets);
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public MetaSetting[] GetSettings(string type, bool enabledOnly = true)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IMetaSettingRepository settingRepository = _DataRepositoryFactory.GetDataRepository<IMetaSettingRepository>();
            IEnumerable<MetaSetting> settings = settingRepository.GetSettings(type, enabledOnly);

            return settings.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public MetaSetting GetSetting(string type, string code, bool enabledOnly = true)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IMetaSettingRepository settingRepository = _DataRepositoryFactory.GetDataRepository<IMetaSettingRepository>();
            MetaSetting setting = settingRepository.GetSetting(type, code, enabledOnly);

            if (setting == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No setting found for type-code: [{0}]-[{1}].", type, code));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            return setting;
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public MetaLookup[] GetLookups(string type, bool enabledOnly = true)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IMetaLookupRepository lookupRepository = _DataRepositoryFactory.GetDataRepository<IMetaLookupRepository>();
            IEnumerable<MetaLookup> lookups = lookupRepository.GetLookups(type, enabledOnly);

            return lookups.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public MetaLookup GetLookup(string type, string code, bool enabled = true)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IMetaLookupRepository lookupRepository = _DataRepositoryFactory.GetDataRepository<IMetaLookupRepository>();
            MetaLookup lookup = lookupRepository.GetLookup(type, code, enabled);

            if (lookup == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No lookup found for type-code: [{0}]-[{1}].", type, code));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            return lookup;
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public MetaResource GetResource(string set, string type, string key, string cultureCode = "en-US", bool enabled = true)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IMetaResourceRepository resourceRepository = _DataRepositoryFactory.GetDataRepository<IMetaResourceRepository>();
            MetaResource resource = resourceRepository.GetResource(set, type, key, cultureCode, enabled);

            if (resource == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No resource found for set-type-code-culture-enabled: [{0}]-[{1}]-[{2}]-[{3}]-[{4}].", set, type, key, cultureCode, enabled));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            return resource;
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public MetaResource[] GetResources(string set, bool enabledOnly = true)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IMetaResourceRepository resourceRepository = _DataRepositoryFactory.GetDataRepository<IMetaResourceRepository>();
            IEnumerable<MetaResource> resources = resourceRepository.GetResources(set, enabledOnly);

            return resources.ToArray();
         });
      }

      public MetaResource[] GetResourcesByType(string set, string type, bool enabledOnly = true)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IMetaResourceRepository resourceRepository = _DataRepositoryFactory.GetDataRepository<IMetaResourceRepository>();
            IEnumerable<MetaResource> resources = resourceRepository.GetResources(set, type, enabledOnly);

            return resources.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public MetaResource[] GetResourcesByCulture(string set, string cultureCode = "en-US", bool enabledOnly = true)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IMetaResourceRepository resourceRepository = _DataRepositoryFactory.GetDataRepository<IMetaResourceRepository>();
            IEnumerable<MetaResource> resources = resourceRepository.GetResourcesByCulture(set, cultureCode, enabledOnly);

            return resources.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public MetaResource[] GetResourcesByTypeAndCulture(string set, string type, string cultureCode = "en-US", bool enabledOnly = true)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IMetaResourceRepository resourceRepository = _DataRepositoryFactory.GetDataRepository<IMetaResourceRepository>();
            IEnumerable<MetaResource> resources = resourceRepository.GetResourcesByCulture(set, type, cultureCode, enabledOnly);

            return resources.ToArray();
         });
      }
      #endregion
   }
}
