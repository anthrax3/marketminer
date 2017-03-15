using MarketMiner.Algorithm.Common.Contracts;
using N.Core.Common.Core;
using System.ComponentModel.Composition;

namespace MarketMiner.Algorithm.Common
{
   [Export(typeof(IAlgorithmFactory))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class AlgorithmFactory : IAlgorithmFactory
   {
      #region Operations.Interface.IAlgorithmFactory

      T IAlgorithmFactory.CreateAlgorithm<T>()
      {
         return ObjectBase.Container.GetExportedValue<T>();
      }
      #endregion
   }
}
