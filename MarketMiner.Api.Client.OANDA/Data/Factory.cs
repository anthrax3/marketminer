using MarketMiner.Api.Client.OANDA.Data.DataModels;
using MarketMiner.Api.Client.OANDA.ViewModels;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using System;
using System.Collections.Generic;

namespace MarketMiner.Api.Client.OANDA.Data
{
    public class Factory
    {
        public static List<DataItem> GetDataItems<T>(List<T> items, DataGroup group)
        {
            var list = new List<DataItem>();
            foreach (dynamic item in items)
            {
                list.Add(GetViewModel(item, group));
            }
            return list;
        }

        public static DataItem GetViewModel(TradeData trade, DataGroup group)
        {
            return new TradeViewModel(trade, group);
        }
        
        public static DataItem GetViewModel(Order order, DataGroup group)
        {
            return new OrderViewModel(order, group);
        }

        public static DataItem GetViewModel(Transaction trans, DataGroup group)
        {
            return new TransactionViewModel(trans, group);
        }

        public static DataItem GetViewModel(Position trans, DataGroup group)
        {
            return new PositionViewModel(trans, group);
        }

        public static DataItem GetViewModel(Price price, DataGroup group)
        {
            return new PriceViewModel(price, group);
        }

        public static DataItem GetViewModel(object obj, DataGroup group)
        {
            throw new NotImplementedException();
        }
    }
}
