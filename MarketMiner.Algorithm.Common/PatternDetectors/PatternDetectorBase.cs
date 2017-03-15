using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.Common.Contracts;

namespace MarketMiner.Algorithm.Common.PatternDetectors
{
   public abstract class PatternDetectorBase : IPatternDetector
   {
      protected IParameters _parameters;

      public virtual void Initialize(IParameters parameters)
      {
         _parameters = parameters;
      }

      public abstract IPattern DetectPattern(Chart chart);
   }
}
