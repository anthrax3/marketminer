using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class AlgorithmMessage : EntityBase, IIdentifiableEntity
   {
      [DataMember]
      public int AlgorithmMessageID { get; set; }
      [DataMember]
      public int AlgorithmInstanceID { get; set; }
      [DataMember]
      public string Message { get; set; }
      [DataMember]
      public TraceEventType Severity { get; set; }

      public virtual AlgorithmInstance AlgorithmInstance { get; set; }

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return AlgorithmMessageID; }
         set { AlgorithmMessageID = value; }
      }
      #endregion
   }
}
