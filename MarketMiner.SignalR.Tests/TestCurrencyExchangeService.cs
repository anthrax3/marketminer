using MarketMiner.Algorithm.Demo;
using MarketMiner.Host.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using N.Core.Common.Misc;
using System;
using System.Collections.Generic;

namespace MarketMiner.SignalR.Tests
{
    [TestClass]
    public class TestCurrencyExchangeService : UnitTestBase
    {
        #region initialize and cleanup
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            _service = MarketMinerSignalR.Instance;
            PrivateObject privateObject = new PrivateObject(_service);
            privateObject.Invoke("OnStart", new object[] { null });
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }
        #endregion

        static MarketMinerSignalR _service;

        [TestMethod]
        public void TestClientGetMarketStateFromHub()
        {
            // Make sure to call WebApp.Start:
            //PrivateObject privateObject = new PrivateObject(_service);
            //privateObject.Invoke("OnStart", new object[] { null });

            // Create client proxy and call hub method
            using (HubConnection hub = new HubConnection(String.Format("http://{0}:8080", "localhost")))
            {
                IHubProxy proxy = hub.CreateHubProxy("CurrencyExchangeHub");
                hub.Start().Wait();

                var state = proxy.Invoke<string>("GetMarketState").Result;
                Assert.IsNotNull(state);
                Assert.IsTrue(state.Length > 0);
            }
        }

        [TestMethod]
        public void TestClientGetAllCurrenciesFromHub()
        {
            // Make sure to call WebApp.Start
           //PrivateObject privateObject = new PrivateObject(_service);
            //privateObject.Invoke("OnStart", new object[] { null });

            // Create client proxy and call hub method
            using (HubConnection hub = new HubConnection(String.Format("http://{0}:8080", "localhost")))
            {
                IHubProxy proxy = hub.CreateHubProxy("CurrencyExchangeHub");
                hub.Start().Wait();

                var currencies = proxy.Invoke<IEnumerable<Currency>>("GetAllCurrencies").Result;
                Assert.IsNotNull(currencies);
                Assert.IsTrue(currencies.ToString().Length > 0);
            }
        }

        [TestMethod]
        public void TestClientOpenCloseMarketFromHub()
        {
            // Make sure to call WebApp.Start:
           //PrivateObject privateObject = new PrivateObject(_service);
            //privateObject.Invoke("OnStart", new object[] { null });

            // Create client proxy and call hub method
            using (HubConnection hub = new HubConnection(String.Format("http://{0}:8080", "localhost")))
            {
                IHubProxy proxy = hub.CreateHubProxy("CurrencyExchangeHub");
                hub.Start().Wait();

                var state = proxy.Invoke<bool>("OpenMarket").Result;
                Assert.IsNotNull(state);
                Assert.IsTrue(state == true);

                state = proxy.Invoke<bool>("CloseMarket").Result;
                Assert.IsNotNull(state);
                Assert.IsTrue(state == true);
            }
        }

        [TestMethod]
        public void TestGetMarketStateFromHub()
        {
            CurrencyExchangeHub hub = new CurrencyExchangeHub(MarketMinerSignalR.Instance);
            var state = hub.GetMarketState();
            Assert.IsNotNull(state);
        }

        [TestMethod]
        public void TestOpenCloseMarket()
        {
            var currencies = MarketMinerSignalR.Instance.GetAllCurrencies();
            Assert.IsNotNull(currencies);
            bool expected = true;
            bool actual = MarketMinerSignalR.Instance.OpenMarket();
            Assert.AreEqual(expected, actual);
            actual = MarketMinerSignalR.Instance.OpenMarket();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestOpenCloseMarketFromHub()
        {
            var hub = new CurrencyExchangeHub(MarketMinerSignalR.Instance);
            var currencies = hub.GetAllCurrencies();
            Assert.IsNotNull(currencies);
            bool expected = true;
            bool actual = hub.OpenMarket();
            Assert.AreEqual(expected, actual);
            actual = hub.OpenMarket();
            Assert.AreEqual(expected, actual);
        }
    }
}
