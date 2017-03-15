using MarketMiner.Business.Contracts;
using P.Core.Common.Contracts;
using System.Collections.Generic;

namespace MarketMiner.Business.Common
{
   abstract class MailerBase : IMailer
   {
      protected IDataRepositoryFactory _DataRepositoryFactory;

      protected abstract bool SendMail(EmailCommunication email);

      public virtual bool SendMail(string sender, string recipients, string subject, string body, IEnumerable<string> ccRecipients, IEnumerable<string> bccRecipients, IEnumerable<string> attachments)
      {
         EmailCommunication email = new EmailCommunication()
         {
            Sender = sender,
            Recipients = recipients,
            Subject = subject,
            Body = body,
            CcRecipients = ccRecipients,
            BccRecipients = bccRecipients,
            Attachments = attachments
         };

         return SendMail(email);
      }

      public bool SendMail(string sender, string recipients, string subject, string body)
      {
         return SendMail(sender, recipients, subject, body, null, null, null);
      }

      public bool SendMail(string sender, string recipients, string subject, string body, IEnumerable<string> ccRecipients)
      {
         return SendMail(sender, recipients, subject, body, ccRecipients, null, null);
      }

      public bool SendMail(string sender, string recipients, string subject, string body, IEnumerable<string> ccRecipients, IEnumerable<string> bccRecipients)
      {
         return SendMail(sender, recipients, subject, body, ccRecipients, bccRecipients, null);
      }

      public abstract void Dispose();
   }
}
