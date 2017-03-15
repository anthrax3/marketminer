using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class Communication : EntityBase, IIdentifiableEntity   
   {
      [DataMember]
      public int CommunicationID { get; set; }
      [DataMember]
      public int CommunicationTemplateID { get; set; }
      [DataMember]
      public int AccountID { get; set; }
      [DataMember]
      public short SendType { get; set; }
      [DataMember]
      public DateTime? Postmark { get; set; }

      public virtual CommunicationTemplate Template { get; set; }
      public virtual Account Account { get; set; }

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return CommunicationID; }
         set { CommunicationID = value; }
      }
      #endregion
   }
}
