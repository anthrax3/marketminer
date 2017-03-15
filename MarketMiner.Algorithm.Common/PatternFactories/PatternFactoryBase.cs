using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.Common.Contracts;

namespace MarketMiner.Algorithm.Common.PatternManagers
{
   public abstract class PatternFactoryBase : IPatternFactory
   {
      protected IParameters _parameters;

      public virtual void Initialize(IParameters parameters)
      {
         _parameters = parameters;
      }

      public abstract IPattern BuildPattern(Chart chart, IPattern pattern);
      public abstract IPattern UpdatePattern(Chart chart, IPattern pattern);
   }
}
