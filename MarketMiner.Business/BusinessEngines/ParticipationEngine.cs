using MarketMiner.Business.Contracts;
using MarketMiner.Business.Entities;
using MarketMiner.Common.Faults;
using MarketMiner.Data.Contracts;
using P.Core.Common.Contracts;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;

namespace MarketMiner.Business.BusinessEngines
{
   [Export(typeof(IParticipationEngine))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class ParticipationEngine : BusinessEngineBase, IParticipationEngine
   {
      #region Constructors
      [ImportingConstructor]
      public ParticipationEngine(IDataRepositoryFactory dataRepositoryFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
      }
      #endregion

      public bool IsAccountParticipatingInFund(int accountId, int fundId)
      {
         // does account have a participation in the fund with balance > 0?
         return true;
      }

      public bool CanFundAcceptNewParticipation(int fundId, decimal amount)
      {
         bool fundHasSufficientCapacity = CanFundAcceptParticipationAddition(fundId, amount);

         if (fundHasSufficientCapacity)
         {
            IFundRepository fundRepository = _DataRepositoryFactory.GetDataRepository<IFundRepository>();
            Fund fund = fundRepository.Get(fundId);

            if (!fund.OpenToNew || DateTime.UtcNow > fund.CloseDate)
            {
               FundFault fault = new FundFault(string.Format("Fund {0} is closed to new participations.", fundId));
               throw new FaultException<FundFault>(fault, fault.Message);
            }

            return true;
         }
         else
            return fundHasSufficientCapacity;
      }

      public bool CanFundAcceptParticipationAddition(int fundId, decimal amount)
      {
         IFundRepository fundRepository = _DataRepositoryFactory.GetDataRepository<IFundRepository>();
         Fund fund = fundRepository.Get(fundId);

         if (fund == null)
         {
            FundFault fault = new FundFault(string.Format("Fund {0} does not exist in the system.", fundId));
            throw new FaultException<FundFault>(fault, fault.Message);
         }

         if (amount <= 0)
         {
            FundFault fault = new FundFault(string.Format("Allocation amount ${0} is invalid.", amount));
            throw new FaultException<FundFault>(fault, fault.Message);
         }
         else
         {
            decimal fundMaximumAUM = fund.Strategies.Sum(e => e.MaximumAUM);
            decimal fundCurrentAUM = fund.Strategies.Sum(e => e.CurrentAUM);

            if (amount > (fundMaximumAUM - fundCurrentAUM))
            {
               FundFault fault = new FundFault(string.Format("Allocation amount ${0} exceeds Fund {1} available capacity.", amount, fundId));
               throw new FaultException<FundFault>(fault, fault.Message);
            }
         }

         return true;
      }

      public bool CanParticipationBeIncreased(int participationId, decimal amount)
      {
         // apply rules here: 
         // is accountId allowed to increase? 
         // regulatory considerations? etc.
         return true;
      }

      public Participation PushAllocationIntoParticipation(Allocation allocation)
      {
         IParticipationRepository participationRepository = _DataRepositoryFactory.GetDataRepository<IParticipationRepository>();
         IFundRepository fundRepository = _DataRepositoryFactory.GetDataRepository<IFundRepository>();

         Participation participation = participationRepository.GetParticipationsByAccount(allocation.Reservation.AccountID)
                                                              .Where(p => p.FundID == allocation.FundID)
                                                              .FirstOrDefault();
         try
         {
            if (participation == null)
            {
               if (CanFundAcceptNewParticipation(allocation.FundID, allocation.Amount))
               {
                  participation = new Participation()
                  {
                     AccountID = allocation.Reservation.AccountID,
                     InitialBalance = allocation.Amount,
                     Fund = fundRepository.Get(allocation.FundID)
                  };

                  participationRepository.Add(participation);
               }
            }
            else
            {
               if (CanFundAcceptParticipationAddition(participation.Fund.FundID, allocation.Amount))
               {
                  participation.CurrentBalance += allocation.Amount;
               }

               participationRepository.Update(participation);
            }

            allocation.Pushed = true;
         }
         catch (Exception e)
         {

         }

         return participation;
      }

      public Redemption RedeemFundsFromParticipation(int participationId, decimal amount)
      {
         return new Redemption();
      }
   }
}
