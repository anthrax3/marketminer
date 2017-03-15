using MarketMiner.Business.Entities;
using MarketMiner.Common.Faults;
using P.Core.Common.Faults;
using System.ServiceModel;

namespace MarketMiner.Business.Contracts
{
   [ServiceContract]
   public interface IAccountService
   {
      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Account GetAccount(string loginEmail);

      [OperationContract]
      [FaultContract(typeof(AuthorizationValidationFault))]
      [FaultContract(typeof(EntityValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      void UpdateAccount(Account account);
   }
}
