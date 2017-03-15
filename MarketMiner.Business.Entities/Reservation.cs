using P.Core.Common.Contracts;
using N.Core.Common.Core;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class Reservation : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
   {
      #region Properties
      [DataMember]
      public int ReservationID { get; set; }
      [DataMember]
      public int AccountID { get; set; }
      [DataMember]
      public double Amount { get; set; }
      [DataMember]
      public bool Open { get; set; }
      [DataMember]
      public bool Cancelled { get; set; }    
  
      public virtual Account Account { get; set; }      
      public virtual List<Allocation> Allocations { get; set; }
      #endregion

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return ReservationID; }
         set { ReservationID = value; }
      }
      #endregion

      #region Members.IAccountOwnedEntity
      int IAccountOwnedEntity.OwnerAccountID
      {
         get { return AccountID; }
      }
      #endregion
   }
}
