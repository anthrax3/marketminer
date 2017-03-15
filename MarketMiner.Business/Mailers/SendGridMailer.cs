using MarketMiner.Business.Contracts;
using MarketMiner.Common;
using MarketMiner.Data.Contracts;
using P.Core.Common.Contracts;
using SendGrid.Helpers.Mail;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMiner.Business.Common
{
   [Export("SendGridMailer", typeof(IMailer))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   class SendGridMailer : MailerBase
   {
      [ImportingConstructor]
      public SendGridMailer(IDataRepositoryFactory dataRepositoryFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
      }

      protected override bool SendMail(EmailCommunication email)
      {
         return SendMessage(email).Result;
      }

      private async Task<bool> SendMessage(EmailCommunication email)
      {
         try
         {
            Mail mail = new Mail();
            mail.From = new Email(email.Sender);
            mail.Subject = email.Subject;
            mail.AddContent(new Content("text/plain", email.Body));

            AddRecipients(email, mail);
            AddAttachments(email, mail);

            string type = Constants.MetaSettings.Types.Api;
            string code = Constants.MetaSettings.Codes.SendGridApiKey;

            IMetaSettingRepository settingRepository = _DataRepositoryFactory.GetDataRepository<IMetaSettingRepository>();
            string apiKey = settingRepository.GetSetting(type, code).Value;

            dynamic sg = new SendGrid.SendGridAPIClient(apiKey, "https://api.sendgrid.com");

            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());

            return true;
         }
         catch (Exception e)
         {
            throw e;
         }
      }

      private void AddRecipients(EmailCommunication email, Mail mail)
      {
         Personalization personalization = new Personalization();

         string[] recipients = email.Recipients.Split(';');
         recipients.ToList().ForEach(r => personalization.AddTo(new Email(r)));

         if (email.CcRecipients != null)
            email.CcRecipients.ToList().ForEach(r => personalization.AddCc(new Email(r)));

         if (email.BccRecipients != null)
            email.BccRecipients.ToList().ForEach(r => personalization.AddBcc(new Email(r)));

         mail.AddPersonalization(personalization);
      }

      private void AddAttachments(EmailCommunication email, Mail mail)
      {
         if (email.Attachments != null)
         {
            foreach (string filePath in email.Attachments)
            {
               string fileContents = Convert.ToBase64String(File.ReadAllBytes(filePath));

               string contentType = "application/octet-stream";
               //new FileExtensionContentTypeProvider().TryGetContentType(filePath, out contentType);
               //contentType = contentType ?? "application/octet-stream";

               Attachment attachment = new Attachment();
               attachment.Content = fileContents;
               attachment.Type = contentType;
               attachment.Filename = Path.GetFileName(filePath);
               attachment.Disposition = "attachment";
               mail.AddAttachment(attachment);
            }
         }
      }

      public override void Dispose()
      {
         //
      }
   }
}
