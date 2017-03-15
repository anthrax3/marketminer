using MarketMiner.Business.Entities;
using P.Core.Common.Contracts;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IProductRepository : IDataRepository<Product>
   {
      IEnumerable<Product> GetProducts(bool activesOnly = true);
      IEnumerable<Product> GetProductByCode(string code);
   }
}
