using P.Core.Common.Contracts;
using N.Core.Common.Core;
using System.Runtime.Serialization;

namespace MarketMiner.Business.Entities
{
   [DataContract]
   public class Allocation : EntityBase, IIdentifiableEntity
   {
      #region Accessors
      [DataMember]
      public int AllocationID { get; set; }
      [DataMember]
      public int ReservationID { get; set; }
      [DataMember]
      public int FundID { get; set; }
      [DataMember]
      public decimal Amount { get; set; }
      [DataMember]
      public bool Pushed { get; set; }

      public virtual Reservation Reservation { get; set; }
      public virtual Fund Fund { get; set; }
      #endregion

      #region Members.IIdentifiableEntity
      int IIdentifiableEntity.EntityID
      {
         get { return AllocationID; }
         set { AllocationID = value; }
      }
      #endregion
   }
}
