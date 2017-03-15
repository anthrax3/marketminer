using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IProductRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class ProductRepository : MarketMinerRepositoryBase<Product>, IProductRepository
   {
      #region Members.Override
      protected override Product AddEntity(MarketMinerContext entityContext, Product entity)
      {
         return entityContext.ProductSet.Add(entity);
      }

      protected override Product UpdateEntity(MarketMinerContext entityContext, Product entity)
      {
         return (from e in entityContext.ProductSet
                 where e.ProductID == entity.ProductID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<Product> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.ProductSet
                select e;
      }

      protected override Product GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.ProductSet
                      where e.ProductID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.IProductRepository
      public IEnumerable<Product> GetProducts(bool activesOnly = true)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            if (activesOnly)
               return GetEntities(entityContext).Where(e => e.Active == true).ToArray().ToList();
            else
               return GetEntities(entityContext).ToArray().ToList();
         }
      }

      public IEnumerable<Product> GetProductByCode(string code)
      {
         return GetProducts().Where(p => p.Code == code);
      }
      #endregion
   }
}
