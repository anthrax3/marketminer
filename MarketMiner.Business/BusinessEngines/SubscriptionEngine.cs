using MarketMiner.Business.Contracts;
using MarketMiner.Business.Entities;
using MarketMiner.Common;
using MarketMiner.Data.Contracts;
using N.Core.Common.Data;
using P.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Business.BusinessEngines
{
   [Export(typeof(ISubscriptionEngine))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class SubscriptionEngine : BusinessEngineBase, ISubscriptionEngine
   {
      #region Constructors
      [ImportingConstructor]
      public SubscriptionEngine(IDataRepositoryFactory dataRepositoryFactory, IMailerFactory mailerFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
         _MailerFactory = mailerFactory;
      }
      #endregion

      public Signal PushSignalToSubscribers(int signalId)
      {
         Signal pushedEntity = null;

         IMetaSettingRepository settingsRepository = _DataRepositoryFactory.GetDataRepository<IMetaSettingRepository>();
         string type = Constants.MetaSettings.Types.Communication;
         string code = Constants.MetaSettings.Codes.EmailSender;
         string name = settingsRepository.GetSetting(type, code).Value;

         using (IMailer mailer = _MailerFactory.GetMailer(name))
         using (IDbContextRepository<Signal> signalRepository = _DataRepositoryFactory.GetDataRepository<ISignalRepository>())
         {
            Signal signal = signalRepository.EntitySet.FirstOrDefault(s => s.SignalID == signalId);

            List<Subscription> subscriptions = signal.Product.Subscriptions;

            // send signal to subscribers
            foreach (var subscription in subscriptions)
            {
               string sender = "noreply@marketminerllc.com";
               string recipients = subscription.Account.LoginEmail;
               string subject = string.Format("Alert - New MarketMiner Signal - {0} - {1}", signal.Instrument, signal.Granularity);
               string side = signal.Side == "S" ? "SELL" : signal.Side == "B" ? "BUY" : "NONE";
               DateTime time = Convert.ToDateTime(signal.SignalTime).ToUniversalTime();
               string body = string.Format("New Signal - {0} : {1} : {2} : {3} : {4} : {5}", signal.Type, side, signal.Instrument, signal.Granularity, signal.SignalPrice, time);

               mailer.SendMail(sender, recipients, subject, body);
            }
            
            // postmark the signal
            signal.SendPostmark = DateTime.UtcNow;

            signalRepository.Context.SaveChanges();

            pushedEntity = signalRepository.Entity(signal);
         }

         return pushedEntity;
      }
   }
}
