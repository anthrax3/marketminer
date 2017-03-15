namespace MarketMiner.Common.Enums
{
   public static class EnumExtensions
   {
      public static string AsAlgorithmStatusString(this short? status)
      {
         switch ((AlgorithmStatus)status.GetValueOrDefault())
         {
            case AlgorithmStatus.NotRunning: return AlgorithmStatus.NotRunning.ToString("f");
            case AlgorithmStatus.Starting: return AlgorithmStatus.Starting.ToString("f");
            case AlgorithmStatus.Running: return AlgorithmStatus.Running.ToString("f");
            case AlgorithmStatus.RunningSuspended: return AlgorithmStatus.RunningSuspended.ToString("f");
            default: return null;
         }
      }
   }
}
