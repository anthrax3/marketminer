namespace MarketMiner.Api.Client.Common.Contracts
{
   public interface IParameters
   {
      string GetString(string name);
      bool? GetBoolean(string name);
      double? GetDouble(string name);
      decimal? GetDecimal(string name);
      short? GetShort(string name);
      int? GetInteger(string name);
      long? GetLong(string name);
   }
}
