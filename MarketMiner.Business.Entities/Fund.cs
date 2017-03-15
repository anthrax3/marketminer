using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class Fund : EntityBase, IIdentifiableEntity   
   {
      [DataMember]
      public int FundID { get; set; }
      [DataMember]
      public string Name { get; set; }
      [DataMember]
      public DateTime? OpenDate { get; set; }
      [DataMember]
      public DateTime? CloseDate { get; set; }
      [DataMember]
      public bool OpenToNew { get; set; }
      [DataMember]
      public bool OpenToAdd { get; set; }
      [DataMember]
      public bool OpenToRedeem { get; set; }

      public virtual List<Strategy> Strategies { get; set; }
      public virtual List<Participation> Participations { get; set; }
      public virtual List<Allocation> Allocations { get; set; }

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return FundID; }
         set { FundID = value; }
      }
      #endregion
   }
}
