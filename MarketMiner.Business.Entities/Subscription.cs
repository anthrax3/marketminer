using P.Core.Common.Contracts;
using N.Core.Common.Core;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class Subscription : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
   {
      [DataMember]
      public int SubscriptionID { get; set; }
      [DataMember]
      public int AccountID { get; set; }
      [DataMember]
      public int ProductID { get; set; }
      [DataMember]
      public bool Active { get; set; }

      public virtual Account Account { get; set; }
      public virtual Product Product { get; set; }

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return SubscriptionID; }
         set { SubscriptionID = value; }
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
