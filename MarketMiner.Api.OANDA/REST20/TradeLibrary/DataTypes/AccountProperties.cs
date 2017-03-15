using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMiner.Api.OANDA.REST20.TradeLibrary.DataTypes
{
   public class AccountProperties: Definition
   {
      string id;
      int mt4AccountID;
      string[] tags;
   }
}
