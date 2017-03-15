using MarketMiner.Business.Entities;
using MarketMiner.Common;
using MarketMiner.Common.Faults;
using MarketMiner.Data.Contracts;
using N.Core.Business.Contracts;
using N.Core.Common.Core;
using P.Core.Common.Contracts;
using P.Core.Common.Faults;
using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Threading;

namespace MarketMiner.Business.Managers
{
   public abstract class ManagerBase
   {
      #region Properties
      /// <summary>
      /// Creates repositories via GetDataRepository<T> method.
      /// Is injected by Mef if AccountManager is instantiated via the parameter-less constructor.
      /// </summary>
      [Import]
      protected IDataRepositoryFactory _DataRepositoryFactory;

      /// <summary>
      /// Creates a business engine via GetDataRepository<T> method.
      /// Is injected by Mef if InventoryManager is instantiated via the parameter-less constructor.
      /// </summary>
      [Import]
      protected IBusinessEngineFactory _BusinessEngineFactory;

      string _LoginName = string.Empty;
      protected Account _AuthorizationAccount = null;

      #endregion

      #region Constructors

      public ManagerBase()
      {
         OperationContext context = OperationContext.Current;
         if (context != null)
         {
            try
            {
               _LoginName = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("String", "System");

               // if name contains a backslash then it's an admin user not a web user
               // the server needs to have OCOAppUsers and OCOAppAdministrators roles added using lusrmgr.msc
               // the service needs to be running under the login of a user with both roles
               // the admin app must be running under the identity of a user in the admin group
               if (_LoginName.IndexOf(@"\") > -1)
               {
                  _LoginName = string.Empty;
               }
            }
            catch
            {
               _LoginName = string.Empty;
            }
         }

         // load dependencies from the MefDI container .. since mgrs is not exported
         if (ObjectBase.Container != null)
            ObjectBase.Container.SatisfyImportsOnce(this);

         if (!string.IsNullOrWhiteSpace(_LoginName))
         {
            _AuthorizationAccount = LoadAuthorizationValidationAccount(_LoginName);
         }
      }
      #endregion

      #region Operations
      protected virtual Account LoadAuthorizationValidationAccount(string loginName)
      {
         return GetAccountEntity(loginName);
      }

      protected Account GetAccountEntity(string loginName)
      {
         IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
         Account acctEntity = accountRepository.GetByLogin(loginName);
         if (acctEntity == null)
         {
            NotFoundFault fault = new NotFoundFault(string.Format("Cannot find account for login name {0} to use as thread principal.", loginName));
            throw new FaultException<NotFoundFault>(fault, fault.Message);
         }

         return acctEntity;
      }

      protected void ValidateAuthorization(IAccountOwnedEntity entity)
      {
         if (!Thread.CurrentPrincipal.IsInRole(OCOApp.Security.Admin))
         {
            if (_AuthorizationAccount != null)
            {
               if (_LoginName != string.Empty && entity.OwnerAccountID != _AuthorizationAccount.AccountID)
               {
                  AuthorizationValidationFault fault = new AuthorizationValidationFault(
                        "Attempt to access a secure record with improper user authorization validation.");
                  throw new FaultException<AuthorizationValidationFault>(fault, fault.Message);
               }
            }
         }
      }

      /// <summary>
      /// Executes an operation Func delegated by a manager. Exceptions are handled.
      /// </summary>
      /// <typeparam name="T">Return type of the Func</typeparam>
      /// <param name="codetoExecute">Func to execute</param>
      /// <returns></returns>
      protected T ExecuteFaultHandledOperation<T>(Func<T> codetoExecute)
      {
         try
         {
            return codetoExecute.Invoke();
         }
         catch (FaultException ex)
         {
            throw ex;
         }
         catch (Exception ex)
         {
            throw new FaultException(ex.Message);
         }
      }

      /// <summary>
      /// Executes an operation Action delegated by a manager. Exceptions are handled.
      /// </summary>
      /// <param name="codetoExecute"></param>
      protected void ExecuteFaultHandledOperation(Action codetoExecute)
      {
         try
         {
            codetoExecute.Invoke();
         }
         catch (FaultException ex)
         {
            throw ex;
         }
         catch (Exception ex)
         {
            throw new FaultException(ex.Message);
         }
      }
      #endregion
   }
}

