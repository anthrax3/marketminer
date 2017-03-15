using MarketMiner.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MarketMiner.Algorithm.Common
{
    public class AlgorithmReport
    {
        #region Declarations
        List<ITransaction> _transactions = new List<ITransaction>();
        Dictionary<string, string> _mutableMessages = new Dictionary<string, string>();
        #endregion

        #region Properties
        public ReadOnlyDictionary<string, string> Messages
        {
            get { return new ReadOnlyDictionary<string, string>(_mutableMessages); }
        }

        public IReadOnlyList<ITransaction> Transactions
        {
            get { return (IReadOnlyList<ITransaction>)_transactions; }
        }
        #endregion

        #region Methods
        public void Add(ITransaction transaction)
        {
            _transactions.Add(transaction);
        }

        public void Add(string message)
        {
            _mutableMessages.Add(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ':' + _mutableMessages.Count, message);
        }
        #endregion
    }
}
