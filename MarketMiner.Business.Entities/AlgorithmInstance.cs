using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class AlgorithmInstance : EntityBase, IIdentifiableEntity
   {
      #region Properties
      [DataMember]
      public int AlgorithmInstanceID { get; set; }
      [DataMember]
      public int StrategyID { get; set; }
      [DataMember]
      public short? Status { get; set; }
      [DataMember]
      public DateTime? FirstTradeDateTime { get; set; }
      [DataMember]
      public DateTime? LastTradeDateTime { get; set; }
      [DataMember]
      public DateTime? RunStartDateTime { get; set; }
      [DataMember]
      public DateTime? RunStopDateTime { get; set; }

      public virtual Strategy Strategy { get; set; }
      public virtual List<AlgorithmMessage> Messages { get; set; }
      #endregion

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return AlgorithmInstanceID; }
         set { AlgorithmInstanceID = value; }
      }
      #endregion
   }
}
