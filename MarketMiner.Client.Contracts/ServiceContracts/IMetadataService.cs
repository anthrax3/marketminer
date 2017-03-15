using MarketMiner.Common.Faults;
using P.Core.Common.Contracts;
using P.Core.Common.Faults;
using P.Core.Common.Meta;
using System.ServiceModel;
using System.Threading.Tasks;

namespace MarketMiner.Client.Contracts
{
   [ServiceContract]
   public interface IMetadataService : IServiceContract
   {
      #region Operations
      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      void ClearCache();

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      void ClearCacheItem(string key);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      void ClearCacheSets(string[] entitySets);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      MetaSetting GetSetting(string type, string code, bool enabledOnly = true);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      MetaSetting[] GetSettings(string type, bool enabledOnly = true);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      MetaLookup GetLookup(string type, string code, bool enabled = true);

      [OperationContract]
      [FaultContract(typeof(AuthorizationValidationFault))]
      MetaLookup[] GetLookups(string type, bool enabledOnly = true);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      MetaResource GetResource(string set, string type, string key, string cultureCode = "en-US", bool enabled = true);

      [OperationContract]
      [FaultContract(typeof(AuthorizationValidationFault))]
      MetaResource[] GetResources(string set, bool enabledOnly = true);

      [OperationContract]
      [FaultContract(typeof(AuthorizationValidationFault))]
      MetaResource[] GetResourcesByType(string set, string type, bool enabledOnly = true);

      [OperationContract]
      [FaultContract(typeof(AuthorizationValidationFault))]
      MetaResource[] GetResourcesByCulture(string set, string cultureCode = "en-US", bool enabledOnly = true);

      [OperationContract]
      [FaultContract(typeof(AuthorizationValidationFault))]
      MetaResource[] GetResourcesByTypeAndCulture(string set, string type, string cultureCode = "en-US", bool enabledOnly = true);
      #endregion

      #region Operations.Async
      [OperationContract]
      Task ClearCacheAsync();

      [OperationContract]
      Task ClearCacheItemAsync(string key);

      [OperationContract]
      Task ClearCacheSetsAsync(string[] entitySets);

      [OperationContract]
      Task<MetaSetting> GetSettingAsync(string type, string code, bool enabledOnly = true);

      [OperationContract]
      Task<MetaSetting[]> GetSettingsAsync(string type, bool enabledOnly = true);

      [OperationContract]
      Task<MetaLookup> GetLookupAsync(string type, string code, bool enabled = true);

      [OperationContract]
      Task<MetaLookup[]> GetLookupsAsync(string type, bool enabledOnly = true);

      [OperationContract]
      Task<MetaResource> GetResourceAsync(string set, string type, string key, string cultureCode = "en-US", bool enabled = true);

      [OperationContract]
      Task<MetaResource[]> GetResourcesAsync(string set, bool enabledOnly = true);

      [OperationContract]
      Task<MetaResource[]> GetResourcesByTypeAsync(string set, string type, bool enabledOnly = true);

      [OperationContract]
      Task<MetaResource[]> GetResourcesByCultureAsync(string set, string cultureCode = "en-US", bool enabledOnly = true);

      [OperationContract]
      Task<MetaResource[]> GetResourcesByTypeAndCultureAsync(string set, string type, string cultureCode = "en-US", bool enabledOnly = true);
      #endregion
   }
}
