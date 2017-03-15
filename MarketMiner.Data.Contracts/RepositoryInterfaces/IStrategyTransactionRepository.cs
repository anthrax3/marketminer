using MarketMiner.Business.Entities;
using P.Core.Common.Contracts;
using System;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IStrategyTransactionRepository : IDataRepository<StrategyTransaction>
   {
      IEnumerable<StrategyTransaction> GetStrategyTransactions();
      IEnumerable<StrategyTransaction> GetStrategyTransactionsByBroker(int brokerId);
      IEnumerable<StrategyTransaction> GetStrategyTransactionsByInstrument(string instrument);
      IEnumerable<StrategyTransaction> GetStrategyTransactionsBySide(string side);
      IEnumerable<StrategyTransaction> GetStrategyTransactionsByDate(DateTime dateCreated);
      IEnumerable<StrategyTransaction> GetStrategyTransactionsByDateRange(DateTime dateBottom, DateTime dateTop);
   }
}
