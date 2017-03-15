using MarketMiner.Algorithm.Common;
using P.MarketMiner.Client.Entities;
using System.Threading.Tasks;

namespace MarketMiner.Algorithm.OANDA.CentralBanks
{
   [AlgorithmModule(Name = "BankOfJapanStrategy")]
   public class BankOfJapanAlgorithm : AlgorithmBase
   {
      public BankOfJapanAlgorithm(int accountId, Strategy strategy)
         : base(strategy)
      {
      }

      public override Task<bool> Start()
      {
         // DoThisBeforeARateDecision();
         // DoThisAfterARateDecision();

         return base.Start();
      }
   }
}
