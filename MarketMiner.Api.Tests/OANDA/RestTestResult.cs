using P.Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MarketMiner.Api.Tests.OANDA
{
    public class RestTestResult
    {
        public bool Success { get; set; }
        public string Details { get; set; }
    }

    public class RestTestResults : NotificationObject
    {
        #region Declarations
        string _lastMessage;
        List<RestTestResult> _results = new List<RestTestResult>();
        Dictionary<string, string> _mutableMessages = new Dictionary<string, string>();
        #endregion

        #region Public properties and methods
        public ReadOnlyDictionary<string, string> Messages
        {
            get { return new ReadOnlyDictionary<string, string>(_mutableMessages); }
        }

        public string LastMessage
        {
            get { return _lastMessage; }
        }

        //------
        public bool Verify(string success, string testDescription)
        {
            return Verify(!string.IsNullOrEmpty(success), testDescription);
        }

        public bool Verify(bool success, string testDescription)
        {
            _results.Add(new RestTestResult { Success = success, Details = testDescription });
            if (!success)
            {
                Add(success + ": " + testDescription); // add message
            }
            return success;
        }

        //------
        public void Add(RestTestResult testResult)
        {
            _results.Add(testResult);
        }

        public void Add(string message)
        {
            _lastMessage = message;
            _mutableMessages.Add(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ':' + _mutableMessages.Count, message);
            OnPropertyChanged("Messages");
        }
        #endregion
    }
}
