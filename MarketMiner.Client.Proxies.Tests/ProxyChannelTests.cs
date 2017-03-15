using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;

namespace MarketMiner.Client.Proxies.Tests
{
   [TestClass]
   public class ProxyChannelTests
   {
      [TestMethod]
      public void test_inventory_client_connection()
      {
         AccountClient proxy = new AccountClient();
         test_client_connection(proxy as ICommunicationObject, "AccountClient");
      }

      [TestMethod]
      public void test_metadata_client_connection()
      {
         MetadataClient proxy = new MetadataClient();
         test_client_connection(proxy as ICommunicationObject, "MetadataClient");
      }

      [TestMethod]
      public void test_participation_client_connection()
      {
         ParticipationClient proxy = new ParticipationClient();
         test_client_connection(proxy as ICommunicationObject, "ParticipationClient");
      }

      [TestMethod]
      public void test_strategy_client_connection()
      {
         StrategyClient proxy = new StrategyClient();
         test_client_connection(proxy as ICommunicationObject, "StrategyClient");
      }

      [TestMethod]
      public void test_subscription_client_connection()
      {
         SubscriptionClient proxy = new SubscriptionClient();
         test_client_connection(proxy as ICommunicationObject, "SubscriptionClient");
      }

      #region Utilities
      private void test_client_connection(ICommunicationObject channel, string clientName)
      {
         channel.Open();

         Assert.IsTrue(channel != null, string.Format("{0} proxy channel is null.", clientName));
         Assert.IsTrue(channel.State == CommunicationState.Opened, string.Format("{0} proxy channel did not open.", clientName));

         channel.Close();
      }
      #endregion
   }
}
