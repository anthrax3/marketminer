using P.Core.Common.Contracts;
using N.Core.Common.Core;
using System;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class Redemption : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
   {
      [DataMember]
      public int RedemptionID { get; set; }
      [DataMember]
      public int Amount { get; set; }
      [DataMember]
      public DateTime? DatePaid { get; set; }
      [DataMember]
      public int? AccountID { get; set; }
      [DataMember]
      public int ParticipationID { get; set; }

      public virtual Account Account { get; set; }      
      public virtual Participation Participation { get; set; }

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return RedemptionID; }
         set { RedemptionID = value; }
      }
      #endregion

      #region Members.IAccountOwnedEntity
      int IAccountOwnedEntity.OwnerAccountID
      {
         get { return AccountID.GetValueOrDefault(); }
      }
      #endregion
   }
}
