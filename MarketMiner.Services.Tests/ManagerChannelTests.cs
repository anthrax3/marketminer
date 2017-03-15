using MarketMiner.Business.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;

namespace MarketMiner.Services.Tests
{
   [TestClass]
   public class ManagerChannelTests
   {
      [TestMethod]
      public void test_account_manager_as_service()
      {
         ChannelFactory<IAccountService> channelFactory = new ChannelFactory<IAccountService>("");

         IAccountService proxy = channelFactory.CreateChannel();

         test_manager_as_service(proxy as ICommunicationObject, "IAccountService");

         channelFactory.Close();
      }

      public void test_metadata_manager_as_service()
      {
         ChannelFactory<IMetadataService> channelFactory = new ChannelFactory<IMetadataService>("");

         IMetadataService proxy = channelFactory.CreateChannel();

         test_manager_as_service(proxy as ICommunicationObject, "IMetadataService");

         channelFactory.Close();
      }

      [TestMethod]
      public void test_participation_manager_as_service()
      {
         ChannelFactory<IParticipationService> channelFactory = new ChannelFactory<IParticipationService>("");

         IParticipationService proxy = channelFactory.CreateChannel();

         test_manager_as_service(proxy as ICommunicationObject, "IParticipationService");

         channelFactory.Close();
      }

      [TestMethod]
      public void test_strategy_manager_as_service()
      {
         ChannelFactory<IStrategyService> channelFactory = new ChannelFactory<IStrategyService>("");

         IStrategyService proxy = channelFactory.CreateChannel();

         test_manager_as_service(proxy as ICommunicationObject, "IStrategyService");

         channelFactory.Close();
      }

      [TestMethod]
      public void test_subscription_manager_as_service()
      {
         ChannelFactory<ISubscriptionService> channelFactory = new ChannelFactory<ISubscriptionService>("");

         ISubscriptionService proxy = channelFactory.CreateChannel();

         test_manager_as_service(proxy as ICommunicationObject, "ISubscriptionService");

         channelFactory.Close();
      }

      #region Utilities
      private void test_manager_as_service(ICommunicationObject channel, string managerName)
      {
         channel.Open();

         Assert.IsTrue(channel != null, string.Format("{0} channel is null.", managerName));
         Assert.IsTrue(channel.State == CommunicationState.Opened, string.Format("{0} channel did not open.", managerName));

         channel.Close();
      }
      #endregion
   }
}
