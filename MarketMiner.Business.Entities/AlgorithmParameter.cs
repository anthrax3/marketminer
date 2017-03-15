using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class AlgorithmParameter : EntityBase, IIdentifiableEntity
   {
      #region Properties
      [DataMember]
      public int AlgorithmParameterID { get; set; }
      [DataMember]
      public int StrategyID { get; set; }
      [DataMember]
      public string ParameterName { get; set; }
      [DataMember]
      public string ParameterValue { get; set; }
      [DataMember]
      public string ParameterType { get; set; }

      public virtual Strategy Strategy { get; set; }
      #endregion

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return AlgorithmParameterID; }
         set { AlgorithmParameterID = value; }
      }
      #endregion
   }
}
