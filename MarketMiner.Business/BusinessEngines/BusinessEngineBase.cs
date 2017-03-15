using MarketMiner.Business.Contracts;
using N.Core.Business.Contracts;
using P.Core.Common.Contracts;

namespace MarketMiner.Business
{
   public abstract class BusinessEngineBase : IBusinessEngine
   {
      public IDataRepositoryFactory _DataRepositoryFactory { get; set; }
      public IMailerFactory _MailerFactory { get; set; }
   }
}
