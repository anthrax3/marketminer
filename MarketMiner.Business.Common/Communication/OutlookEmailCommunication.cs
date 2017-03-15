using MarketMiner.Common.Enums;
using System.Collections.Generic;

namespace MarketMiner.Business.Common
{
   public class OutlookEmailCommunication : EmailCommunication
   {
      public OutlookSendType OutlookSendType { get; set; }

      public OutlookEmailCommunication()
      {
         OutlookSendType = OutlookSendType.SendDirect;
      }
   }

   public enum OutlookSendType
   {
      SendDirect = 0,
      ShowModal = 1,
      ShowModeless = 2
   }
}
