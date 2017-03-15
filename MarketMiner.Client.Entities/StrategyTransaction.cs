using MarketMiner.Common.Contracts;
using N.Core.Common.Core;
using System;

namespace MarketMiner.Client.Entities
{
   public class StrategyTransaction : ObjectBase, ITransaction
   {
      int _strategyTransactionID { get; set; }
      int _strategyID { get; set; }
      int _brokerID { get; set; }
      string _accountID { get; set; }
      string _brokerTransactionID { get; set; }
      string _brokerOrderID { get; set; }
      string _brokerTradeID { get; set; }
      string _instrument { get; set; }
      string _type { get; set; }
      DateTime? _time { get; set; }
      string _side { get; set; }
      double _price { get; set; }
      double? _takeProfit { get; set; }
      double? _stopLoss { get; set; }

      public int StrategyTransactionID
      {
         get { return _strategyTransactionID; }
         set { if (_strategyTransactionID != value) { _strategyTransactionID = value; OnPropertyChanged(); } }
      }
      public int StrategyID
      {
         get { return _strategyID; }
         set { if (_strategyID != value) { _strategyID = value; OnPropertyChanged(); } }
      }
      public int BrokerID
      {
         get { return _brokerID; }
         set { if (_brokerID != value) { _brokerID = value; OnPropertyChanged(); } }
      }
      public string AccountID
      {
         get { return _accountID; }
         set { if (_accountID != value) { _accountID = value; OnPropertyChanged(); } }
      }
      public string BrokerTransactionID
      {
         get { return _brokerTransactionID; }
         set { if (_brokerTransactionID != value) { _brokerTransactionID = value; OnPropertyChanged(); } }
      }
      public string BrokerOrderID
      {
         get { return _brokerOrderID; }
         set { if (_brokerOrderID != value) { _brokerOrderID = value; OnPropertyChanged(); } }
      }
      public string BrokerTradeID
      {
         get { return _brokerTradeID; }
         set { if (_brokerTradeID != value) { _brokerTradeID = value; OnPropertyChanged(); } }
      }
      public string Instrument
      {
         get { return _instrument; }
         set { if (_instrument != value) { _instrument = value; OnPropertyChanged(); } }
      }
      public string Type
      {
         get { return _type; }
         set { if (_type != value) { _type = value; OnPropertyChanged(); } }
      }
      public DateTime? Time
      {
         get { return _time; }
         set { if (_time != value) { _time = value; OnPropertyChanged(); } }
      }
      public string Side
      {
         get { return _side; }
         set { if (_side != value) { _side = value; OnPropertyChanged(); } }
      }
      public double Price
      {
         get { return _price; }
         set { if (_price != value) { _price = value; OnPropertyChanged(); } }
      }
      public double? TakeProfit
      {
         get { return _takeProfit; }
         set { if (_takeProfit != value) { _takeProfit = value; OnPropertyChanged(); } }
      }
      public double? StopLoss
      {
         get { return _stopLoss; }
         set { if (_stopLoss != value) { _stopLoss = value; OnPropertyChanged(); } }
      }
   }
}
