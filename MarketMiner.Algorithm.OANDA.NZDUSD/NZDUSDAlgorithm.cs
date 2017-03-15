using MarketMiner.Algorithm.Common;
using P.MarketMiner.Client.Entities;
using System.Threading.Tasks;

namespace MarketMiner.Strategies.OANDA.Currencies
{
   [AlgorithmModule(Name = "NZDUSDAlgorithm")]
   public class NZDUSDAlgorithm : AlgorithmBase
   {
      public NZDUSDAlgorithm(int accountId, Strategy strategy)
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
