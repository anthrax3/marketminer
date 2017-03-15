using MarketMiner.Algorithm.Common;
using MarketMiner.Api.OANDA;
using MarketMiner.Api.OANDA.TradeLibrary.DataTypes.Communications.Requests;
using P.MarketMiner.Client.Entities;
using System;
using System.Threading.Tasks;
using OANDAExt = MarketMiner.Api.OANDA.Extensions;

namespace MarketMiner.Algorithm.OANDA.Currencies
{
    [AlgorithmModule(Name = "USDJPYAlgorithm")]
    public class USDJPYAlgorithm : AlgorithmBase
    {
        private string _instrument = OANDAExt.Constants.Instruments.USDJPY;

        public USDJPYAlgorithm(int accountId, Strategy strategy)
           : base(strategy)
        {
        }

        public override async Task<bool> Start()
        {
           await base.Start();
            _shutdown = false;


            return true;
        }
    }
}
