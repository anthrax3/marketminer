using MarketMiner.Business.Entities;
using P.Core.Common.Contracts;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface ICommunicationTemplateRepository : IDataRepository<CommunicationTemplate>
   {
      IEnumerable<CommunicationTemplate> GetCommunicationTemplateByName(string name);
   }
}
