using P.Core.Common.Contracts;

namespace MarketMiner.Business.Contracts
{
   public interface IMailerFactory
   {
      T GetMailer<T>() where T : IMailer;
      IMailer GetMailer(string assemblyQualifiedName);
   }
}