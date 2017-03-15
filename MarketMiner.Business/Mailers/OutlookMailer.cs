using MarketMiner.Business.Contracts;
using P.Core.Common.Contracts;
using System;
using System.ComponentModel.Composition;
//using Outlook = Microsoft.Office.Interop.Outlook;

namespace MarketMiner.Business.Common
{
   [Export("OutlookMailer", typeof(IMailer))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   class OutlookMailer : MailerBase
   {
      //Outlook.Application _outlook;

      [ImportingConstructor]
      public OutlookMailer(IDataRepositoryFactory dataRepositoryFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
      }

      protected override bool SendMail(EmailCommunication email)
      {
         OutlookEmailCommunication outlookEmail = (OutlookEmailCommunication)email;

         try
         {
            // need to install Outlook on the machine/server for this to work
            // commenting out the code until/if Outlook is installed

            //// create the outlook application.
            //_outlook = new Outlook.Application();
            //if (_outlook == null)
            //   throw new Exception("Outlook App creation failed.");

            //// create a new mail item.
            //Outlook.MailItem mailItem = _outlook.CreateItem(Outlook.OlItemType.olMailItem) as Outlook.MailItem;

            //// compose email
            //mailItem.HTMLBody = email.Body;

            //if (email.Attachments != null)
            //{
            //   foreach (string file in email.Attachments)
            //   {
            //      Outlook.Attachment attachment = mailItem.Attachments.Add(file);
            //   }
            //}

            //mailItem.Subject = email.Subject;
            //mailItem.To = email.Recipients;

            //if (outlookEmail.OutlookSendType == OutlookSendType.SendDirect)
            //   ((Outlook._MailItem)mailItem).Send();
            //else if (outlookEmail.OutlookSendType == OutlookSendType.ShowModal)
            //   mailItem.Display(true);
            //else if (outlookEmail.OutlookSendType == OutlookSendType.ShowModeless)
            //   mailItem.Display(false);

            //mailItem = null;

            return true;
         }
         catch (Exception e)
         {
            throw e;
         }
      }

      public override void Dispose()
      {
         //_outlook.Application.Quit();
         //_outlook = null;
      }
   }
}
