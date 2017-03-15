using System;

namespace MarketMiner.Algorithm.Common
{
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
   public class AlgorithmModuleAttribute : Attribute
   {
      public string Name { get; set; }
      public int StrategyID { get; set; }
   }
}
