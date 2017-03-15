using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using P.Core.Common.Extensions;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(ISubscriptionRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class SubscriptionRepository : MarketMinerRepositoryBase<Subscription>, ISubscriptionRepository
   {
      #region Members.Override
      protected override Subscription AddEntity(MarketMinerContext entityContext, Subscription entity)
      {
         return entityContext.SubscriptionSet.Add(entity);
      }

      protected override Subscription UpdateEntity(MarketMinerContext entityContext, Subscription entity)
      {
         return (from e in entityContext.SubscriptionSet
                 where e.SubscriptionID == entity.SubscriptionID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<Subscription> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.SubscriptionSet
                select e;
      }

      protected override Subscription GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.SubscriptionSet
                      where e.SubscriptionID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.ISubscriptionRepository

      public IEnumerable<Subscription> GetSubscriptionsByInstrument(string instrument)
      {
         throw new System.NotImplementedException();
      }

      public IEnumerable<AccountSubscriptionInfo> GetAccountSubscriptionsByProduct(int productId)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from s in entityContext.SubscriptionSet
                        join a in entityContext.AccountSet
                           on s.AccountID equals a.AccountID
                        where s.ProductID == productId
                           && s.Active == true
                        select new AccountSubscriptionInfo()
                        {
                           Account = a,
                           Subscription = s
                        };

            return query.ToFullyLoaded();
         }
      }

      public IEnumerable<AccountSubscriptionInfo> GetCurrentAccountSubscriptionInfo()
      {
         throw new System.NotImplementedException();
      }

      public IEnumerable<AccountSubscriptionInfo> GetAccountOpenSubscriptionInfo(int accountId)
      {
         throw new System.NotImplementedException();
      }
      #endregion
   }
}
