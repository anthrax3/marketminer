using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using System.Collections.Generic;

namespace MarketMiner.Api.Client.OANDA.Data.DataModels
{
    public class UserData
    {
        public List<Account> Accounts { get; private set;  }

        //public AccountData SelectedAccount { get; }
    }
}
