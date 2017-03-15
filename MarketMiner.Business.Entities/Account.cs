using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class Account : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
   {
      #region Accessors
      [DataMember]
      public int AccountID { get; set; }
      [DataMember]
      public string LoginEmail { get; set; }
      [DataMember]
      public string FirstName { get; set; }
      [DataMember]
      public string LastName { get; set; }
      [DataMember]
      public string Address { get; set; }
      [DataMember]
      public string City { get; set; }
      [DataMember]
      public string State { get; set; }
      [DataMember]
      public string ZipCode { get; set; }
      [DataMember]
      public string CreditCard { get; set; }
      [DataMember]
      public DateTime? ExpDate { get; set; }

      public virtual List<Subscription> Subscriptions { get; set; }
      public virtual List<Participation> Participations { get; set; }
      public virtual List<Reservation> Reservations { get; set; }
      public virtual List<Redemption> Redemptions { get; set; }
      #endregion

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return AccountID; }
         set { AccountID = value; }
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
