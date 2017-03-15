using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarketMiner.Tests
{
   [TestClass]
   public abstract class UnitTestBase
   {
      /// <summary>
      /// Runs code to initialize state before the first test is invoked.
      /// </summary>
      [ClassInitialize()]
      public virtual void MyClassInitialize(TestContext testContext)
      {
      }

      /// <summary>
      /// Runs code to clean up state after all tests are completed.
      /// </summary>
      [ClassCleanup()]
      public virtual void MyClassCleanup()
      {
      }

      /// <summary>
      /// Runs code to initialize state before invoking a test.
      /// </summary>
      [TestInitialize()]
      public virtual void MyTestInitialize()
      {
      }

      /// <summary>
      /// Runs code to clean up state after a test.
      /// </summary>
      [TestCleanup()]
      public virtual void MyTestCleanup()
      {
      }
   }
}
