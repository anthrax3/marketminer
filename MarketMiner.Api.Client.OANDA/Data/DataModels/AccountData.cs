using MarketMiner.Api.OANDA;
using MarketMiner.Api.OANDA.Framework;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using P.Core.Common.Core;
using P.Core.Common.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MarketMiner.Api.Client.OANDA.Data.DataModels
{
    public class AccountData : NotificationObject
    {
        long _currentTrans;

        public AccountData(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }

        public AccountDetails Details { get; set; }

        private ObservableCollection<Position> _positions;
        private ObservableCollection<Order> _orders;
        private ObservableCollection<TradeData> _trades;
        private ObservableCollection<Transaction> _transactions;

        public ObservableCollection<Position> Positions { get { return _positions; } set { _positions = value; OnPropertyChanged("Positions"); } }
        public ObservableCollection<Order> Orders { get { return _orders; } set { _orders = value; OnPropertyChanged("Orders"); } }
        public ObservableCollection<TradeData> Trades { get { return _trades; } set { _trades = value; OnPropertyChanged("Trades"); } }
        public ObservableCollection<Transaction> Transactions { get { return _transactions; } set { _transactions = value; OnPropertyChanged("Transactions"); } }

        public event EventHandler<CustomEventArgs<Transaction>> NewTransaction;

        public void OnNewTransaction(Transaction e)
        {
            EventHandler<CustomEventArgs<Transaction>> handler = NewTransaction;
            if (handler != null) handler(this, new CustomEventArgs<Transaction>(e));
        }

        public ObservableCollection<T> GetObservable<T>(List<T> list)
        {
            var collection = new ObservableCollection<T>();
            foreach (var item in list)
            {
                collection.Add(item);
            }
            return collection;
        }

        #region Object refresh

        TaskTimer _taskTimer;

        public virtual void EnableUpdates()
        {
            if (_taskTimer == null)
            {
                _taskTimer = new TaskTimer(Refresh, null, 0, 100, true);
            }
        }

        protected virtual void Refresh(object sender, object e)
        {
            Refresh();
        }

        /// <summary>
        /// Refreshes account data using state parameters if provided
        /// </summary>
        /// <param name="state"></param>
        protected virtual void Refresh(object state)
        {
            Refresh();
        }

        public virtual async void Refresh()
        {
            var transParams = new Dictionary<string, string> {{"minId", "" + (_currentTrans + 1)}};
            var newTransactions = await Rest.GetTransactionListAsync(Id, transParams);
            if ( newTransactions.Count > 0 )
            {
                _currentTrans = newTransactions[0].id;
                foreach (var newTransaction in newTransactions)
                {
                    OnNewTransaction(newTransaction);
                }

                // these can't change unless there's been a transaction...
                Trades = GetObservable(await Rest.GetTradeListAsync(Id));
                Orders = GetObservable(await Rest.GetOrderListAsync(Id));
                Transactions = GetObservable(await Rest.GetTransactionListAsync(Id));
                Positions = GetObservable(await Rest.GetPositionsAsync(Id));
            }
        }
        #endregion
    }
}
