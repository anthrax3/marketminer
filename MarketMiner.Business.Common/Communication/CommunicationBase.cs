using MarketMiner.Business.Entities;
using System.Collections.Generic;

namespace MarketMiner.Business.Common
{
   public abstract class CommunicationBase : Communication
   {
      public string Subject { get; set; }
      public string Body { get; set; }

      /// <summary>
      /// The filepaths to the attachments to the email or the enclosures in the letter.
      /// </summary>
      public IEnumerable<string> Attachments { get; set; }
   }
}
