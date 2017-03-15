using EFCache;
using MarketMiner.Business.Entities;
using MarketMiner.Data.Configurations;
using N.Core.Common.Data;
using P.Core.Common.Contracts;
using P.Core.Common.Meta;
using System.Data.Entity;
using System.Linq;

namespace MarketMiner.Data
{
   public class MarketMinerContext : DbContextBase
   {
      #region Constructors
      public MarketMinerContext()
         : base("name=DefaultConnection", typeof(AccountConfiguration))
      {
         Configuration.LazyLoadingEnabled = true;
         Configuration.ProxyCreationEnabled = true;
      }
      #endregion

      #region Sets
      public DbSet<Account> AccountSet { get; set; }
      public DbSet<AlgorithmInstance> AlgorithmInstanceSet { get; set; }
      public DbSet<AlgorithmMessage> AlgorithmMessageSet { get; set; }
      public DbSet<AlgorithmParameter> AlgorithmParameterSet { get; set; }
      public DbSet<Allocation> AllocationSet { get; set; }
      public DbSet<Broker> BrokerSet { get; set; }
      public DbSet<Communication> CommunicationSet { get; set; }
      public DbSet<CommunicationTemplate> CommunicationTemplateSet { get; set; }
      public DbSet<Fund> FundSet { get; set; }
      public DbSet<MetaFieldDefinition> MetaFieldDefinitionSet { get; set; }
      public DbSet<MetaLookup> MetaLookupSet { get; set; }
      public DbSet<MetaResource> MetaResourceSet { get; set; }
      public DbSet<MetaSetting> MetaSettingSet { get; set; }
      public DbSet<Participation> ParticipationSet { get; set; }
      public DbSet<Product> ProductSet { get; set; }
      public DbSet<Reservation> ReservationSet { get; set; }
      public DbSet<Signal> SignalSet { get; set; }
      public DbSet<Subscription> SubscriptionSet { get; set; }
      public DbSet<Strategy> StrategySet { get; set; }
      public DbSet<StrategyTransaction> StrategyTransactionSet { get; set; }
      #endregion

      #region Methods.Override
      public override int SaveChanges()
      {
         /// first, remove orphaned entities
         var orphanedObjects = ChangeTracker.Entries().Where(
            e => (e.State == EntityState.Modified || e.State == EntityState.Added) &&
               e.Entity.GetType().GetInterfaces().Any(x => x.GetType() == typeof(IAccountOwnedEntity) &&
               e.Reference("AccountID").CurrentValue == null) &&
               //excludeds
               e.Entity.GetType() != typeof(Redemption));

         foreach (var orphanedObject in orphanedObjects)
         {
            orphanedObject.State = EntityState.Deleted;
         }

         /// second, call base to do the rest
         return base.SaveChanges();
      }
      #endregion
   }
}
