using System.Threading.Tasks;

namespace MarketMiner.Algorithm.Common.Contracts
{
   interface IBacktest
   {
      short Granularity { get; set; }
      int Period { get; set; }
      decimal WinLoss { get; set; }

      Task Run();
   }
}
