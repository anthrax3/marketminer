using MarketMiner.Client.Proxies.ServiceCallers;
using P.Core.Common.Meta;
using System;

namespace MarketMiner.Client.Common
{
    public class MetadataHelper
    {
       public static string GetSettingAsString(string type, string code, bool enabledOnly = true)
       {
          try
          {
             MetaSetting setting = MetadataCaller.Instance().GetSetting(type, code, enabledOnly);
             return setting.Value;
          }
          catch { return null; }
       }

       public static int? GetSettingAsInteger(string type, string code, bool enabledOnly = true)
       {
          try {
             MetaSetting setting = MetadataCaller.Instance().GetSetting(type, code, enabledOnly); 
             return Convert.ToInt32(setting.Value);
          }
          catch { return null; }
       }
    }
}
