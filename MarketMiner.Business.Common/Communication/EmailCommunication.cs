using MarketMiner.Common.Enums;
using System.Collections.Generic;

namespace MarketMiner.Business.Common
{
   public class EmailCommunication : CommunicationBase
   {
      public EmailCommunication()
      {
         SendType = (short)CommunicationSendType.Email;
      }

      public string Sender { get; set; }
      public string Recipients { get; set; }
      public IEnumerable<string> CcRecipients { get; set; }
      public IEnumerable<string> BccRecipients { get; set; }
   }
}
