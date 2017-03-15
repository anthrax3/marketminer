using MarketMiner.Api.Client.Common.Charting;

namespace MarketMiner.Api.Client.Common.Contracts
{
   public interface IPattern
   {
      string Type { get; set; }
      string Instrument { get; set; }
      string Granularity { get; set; }
      EPatternDirection Direction { get; set; }
      string Side { get; set; }
      double SignalPrice { get; set; }
      string SignalTime { get; set; }
      IStudy Study { get; set; }
      bool Active { get; set; }
   }

   public interface IPatternDetector
   {
      void Initialize (IParameters parameters);
      IPattern DetectPattern(Chart chart);
   }

   public interface IPatternFactory
   {
      void Initialize(IParameters parameters);
      IPattern BuildPattern(Chart chart, IPattern pattern);
      IPattern UpdatePattern(Chart chart, IPattern pattern);
   }
}
