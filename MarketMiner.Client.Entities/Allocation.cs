using N.Core.Common.Core;

namespace MarketMiner.Client.Entities
{
   public class Allocation : ObjectBase
   {
      #region Properties

      int _allocationId;
      int _strategyId;
      double _amount;

      #endregion

      #region Accessors

      public int AllocationId
      {
         get { return _allocationId; }
         set
         {
            if (_allocationId != value)
            {
               _allocationId = value;
               OnPropertyChanged(() => AllocationId);
            }
         }
      }

      public int StrategyId
      {
         get { return _strategyId; }
         set
         {
            if (_strategyId != value)
            {
               _strategyId = value;
               OnPropertyChanged(() => StrategyId);
            }
         }
      }

      public double Amount
      {
         get { return _amount; }
         set
         {
            if (_amount != value)
            {
               _amount = value;
               OnPropertyChanged(() => Amount);
            }
         }
      }
      #endregion
   }
}
