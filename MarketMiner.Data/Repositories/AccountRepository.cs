using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IAccountRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class AccountRepository : MarketMinerRepositoryBase<Account>, IAccountRepository
   {
      #region Members.Override

      protected override Account AddEntity(MarketMinerContext entityContext, Account entity)
      {
         return entityContext.AccountSet.Add(entity);
      }

      protected override Account UpdateEntity(MarketMinerContext entityContext, Account entity)
      {
         return (from e in entityContext.AccountSet
                 where e.AccountID == entity.AccountID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<Account> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.AccountSet
                select e;
      }

      protected override Account GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.AccountSet
                      where e.AccountID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.IAcountRepository

      public Account GetByLogin(string login)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            return (from a in entityContext.AccountSet
                    where a.LoginEmail == login
                    select a).FirstOrDefault();
         }
      }

      public Account GetById(int accountId)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            return (from a in entityContext.AccountSet
                    where a.AccountID == accountId
                    select a).FirstOrDefault();
         }
      }
      #endregion
   }
}
