using MarketMiner.Business.Contracts;
using MarketMiner.Business.Entities;
using MarketMiner.Common;
using MarketMiner.Data.Contracts;
using N.Core.Common.ServiceModel;
using P.Core.Common.Contracts;
using System.Security.Permissions;
using System.ServiceModel;

namespace MarketMiner.Business.Managers
{
   [ApplyProxyDataContractResolver]
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, ReleaseServiceInstanceOnTransactionComplete = false)]
   public class AccountManager : ManagerBase, IAccountService
   {
      #region Constructors
      public AccountManager() { }

      public AccountManager(IDataRepositoryFactory dataRepositoryFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
      }
      #endregion

      #region Operations.IAccountService
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.User)]
      public Account GetAccount(string loginEmail)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            Account accountEntity = GetAccountEntity(loginEmail);
            ValidateAuthorization(accountEntity);
            return accountEntity;
         });
      }

      [OperationBehavior(TransactionScopeRequired = true)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.User)]
      public void UpdateAccount(Account account)
      {
         ExecuteFaultHandledOperation(() =>
         {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            ValidateAuthorization(account);
            Account updatedAccount = accountRepository.Update(account);
         });
      }
      #endregion

   }
}
