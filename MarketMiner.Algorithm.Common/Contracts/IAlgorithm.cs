using MarketMiner.Client.Entities;
using P.Core.Common.Contracts;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MarketMiner.Algorithm.Common.Contracts
{
   public interface IAlgorithm
   {
      ILogger Logger { get; }
      Strategy Strategy { get; }
      AlgorithmInstance Instance { get; }
      ObservableCollection<AlgorithmMessage> Messages { get; }

      Task<bool> Initialize(int strategyId);
      Task<bool> Start();
      Task<bool> Stop();
   }
}
