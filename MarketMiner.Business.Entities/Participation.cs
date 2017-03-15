using P.Core.Common.Contracts;
using N.Core.Common.Core;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class Participation : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
   {
      [DataMember]
      public int ParticipationID { get; set; }
      [DataMember]
      public int AccountID { get; set; }
      [DataMember]
      public int FundID { get; set; }
      [DataMember]
      public decimal InitialBalance { get; set; }
      [DataMember]
      public decimal CurrentBalance { get; set; }

      public virtual Account Account { get; set; }
      public virtual Fund Fund { get; set; }
      public virtual List<Redemption> Redemptions { get; set; }

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return ParticipationID; }
         set { ParticipationID = value; }
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

