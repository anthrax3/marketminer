using MarketMiner.Business.Contracts;
using MarketMiner.Business.Entities;
using MarketMiner.Common;
using MarketMiner.Common.Enums;
using MarketMiner.Data.Contracts;
using N.Core.Common.Data;
using N.Core.Common.Threading;
using P.Core.Common.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Business
{
   [Export(typeof(IStrategyEngine))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class StrategyEngine : BusinessEngineBase, IStrategyEngine
   {
      #region Constructors
      [ImportingConstructor]
      public StrategyEngine(IDataRepositoryFactory dataRepositoryFactory, IMailerFactory mailerFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
         _MailerFactory = mailerFactory;
      }
      #endregion

      static List<string> _notifications;
      static DataFlowTaskFactory _notificationsSender;
      public void PostAlgorithmStatusNotification(string message)
      {
         if (_notifications == null)
         {
            _notifications = new List<string>();
            _notificationsSender = new DataFlowTaskFactory();
            _notificationsSender.StartWork(SendAlgorithmStatusNotifications, null, 3600);
         }

         _notifications.Add(message);
      }

      private void SendAlgorithmStatusNotifications(object state)
      {
         IMetaSettingRepository settingsRepository = _DataRepositoryFactory.GetDataRepository<IMetaSettingRepository>();
         string type = Constants.MetaSettings.Types.Communication;
         string code = Constants.MetaSettings.Codes.EmailSender;
         string name = settingsRepository.GetSetting(type, code).Value;

         string sender = "noreply@marketminerllc.com";     
         string subject = "Algorithm Status Notifications";

         string body = "";
         _notifications.Distinct().ToList().ForEach(notification => body += string.Format("\n{0}", notification));

         using (IMailer mailer = _MailerFactory.GetMailer(name))
         {
            // refactor this later to send emails to all admins
            string recipients = "mrchrisok@hotmail.com";

            mailer.SendMail(sender, recipients, subject, body);
         }

         _notifications.Clear();
      }

      public bool CanStrategyAcceptFunds(int strategyId, decimal amount)
      {
         bool canAcceptFunds = false;

         IStrategyRepository strategyRepository = _DataRepositoryFactory.GetDataRepository<IStrategyRepository>();

         Strategy strategy = strategyRepository.GetStrategy(strategyId);

         canAcceptFunds = amount <= (strategy.MaximumAUM - strategy.CurrentAUM);

         return canAcceptFunds;
      }

      public bool IsStrategyAlgorithmRunning(int strategyId)
      {
         bool running = false;

         IAlgorithmInstanceRepository algoInstanceRepository = _DataRepositoryFactory.GetDataRepository<IAlgorithmInstanceRepository>();

         var instances = algoInstanceRepository.GetAlgorithmInstancesByStrategy(strategyId);
         var runningInstance = instances.Where(i => i.Status != (short)AlgorithmStatus.NotRunning).FirstOrDefault();
         running = runningInstance != null;

         return running;
      }
   }
}
