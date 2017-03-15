using MarketMiner.Business.Contracts;
using MarketMiner.Business.Entities;
using MarketMiner.Common;
using MarketMiner.Data.Contracts;
using N.Core.Business.Contracts;
using N.Core.Common.ServiceModel;
using P.Core.Common.Contracts;
using P.Core.Common.Faults;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;

namespace MarketMiner.Business.Managers
{
   [ApplyProxyDataContractResolver]
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, ReleaseServiceInstanceOnTransactionComplete = false)]
   public class ParticipationManager : ManagerBase, IParticipationService
   {
      #region Constructors

      public ParticipationManager() { }

      public ParticipationManager(IDataRepositoryFactory dataRepositoryFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
      }

      public ParticipationManager(IBusinessEngineFactory businessEngineFactory)
      {
         _BusinessEngineFactory = businessEngineFactory;
      }

      public ParticipationManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
         _BusinessEngineFactory = businessEngineFactory;
      }
      #endregion

      #region Operations.IParticipationService

      #region Reservations
      [OperationBehavior(TransactionScopeRequired = true)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.User)]
      public Reservation UpdateReservation(Reservation reservation)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

            Account account = accountRepository.GetByLogin(reservation.Account.LoginEmail);
            if (account == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No account found for login '{0}'.", reservation.Account.LoginEmail));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            ValidateAuthorization(account);

            Reservation updatedEntity = null;

            if (reservation.ReservationID == 0)
               updatedEntity = reservationRepository.Add(reservation);
            else
               updatedEntity = reservationRepository.Update(reservation);

            return updatedEntity;
         });
      }

      [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.User)]
      public Reservation GetReservation(int reservationId)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

            Reservation reservation = reservationRepository.Get(reservationId);
            if (reservation == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No reservation found for id '{0}'.", reservationId));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            ValidateAuthorization(reservation);

            return reservation;
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Reservation[] GetActiveReservations()
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

            IEnumerable<Reservation> reservations = reservationRepository.Get().Where(r => r.Cancelled == false);

            return reservations.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.User)]
      public Reservation[] GetReservationsByAccount(string loginEmail)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

            Account account = accountRepository.GetByLogin(loginEmail);
            if (account == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No account found for login '{0}'.", loginEmail));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            ValidateAuthorization(account);

            List<Reservation> reservations = new List<Reservation>();

            IEnumerable<Reservation> reservationSet = reservationRepository.GetOpenReservationsByAccount(account.AccountID);

            return reservationSet.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Reservation[] GetCancelledReservations()
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

            IEnumerable<Reservation> reservations = reservationRepository.Get().Where(r => r.Cancelled == true);

            return (reservations != null ? reservations.ToArray() : null);
         });
      }

      [OperationBehavior(TransactionScopeRequired = true)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.User)]
      public void CancelReservation(int reservationId)
      {
         ExecuteFaultHandledOperation(() =>
         {
            Reservation reservation = GetReservation(reservationId);

            ValidateAuthorization(reservation);

            reservation.Cancelled = true;

            UpdateReservation(reservation);
         });
      }
      #endregion

      #region Participations
      [OperationBehavior(TransactionScopeRequired = true)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public void ExecuteParticipationsFromReservation(int reservationId)
      {
         ExecuteFaultHandledOperation(() =>
         {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
            IParticipationEngine ParticipationEngine = _BusinessEngineFactory.GetBusinessEngine<IParticipationEngine>();

            Reservation reservation = reservationRepository.Get(reservationId);
            if (reservation == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("Reservation {0} was not found.", reservationId));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            Account account = accountRepository.Get(reservation.AccountID);
            if (account == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No account found for account ID '{0}'.", reservation.AccountID));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            // create the new participations
            foreach (Allocation allocation in reservation.Allocations)
            {
               try
               {
                  Participation participation = ParticipationEngine.PushAllocationIntoParticipation(allocation);
               }
               catch (FaultException ex)
               {
                  throw ex;
               }
            }

            // close and save the reservation
            reservation.Open = false;
            reservationRepository.Update(reservation);
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.User)]
      public Participation[] GetParticipationsByAccount(string loginEmail)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            IParticipationRepository ParticipationRepository = _DataRepositoryFactory.GetDataRepository<IParticipationRepository>();

            Account account = accountRepository.GetByLogin(loginEmail);
            if (account == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No account found for login '{0}'.", loginEmail));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            ValidateAuthorization(account);

            IEnumerable<Participation> participations = ParticipationRepository.GetParticipationsByAccount(account.AccountID);

            return participations.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Participation[] GetParticipationsByStrategy(short strategyId)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IParticipationRepository ParticipationRepository = _DataRepositoryFactory.GetDataRepository<IParticipationRepository>();

            IEnumerable<Participation> participations = ParticipationRepository.GetParticipationsByStrategy(strategyId);

            return participations.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Participation[] GetParticipations()
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IParticipationRepository ParticipationRepository = _DataRepositoryFactory.GetDataRepository<IParticipationRepository>();

            IEnumerable<Participation> participations = ParticipationRepository.GetParticipations();

            return participations.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Participation GetParticipation(int participationId)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IParticipationRepository ParticipationRepository = _DataRepositoryFactory.GetDataRepository<IParticipationRepository>();

            Participation participation = ParticipationRepository.Get(participationId);
            if (participation == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No participation record found for id '{0}'.", participationId));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            ValidateAuthorization(participation);

            return participation;
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public bool IsStrategyAcceptingParticipations(int strategyId)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IStrategyEngine StrategyEngine = _BusinessEngineFactory.GetBusinessEngine<IStrategyEngine>();

            return StrategyEngine.CanStrategyAcceptFunds(strategyId, (decimal)0.01);
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.User)]
      public void RedeemParticipation(string loginEmail, int participationId, decimal amount)
      {
         ExecuteFaultHandledOperation(() =>
         {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            IParticipationRepository ParticipationRepository = _DataRepositoryFactory.GetDataRepository<IParticipationRepository>();
            IParticipationEngine ParticipationEngine = _BusinessEngineFactory.GetBusinessEngine<IParticipationEngine>();

            Account account = accountRepository.GetByLogin(loginEmail);
            if (account == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No account found for login '{0}'.", loginEmail));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }
            ValidateAuthorization(account);

            Participation participation = ParticipationRepository.Get(participationId);
            if (participation == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No participation record found for id '{0}'.", participationId));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            ParticipationEngine.RedeemFundsFromParticipation(participationId, amount);
         });
      }
      #endregion
      #endregion
   }
}
