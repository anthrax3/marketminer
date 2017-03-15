using MarketMiner.Client.Entities;
using MarketMiner.Common.Faults;
using P.Core.Common.Contracts;
using P.Core.Common.Faults;
using System.ServiceModel;
using System.Threading.Tasks;

namespace MarketMiner.Client.Contracts
{
   [ServiceContract]
   public interface IAccountService : IServiceContract
   {
      #region Operations
      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Account GetAccount(string loginEmail);

      [OperationContract]
      [FaultContract(typeof(AuthorizationValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      Account UpdateAccount(Account account);
      #endregion

      #region Operations.Async
      [OperationContract]
      Task<Account> GetAccountAsync(string loginEmail);

      [OperationContract]
      Task<Account> UpdateAccountAsync(Account account);
      #endregion
   }
}
