using MarketMiner.Client.Contracts;
using MarketMiner.Client.Entities;
using P.Core.Common.Meta;
using System;
using System.Threading.Tasks;

namespace MarketMiner.Client.Proxies.ServiceCallers
{
   public class MetadataCaller : ServiceCaller
   {
      public static MetadataCaller Instance() { return new MetadataCaller(); }

      #region Operation result types
      MetaSetting _metaSettingResult;
      MetaSetting[] _metaSettingResults;
      MetaLookup _metaLookupResult;
      MetaLookup[] _metaLookupResults;
      MetaResource _metaResourceResult;
      MetaResource[] _metaResourceResults;
      #endregion

      #region Operations
      public void ClearCache()
      {
         WithMetadataClient(metadataClient =>
         {
            metadataClient.ClearCache();
         });
      }

      public void ClearCacheItem(string key)
      {
         WithMetadataClient(metadataClient =>
         {
            metadataClient.ClearCacheItem(key);
         });
      }

      public void ClearCacheSets(string[] entitySets)
      {
         WithMetadataClient(metadataClient =>
         {
            metadataClient.ClearCacheSets(entitySets);
         });
      }

      public MetaSetting GetSetting(string type, string code, bool enabledOnly = true)
      {
         WithMetadataClient(metadataClient =>
         {
            _metaSettingResult = metadataClient.GetSetting(type, code, enabledOnly);
         });
         return _metaSettingResult;
      }

      public MetaSetting[] GetSettings(string type, bool enabledOnly = true)
      {
         WithMetadataClient(metadataClient =>
         {
            _metaSettingResults = metadataClient.GetSettings(type, enabledOnly);
         });
         return _metaSettingResults;
      }

      public MetaLookup GetLookup(string type, string code, bool enabled = true)
      {
         WithMetadataClient(metadataClient =>
         {
            _metaLookupResult = metadataClient.GetLookup(type, code, enabled);
         });
         return _metaLookupResult;
      }

      public MetaLookup[] GetLookups(string type, bool enabledOnly = true)
      {
         WithMetadataClient(metadataClient =>
         {
            _metaLookupResults = metadataClient.GetLookups(type, enabledOnly);
         });
         return _metaLookupResults;
      }

      public MetaResource GetResource(string set, string type, string key, string cultureCode = "en-US", bool enabled = true)
      {
         WithMetadataClient(metadataClient =>
         {
            _metaResourceResult = metadataClient. GetResource(set, type, key, cultureCode, enabled);
         });
         return _metaResourceResult;
      }

      public MetaResource[] GetResources(string set, bool enabledOnly = true)
      {
         WithMetadataClient(metadataClient =>
         {
            _metaResourceResults = metadataClient.GetResources(set, enabledOnly);
         });
         return _metaResourceResults;
      }

      public MetaResource[] GetResources(string set, string type, bool enabledOnly = true)
      {
         WithMetadataClient(metadataClient =>
         {
            _metaResourceResults = metadataClient.GetResourcesByType(set, type, enabledOnly);
         });
         return _metaResourceResults;
      }

      public MetaResource[] GetResourcesByCulture(string set, string cultureCode = "en-US", bool enabledOnly = true)
      {
         WithMetadataClient(metadataClient =>
         {
            _metaResourceResults = metadataClient.GetResourcesByCulture(set, cultureCode, enabledOnly);
         });
         return _metaResourceResults;
      }

      public MetaResource[] GetResourcesByCulture(string set, string type, string cultureCode = "en-US", bool enabledOnly = true)
      {
         WithMetadataClient(metadataClient =>
         {
            _metaResourceResults = metadataClient.GetResourcesByTypeAndCulture(set, type, cultureCode, enabledOnly);
         });
         return _metaResourceResults;
      }
      #endregion

      #region Operations.Async
      public async Task ClearCacheAsync()
      {
         await WithMetadataClientAsync(async metadataClient =>
         {
            await metadataClient.ClearCacheAsync();
         });
      }

      public async Task ClearCacheItemAsync(string key)
      {
         await WithMetadataClientAsync(async metadataClient =>
         {
            await metadataClient.ClearCacheItemAsync(key);
         });
      }

      public async Task ClearCacheSetsAsync(string[] entitySets)
      {
         await WithMetadataClientAsync(async metadataClient =>
         {
            await metadataClient.ClearCacheSetsAsync(entitySets);
         });
      }

      public async Task<MetaSetting> GetSettingAsync(string type, string code, bool enabledOnly = true)
      {
         await WithMetadataClientAsync(async metadataClient =>
         {
            _metaSettingResult = await metadataClient.GetSettingAsync(type, code, enabledOnly);
         });
         return _metaSettingResult;
      }

      public async Task<MetaSetting[]> GetSettingsAsync(string type, bool enabledOnly = true)
      {
         await WithMetadataClientAsync(async metadataClient =>
         {
            _metaSettingResults = await metadataClient.GetSettingsAsync(type, enabledOnly);
         });
         return _metaSettingResults;
      }

      public async Task<MetaLookup> GetLookupAsync(string type, string code, bool enabled = true)
      {
         await WithMetadataClientAsync(async metadataClient =>
         {
            _metaLookupResult = await metadataClient.GetLookupAsync(type, code, enabled);
         });
         return _metaLookupResult;
      }

      public async Task<MetaLookup[]> GetLookupsAsync(string type, bool enabledOnly = true)
      {
         await WithMetadataClientAsync(async metadataClient =>
         {
            _metaLookupResults = await metadataClient.GetLookupsAsync(type, enabledOnly);
         });
         return _metaLookupResults;
      }

      public async Task<MetaResource> GetResourceAsync(string set, string type, string key, string cultureCode = "en-US", bool enabled = true)
      {
         await WithMetadataClientAsync(async metadataClient =>
         {
            _metaResourceResult = await metadataClient.GetResourceAsync(set, type, key, cultureCode, enabled);
         });
         return _metaResourceResult;
      }

      public async Task<MetaResource[]> GetResourcesAsync(string set, bool enabledOnly = true)
      {
         await WithMetadataClientAsync(async metadataClient =>
         {
            _metaResourceResults = await metadataClient.GetResourcesAsync(set, enabledOnly);
         });
         return _metaResourceResults;
      }

      public async Task<MetaResource[]> GetResourcesByTypeAsync(string set, string type, bool enabledOnly = true)
      {
         await WithMetadataClientAsync(async metadataClient =>
         {
            _metaResourceResults = await metadataClient.GetResourcesByTypeAsync(set, type, enabledOnly);
         });
         return _metaResourceResults;
      }

      public async Task<MetaResource[]> GetResourcesByCultureAsync(string set, string cultureCode = "en-US", bool enabledOnly = true)
      {
         await WithMetadataClientAsync(async metadataClient =>
         {
            _metaResourceResults = await metadataClient.GetResourcesByCultureAsync(set, cultureCode, enabledOnly);
         });
         return _metaResourceResults;
      }

      public async Task<MetaResource[]> GetResourcesByTypeAndCultureAsync(string set, string type, string cultureCode = "en-US", bool enabledOnly = true)
      {
         await WithMetadataClientAsync(async metadataClient =>
         {
            _metaResourceResults = await metadataClient.GetResourcesByTypeAndCultureAsync(set, type, cultureCode, enabledOnly);
         });
         return _metaResourceResults;
      }
      #endregion

      #region Utilities
      protected virtual void WithMetadataClient(Action<IMetadataService> codeToExecute)
      {
         WithClient<IMetadataService>(_serviceFactory.CreateClient<IMetadataService>(), codeToExecute);
      }

      protected virtual async Task WithMetadataClientAsync(Func<IMetadataService, Task> codeToExecute)
      {
         await WithClientAsync<IMetadataService>(_serviceFactory.CreateClient<IMetadataService>(), codeToExecute);
      }
      #endregion
   }
}
