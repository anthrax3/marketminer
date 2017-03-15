using P.Core.Common.Contracts;
using N.Core.Common.Data;
using System.Configuration;

namespace MarketMiner.Data.Repositories
{
   /// <summary>
   /// Base class for repositories that read/write to MarketMinerContext
   /// </summary>
   /// <typeparam name="T">Entity that has a DbSet defined in MarketMinerContext</typeparam>
   public abstract class MarketMinerRepositoryBase<T> : DataRepositoryBase<T, MarketMinerContext>
      where T : class, IIdentifiableEntity, new()
   {
      protected string _environment = ConfigurationManager.AppSettings["Environment"];
   }
}
