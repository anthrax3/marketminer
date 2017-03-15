using System;
namespace MarketMiner.Common.Contracts
{
    public interface ITransaction
    {
       string BrokerTransactionID { get; set; }
    }
}
