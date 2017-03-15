using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class Strategy : EntityBase, IIdentifiableEntity
   {
      [DataMember] 
      public int StrategyID { get; set; }
      [DataMember] 
      public int FundID { get; set; }
      [DataMember]
      public string Name { get; set; }
      [DataMember]
      public string ShortDesc { get; set; }
      [DataMember]
      public string LongDesc { get; set; }
      [DataMember]
      public decimal InitialAUM { get; set; }
      [DataMember]
      public decimal CurrentAUM { get; set; }
      [DataMember]
      public decimal MaximumAUM { get; set; }
      [DataMember]
      public string AlgorithmClass { get; set; }
      [DataMember]
      public string AlgorithmAssembly { get; set; }

      public virtual Fund Fund { get; set; }
      public virtual List<AlgorithmInstance> AlgorithmInstances { get; set; }
      public virtual List<AlgorithmParameter> AlgorithmParameters { get; set; }
      public virtual List<StrategyTransaction> Transactions { get; set; }

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return StrategyID; }
         set { StrategyID = value; }
      }
      #endregion
   }
}
