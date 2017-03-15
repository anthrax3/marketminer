using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class Broker : EntityBase, IIdentifiableEntity   
   {
      [DataMember]
      public int BrokerID { get; set; }
      [DataMember]
      public string Name { get; set; }

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return BrokerID; }
         set { BrokerID = value; }
      }
      #endregion
   }
}
