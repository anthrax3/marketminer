namespace MarketMiner.Common.Enums
{
   public enum CommunicationTemplate
   {
      EmailSignalNew,
      EmailSignalUpdate,
      LetterRedemptionApproval,
      LetterWelcomeToFundPackage
   }
   public enum CommunicationSendType
   {
      Email,
      LetterNextDay,
      LetterTwoDay,
      LetterThreeDay
   }

   public enum AlgorithmStatus
   {
      NotRunning,
      Starting,
      Running,
      RunningSuspended
   }
}
