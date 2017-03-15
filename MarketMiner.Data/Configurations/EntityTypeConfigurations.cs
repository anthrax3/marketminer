using MarketMiner.Business.Entities;
using P.Core.Common.Meta;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MarketMiner.Data.Configurations
{
   public class AccountConfiguration : EntityTypeConfiguration<Account>
   {
      public AccountConfiguration()
      {
         Property(x => x.AccountID).HasColumnOrder(0);
         Property(x => x.LoginEmail).HasColumnOrder(1).IsRequired().HasMaxLength(100);
         Property(x => x.FirstName).HasColumnOrder(2).IsRequired().HasMaxLength(100);
         Property(x => x.LastName).HasColumnOrder(3).IsRequired().HasMaxLength(100);
         Property(x => x.Address).HasColumnOrder(4).IsRequired().HasMaxLength(100);
         Property(x => x.City).HasColumnOrder(5).IsRequired().HasMaxLength(100);
         Property(x => x.State).HasColumnOrder(6).IsRequired().HasMaxLength(2);
         Property(x => x.ZipCode).HasColumnOrder(7).IsRequired().HasMaxLength(10);
         Property(x => x.CreditCard).HasColumnOrder(8).IsRequired().HasMaxLength(16);
         Property(x => x.ExpDate).HasColumnOrder(9).IsRequired();
      }
   }

   public class AlgorithmInstanceConfiguration : EntityTypeConfiguration<AlgorithmInstance>
   {
      public AlgorithmInstanceConfiguration()
      {
         Property(x => x.AlgorithmInstanceID).HasColumnOrder(0);
         Property(x => x.StrategyID).HasColumnOrder(1);
         Property(x => x.Status).HasColumnOrder(2);
         Property(x => x.FirstTradeDateTime).HasColumnOrder(3);
         Property(x => x.LastTradeDateTime).HasColumnOrder(4);
         Property(x => x.RunStartDateTime).HasColumnOrder(5);
         Property(x => x.RunStopDateTime).HasColumnOrder(6);
      }
   }

   public class AlgorithmMessageConfiguration : EntityTypeConfiguration<AlgorithmMessage>
   {
      public AlgorithmMessageConfiguration()
      {
         Property(x => x.AlgorithmMessageID).HasColumnOrder(0);
         Property(x => x.AlgorithmInstanceID).HasColumnOrder(1);
         Property(x => x.Message).HasColumnOrder(2).IsRequired();
         Property(x => x.Severity).HasColumnOrder(3).IsRequired();
      }
   }

   public class AlgorithmParameterConfiguration : EntityTypeConfiguration<AlgorithmParameter>
   {
      public AlgorithmParameterConfiguration()
      {
         Property(x => x.AlgorithmParameterID).HasColumnOrder(0);
         Property(x => x.StrategyID).HasColumnOrder(1).IsRequired();
         Property(x => x.ParameterName).HasColumnOrder(2).IsRequired().HasMaxLength(100);
         Property(x => x.ParameterType).HasColumnOrder(3).IsRequired().HasMaxLength(50);
         Property(x => x.ParameterValue).HasColumnOrder(4).IsRequired().HasMaxLength(100);
      }
   }

   public class AllocationConfiguration : EntityTypeConfiguration<Allocation>
   {
      public AllocationConfiguration()
      {
         Property(x => x.AllocationID).HasColumnOrder(0);
         Property(x => x.ReservationID).HasColumnOrder(1);
         Property(x => x.FundID).HasColumnOrder(2);
         Property(x => x.Amount).HasColumnOrder(3);
         Property(x => x.Pushed).HasColumnOrder(4);
      }
   }

   public class BrokerConfiguration : EntityTypeConfiguration<Broker>
   {
      public BrokerConfiguration()
      {
         Property(x => x.BrokerID).HasColumnOrder(0);
         Property(x => x.Name).HasColumnOrder(1).IsRequired().HasMaxLength(100);
      }
   }

   public class CommunicationConfiguration : EntityTypeConfiguration<Communication>
   {
      public CommunicationConfiguration()
      {
         Property(x => x.CommunicationID).HasColumnOrder(0);
         Property(x => x.CommunicationTemplateID).HasColumnOrder(1).IsRequired();
         Property(x => x.AccountID).HasColumnOrder(2);
         Property(x => x.SendType).HasColumnOrder(3).IsRequired();
         Property(x => x.Postmark).HasColumnOrder(4);
      }
   }

   public class CommunicationTemplateConfiguration : EntityTypeConfiguration<CommunicationTemplate>
   {
      public CommunicationTemplateConfiguration()
      {
         Property(x => x.CommunicationTemplateID).HasColumnOrder(0);
         Property(x => x.Name).HasColumnOrder(1).IsRequired().HasMaxLength(100);
         Property(x => x.Subject).HasColumnOrder(2).IsRequired().HasMaxLength(200);
         Property(x => x.Body).HasColumnOrder(3).IsRequired();
      }
   }

   public class FundConfiguration : EntityTypeConfiguration<Fund>
   {
      public FundConfiguration()
      {
         Property(x => x.FundID).HasColumnOrder(0);
         Property(x => x.Name).HasColumnOrder(1).IsRequired().HasMaxLength(100);
         Property(x => x.OpenDate).HasColumnOrder(2);
         Property(x => x.CloseDate).HasColumnOrder(3);
         Property(x => x.OpenToNew).HasColumnOrder(4);
         Property(x => x.OpenToAdd).HasColumnOrder(5);
         Property(x => x.OpenToRedeem).HasColumnOrder(6);
      }
   }

   public class MetaFieldDefinitionConfiguration : EntityTypeConfiguration<MetaFieldDefinition>
   {
      public MetaFieldDefinitionConfiguration()
      {
         Property(x => x.MetaFieldDefinitionID).HasColumnOrder(0);

         string uIndexTableNameFieldName = "IX_NU_TableNameFieldName";
         Property(x => x.TableName).HasColumnOrder(1).IsRequired().HasMaxLength(50)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute(uIndexTableNameFieldName, 1) { IsUnique = true }));
         Property(x => x.FieldName).HasColumnOrder(2).IsRequired().HasMaxLength(50)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute(uIndexTableNameFieldName, 2) { IsUnique = true }));

         Property(x => x.Caption).HasColumnOrder(3);
         Property(x => x.LookupTable).HasColumnOrder(4);
         Property(x => x.LookupDescriptionType).HasColumnOrder(5);
         Property(x => x.LookupDescriptionFormat).HasColumnOrder(6);
         Property(x => x.FormatType).HasColumnOrder(7);
         Property(x => x.MaximumValue).HasColumnOrder(8);
         Property(x => x.MaximumLength).HasColumnOrder(9);
         Property(x => x.RegularExpression).HasColumnOrder(10);
         Property(x => x.Required).HasColumnOrder(11);
         Property(x => x.ReadPermissionId).HasColumnOrder(12);
         Property(x => x.EditPermissionId).HasColumnOrder(13);
         Property(x => x.Enabled).HasColumnOrder(14);
      }
   }

   public class MetaLookupConfiguration : EntityTypeConfiguration<MetaLookup>
   {
      public MetaLookupConfiguration()
      {
         Property(x => x.MetaLookupID).HasColumnOrder(0);

         string indexTypeCode = "IX_NU_TypeCode";
         Property(x => x.Type).HasColumnOrder(1).IsRequired().HasMaxLength(50)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute(indexTypeCode, 1) { IsUnique = true }));
         Property(x => x.Code).HasColumnOrder(2).IsRequired().HasMaxLength(50)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute(indexTypeCode, 2) { IsUnique = true }));

         Property(x => x.ShortDesc).HasColumnOrder(3);
         Property(x => x.LongDesc).HasColumnOrder(4);
         Property(x => x.SortOrder).HasColumnOrder(5);
         Property(x => x.Enabled).HasColumnOrder(6);
      }
   }

   public class MetaResourceConfiguration : EntityTypeConfiguration<MetaResource>
   {
      public MetaResourceConfiguration()
      {
         Property(x => x.MetaResourceID).HasColumnOrder(0);

         string uIndexSetTypeKey = "IX_NU_SetTypeKey";
         Property(x => x.Set).HasColumnOrder(1).IsRequired().HasMaxLength(50)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute(uIndexSetTypeKey, 1) { IsUnique = true }));
         Property(x => x.Type).HasColumnOrder(2).IsRequired().HasMaxLength(50)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute(uIndexSetTypeKey, 2) { IsUnique = true }));
         Property(x => x.Key).HasColumnOrder(3).IsRequired().HasMaxLength(50)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute(uIndexSetTypeKey, 3) { IsUnique = true }));
   
         Property(x => x.Value).HasColumnOrder(4);
         Property(x => x.Category).HasColumnOrder(5);
         Property(x => x.CultureCode).HasColumnOrder(6).HasMaxLength(5);
         Property(x => x.Translated).HasColumnOrder(7);
      }
   }

   public class MetaSettingConfiguration : EntityTypeConfiguration<MetaSetting>
   {
      public MetaSettingConfiguration()
      {
         Property(x => x.MetaSettingID).HasColumnOrder(0);

         string uIndexEnvironmentTypeCode = "IX_NU_EnvironmentTypeCode";
         Property(x => x.Environment).IsRequired().HasMaxLength(50).HasColumnOrder(1)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute(uIndexEnvironmentTypeCode, 1) { IsUnique = true }));
         Property(x => x.Type).IsRequired().HasMaxLength(50).HasColumnOrder(2)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute(uIndexEnvironmentTypeCode, 2) { IsUnique = true }));
         Property(x => x.Code).IsRequired().HasMaxLength(50).HasColumnOrder(3)
            .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute(uIndexEnvironmentTypeCode, 3) { IsUnique = true }));

         Property(x => x.Value).HasColumnOrder(4);
         Property(x => x.SortOrder).HasColumnOrder(5);
         Property(x => x.Enabled).HasColumnOrder(6);
      }
   }

   public class ParticipationConfiguration : EntityTypeConfiguration<Participation>
   {
      public ParticipationConfiguration()
      {
         Property(x => x.ParticipationID).HasColumnOrder(0);
         Property(x => x.AccountID).HasColumnOrder(1);
         Property(x => x.FundID).HasColumnOrder(2);
         Property(x => x.InitialBalance).HasColumnOrder(3);
         Property(x => x.CurrentBalance).HasColumnOrder(4);
      }
   }

   public class ProductConfiguration : EntityTypeConfiguration<Product>
   {
      public ProductConfiguration()
      {
         Property(x => x.ProductID).HasColumnOrder(0);
         Property(x => x.Name).HasColumnOrder(1).IsRequired().HasMaxLength(100);
         Property(x => x.Code).HasColumnOrder(2).HasMaxLength(50);
         Property(x => x.ShortDesc).HasColumnOrder(3).IsRequired().HasMaxLength(200);
         Property(x => x.LongDesc).HasColumnOrder(4).HasMaxLength(400);
         Property(x => x.Active).HasColumnOrder(5);
      }
   }

   public class ReservationConfiguration : EntityTypeConfiguration<Reservation>
   {
      public ReservationConfiguration()
      {
         Property(x => x.ReservationID).HasColumnOrder(0);
         Property(x => x.AccountID).HasColumnOrder(1);
         Property(x => x.Amount).HasColumnOrder(2);
         Property(x => x.Open).HasColumnOrder(3);
         Property(x => x.Cancelled).HasColumnOrder(4);
      }
   }

   public class SignalConfiguration : EntityTypeConfiguration<Signal>
   {
      public SignalConfiguration()
      {
         Property(x => x.SignalID).HasColumnOrder(0);
         Property(x => x.ProductID).HasColumnOrder(1);
         Property(x => x.StrategyTransactionID).HasColumnOrder(2);
         Property(x => x.Type).HasColumnOrder(3).IsRequired().HasMaxLength(100);
         Property(x => x.Instrument).HasColumnOrder(4).HasMaxLength(100);
         Property(x => x.Granularity).HasColumnOrder(5).HasMaxLength(3);
         Property(x => x.Side).HasColumnOrder(6);
         Property(x => x.SignalPrice).HasColumnOrder(7);
         Property(x => x.SignalTime).HasColumnOrder(8).HasMaxLength(50);
         Property(x => x.SendPostmark).HasColumnOrder(9);
         Property(x => x.Active).HasColumnOrder(10);
      }
   }

   public class StrategyConfiguration : EntityTypeConfiguration<Strategy>
   {
      public StrategyConfiguration()
      {
         Property(x => x.StrategyID).HasColumnOrder(0);
         Property(x => x.FundID).HasColumnOrder(1);
         Property(x => x.Name).HasColumnOrder(2).IsRequired().HasMaxLength(100);
         Property(x => x.ShortDesc).HasColumnOrder(3).IsRequired().HasMaxLength(200);
         Property(x => x.LongDesc).HasColumnOrder(4).HasMaxLength(400);
         Property(x => x.InitialAUM).HasColumnOrder(5);
         Property(x => x.CurrentAUM).HasColumnOrder(6);
         Property(x => x.MaximumAUM).HasColumnOrder(7);
         Property(x => x.AlgorithmClass).HasColumnOrder(8);
         Property(x => x.AlgorithmAssembly).HasColumnOrder(9);
      }
   }

   public class StrategyTransactionConfiguration : EntityTypeConfiguration<StrategyTransaction>
   {
      public StrategyTransactionConfiguration()
      {
         Property(x => x.StrategyTransactionID).HasColumnOrder(0);
         Property(x => x.StrategyID).HasColumnOrder(1);
         Property(x => x.BrokerID).HasColumnOrder(2);
         Property(x => x.AccountID).HasColumnOrder(3).IsRequired().HasMaxLength(50);
         Property(x => x.BrokerTransactionID).HasColumnOrder(4).IsRequired().HasMaxLength(100);
         Property(x => x.BrokerOrderID).HasColumnOrder(5);
         Property(x => x.BrokerTradeID).HasColumnOrder(6);
         Property(x => x.Instrument).HasColumnOrder(7).HasMaxLength(100);
         Property(x => x.Type).HasColumnOrder(8).IsRequired().HasMaxLength(50);
         Property(x => x.Time).HasColumnOrder(9).IsRequired();
         Property(x => x.Side).HasColumnOrder(10).HasMaxLength(1);
         Property(x => x.Price).HasColumnOrder(11);
         Property(x => x.TakeProfit).HasColumnOrder(12);
         Property(x => x.StopLoss).HasColumnOrder(13);
      }
   }

   public class SubscriptionConfiguration : EntityTypeConfiguration<Subscription>
   {
      public SubscriptionConfiguration()
      {
         Property(x => x.SubscriptionID).HasColumnOrder(0);
         Property(x => x.AccountID).HasColumnOrder(1);
         Property(x => x.ProductID).HasColumnOrder(2);
         Property(x => x.ProductID).HasColumnOrder(3);
      }
   }
}
