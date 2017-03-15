using MarketMiner.Business.Entities;
using N.Core.Common.Data;
using System;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface ISignalRepository : IDbContextRepository<Signal>
   {
      IEnumerable<Signal> GetSignals(bool activesOnly = false);
      IEnumerable<Signal> GetSignalsByType(string type, bool activesOnly = false);
      IEnumerable<Signal> GetSignalsByInstrument(string instrument, bool activesOnly = false);
      IEnumerable<Signal> GetSignalsByGranularity(string granularity, bool activesOnly = false);
      IEnumerable<Signal> GetSignalsByDate(DateTime dateCreated);
      IEnumerable<Signal> GetSignalsByDateRange(DateTime dateBottom, DateTime dateTop);
   }
}
