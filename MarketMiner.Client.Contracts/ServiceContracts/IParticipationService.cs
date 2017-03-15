using MarketMiner.Client.Entities;
using MarketMiner.Common.Faults;
using P.Core.Common.Contracts;
using P.Core.Common.Faults;
using System.ServiceModel;
using System.Threading.Tasks;

namespace MarketMiner.Client.Contracts
{
   [ServiceContract]
   public interface IParticipationService : IServiceContract
   {
      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(FundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      Reservation CreateReservation(Reservation reservation);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Reservation GetReservation(int reservationId);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Reservation[] GetActiveReservations();

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Reservation[] GetReservationsByAccount(string loginEmail);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Reservation[] GetCancelledReservations();

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      void CancelReservation(int reservationId);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      [FaultContract(typeof(FundFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      void ExecuteParticipationsFromReservation(int reservationId);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      void RedeemParticipation(string loginEmail, int participationId, double amount);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Participation[] GetParticipationsByAccount(string loginEmail);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Participation[] GetParticipations();

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Participation GetParticipation(int participationId);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      bool IsStrategyAcceptingParticipations(int strategyId);


      #region Async operations
      [OperationContract]
      Task<Reservation> CreateReservationAsync(Reservation reservation);

      [OperationContract]
      Task<Reservation> GetReservationAsync(int reservationId);

      [OperationContract]
      Task<Reservation[]> GetActiveReservationsAsync();

      [OperationContract]
      Task<Reservation[]> GetReservationsByAccountAsync(string loginEmail);

      [OperationContract]
      Task<Reservation[]> GetCancelledReservationsAsync();

      [OperationContract]
      Task CancelReservationAsync(int reservationId);

      [OperationContract]
      Task ExecuteParticipationsFromReservationAsync(int reservationId);

      [OperationContract]
      Task RedeemParticipationAsync(string loginEmail, int participationId, double amount);

      [OperationContract]
      Task<Participation[]> GetParticipationsByAccountAsync(string loginEmail);

      [OperationContract]
      Task<Participation[]> GetParticipationsAsync();

      [OperationContract]
      Task<Participation> GetParticipationAsync(int participationId);

      [OperationContract]
      Task<bool> IsStrategyAcceptingParticipationsAsync(int strategyId);
      #endregion
   }
}
