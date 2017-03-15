using MarketMiner.Client.Contracts;
using MarketMiner.Client.Entities;
using N.Core.Common.ServiceModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace MarketMiner.Client.Proxies
{
   [Export(typeof(IParticipationService))]  // MefDI: interface mapping
   [PartCreationPolicy(CreationPolicy.NonShared)] // MEfDI: non-singleton
   public class ParticipationClient : UserClientBase<IParticipationService>, IParticipationService
   {
      public Reservation CreateReservation(Reservation reservation)
      {
         return Channel.CreateReservation(reservation);
      }

      public Reservation GetReservation(int reservationId)
      {
         return Channel.GetReservation(reservationId);
      }

      public Reservation[] GetActiveReservations()
      {
         return Channel.GetActiveReservations();
      }

      public Reservation[] GetReservationsByAccount(string loginEmail)
      {
         return Channel.GetReservationsByAccount(loginEmail);
      }

      public Reservation[] GetCancelledReservations()
      {
         return Channel.GetCancelledReservations();
      }

      public void CancelReservation(int reservationId)
      {
         Channel.CancelReservation(reservationId);
      }

      public void ExecuteParticipationsFromReservation(int reservationId)
      {
         Channel.ExecuteParticipationsFromReservation(reservationId);
      }

      public void RedeemParticipation(string loginEmail, int participationId, double amount)
      {
         Channel.RedeemParticipation(loginEmail, participationId, amount);
      }

      public Participation[] GetParticipationsByAccount(string loginEmail)
      {
         return Channel.GetParticipationsByAccount(loginEmail);
      }

      public Participation[] GetParticipations()
      {
         return Channel.GetParticipations();
      }

      public Participation GetParticipation(int participationId)
      {
         return Channel.GetParticipation(participationId);
      }

      public bool IsStrategyAcceptingParticipations(int strategyId)
      {
         return Channel.IsStrategyAcceptingParticipations(strategyId);
      }

      #region Async methods
      public async Task<Reservation> CreateReservationAsync(Reservation reservation)
      {
         return await Channel.CreateReservationAsync(reservation);
      }

      public async Task<Reservation> GetReservationAsync(int reservationId)
      {
         return await Channel.GetReservationAsync(reservationId);
      }

      public async Task<Reservation[]> GetActiveReservationsAsync()
      {
         return await Channel.GetActiveReservationsAsync();
      }

      public async Task<Reservation[]> GetReservationsByAccountAsync(string loginEmail)
      {
         return await Channel.GetReservationsByAccountAsync(loginEmail);
      }

      public async Task<Reservation[]> GetCancelledReservationsAsync()
      {
         return await Channel.GetCancelledReservationsAsync();
      }

      public async Task CancelReservationAsync(int reservationId)
      {
         await Channel.CancelReservationAsync(reservationId);
      }

      public async Task ExecuteParticipationsFromReservationAsync(int reservationId)
      {
         await Channel.ExecuteParticipationsFromReservationAsync(reservationId);
      }

      public async Task RedeemParticipationAsync(string loginEmail, int participationId, double amount)
      {
         await Channel.RedeemParticipationAsync(loginEmail, participationId, amount);
      }

      public async Task<Participation[]> GetParticipationsByAccountAsync(string loginEmail)
      {
         return await Channel.GetParticipationsByAccountAsync(loginEmail);
      }

      public async Task<Participation[]> GetParticipationsAsync()
      {
         return await Channel.GetParticipationsAsync();
      }

      public async Task<Participation> GetParticipationAsync(int participationId)
      {
         return await Channel.GetParticipationAsync(participationId);
      }

      public async Task<bool> IsStrategyAcceptingParticipationsAsync(int strategyId)
      {
         return await Channel.IsStrategyAcceptingParticipationsAsync(strategyId);
      }
      #endregion
   }
}
