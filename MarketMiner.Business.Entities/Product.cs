using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class Product : EntityBase, IIdentifiableEntity
   {
      [DataMember]
      public int ProductID { get; set; }
      [DataMember]
      public string Name { get; set; }
      [DataMember]
      public string Code { get; set; }
      [DataMember]
      public string ShortDesc { get; set; }
      [DataMember]
      public string LongDesc { get; set; }
      [DataMember]
      public bool Active { get; set; }

      public virtual List<Subscription> Subscriptions { get; set; }

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return ProductID; }
         set { ProductID = value; }
      }
      #endregion
   }
}
