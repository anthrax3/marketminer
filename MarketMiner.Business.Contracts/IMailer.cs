using System;
using System.Collections.Generic;

namespace MarketMiner.Business.Contracts
{
   public interface IMailer : IDisposable
   {
      bool SendMail(string sender, string recipients, string subject, string body);
      bool SendMail(string sender, string recipients, string subject, string body, IEnumerable<string> ccRecipients);
      bool SendMail(string sender, string recipients, string subject, string body, IEnumerable<string> ccRecipients, IEnumerable<string> bccRecipients);
      bool SendMail(string sender, string recipients, string subject, string body, IEnumerable<string> ccRecipients, IEnumerable<string> bccRecipients, IEnumerable<string> attachments);
   }
}
