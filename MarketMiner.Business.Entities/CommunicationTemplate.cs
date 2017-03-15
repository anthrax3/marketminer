using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class CommunicationTemplate : EntityBase, IIdentifiableEntity   
   {
      [DataMember]
      public int CommunicationTemplateID { get; set; }
      [DataMember]
      public string Name { get; set; }
      [DataMember]
      public string Subject { get; set; }
      [DataMember]
      public string Body { get; set; }

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return CommunicationTemplateID; }
         set { CommunicationTemplateID = value; }
      }
      #endregion
   }
}
