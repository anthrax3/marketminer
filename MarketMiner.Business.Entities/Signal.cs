using P.Core.Common.Contracts;
using N.Core.Common.Core;
using System.Runtime.Serialization;
using System;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class Signal : EntityBase, IIdentifiableEntity
   {
      [DataMember]
      public int SignalID { get; set; }
      [DataMember]
      public int ProductID { get; set; }
      [DataMember]
      public int? StrategyTransactionID { get; set; }
      [DataMember]
      public string Type { get; set; }
      [DataMember]
      public string Instrument { get; set; }
      [DataMember]
      public string Granularity { get; set; }
      [DataMember]
      public string Side { get; set; }
      [DataMember]
      public double SignalPrice { get; set; }
      [DataMember]
      public string SignalTime { get; set; }
      [DataMember]
      public DateTime? SendPostmark { get; set; }
      [DataMember]
      public bool Active { get; set; }

      public virtual Product Product { get; set; }
      public virtual StrategyTransaction StrategyTransaction { get; set; }

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return SignalID; }
         set { SignalID = value; }
      }
      #endregion
   }
}
