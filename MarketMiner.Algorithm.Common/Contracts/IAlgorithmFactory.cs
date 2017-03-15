namespace MarketMiner.Algorithm.Common.Contracts
{
   public interface IAlgorithmFactory
   {
      T CreateAlgorithm<T>() where T : IAlgorithm;
   }
}
