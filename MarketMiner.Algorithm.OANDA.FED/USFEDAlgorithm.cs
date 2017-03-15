using MarketMiner.Algorithm.Common;
using P.MarketMiner.Client.Entities;
using System.Threading.Tasks;

namespace MarketMiner.Algorithm.OANDA.CentralBanks
{
   [AlgorithmModule(Name = "USFEDAlgorithm")]
   class USFEDAlgorithm : AlgorithmBase
   {
      public USFEDAlgorithm(int accountId, Strategy strategy)
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
