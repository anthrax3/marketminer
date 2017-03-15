namespace MarketMiner.Data.Migrations
{
   using System;
   using System.Data.Entity.Migrations;

   public partial class InitialCreate : DbMigration
   {
      public override void Up()
      {
         CreateTable(
             "dbo.Account",
             c => new
                 {
                    AccountID = c.Int(nullable: false, identity: true),
                    LoginEmail = c.String(nullable: false, maxLength: 100),
                    FirstName = c.String(nullable: false, maxLength: 100),
                    LastName = c.String(nullable: false, maxLength: 100),
                    Address = c.String(nullable: false, maxLength: 100),
                    City = c.String(nullable: false, maxLength: 100),
                    State = c.String(nullable: false, maxLength: 2),
                    ZipCode = c.String(nullable: false, maxLength: 10),
                    CreditCard = c.String(nullable: false, maxLength: 16),
                    ExpDate = c.DateTime(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.AccountID);

         CreateTable(
             "dbo.Participation",
             c => new
                 {
                    ParticipationID = c.Int(nullable: false, identity: true),
                    AccountID = c.Int(nullable: false),
                    FundID = c.Int(nullable: false),
                    InitialBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CurrentBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.ParticipationID)
             .ForeignKey("dbo.Account", t => t.AccountID, cascadeDelete: true)
             .ForeignKey("dbo.Fund", t => t.FundID, cascadeDelete: true)
             .Index(t => t.AccountID)
             .Index(t => t.FundID);

         CreateTable(
             "dbo.Fund",
             c => new
                 {
                    FundID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    OpenDate = c.DateTime(),
                    CloseDate = c.DateTime(),
                    OpenToNew = c.Boolean(nullable: false),
                    OpenToAdd = c.Boolean(nullable: false),
                    OpenToRedeem = c.Boolean(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.FundID);

         CreateTable(
             "dbo.Allocation",
             c => new
                 {
                    AllocationID = c.Int(nullable: false, identity: true),
                    ReservationID = c.Int(nullable: false),
                    FundID = c.Int(nullable: false),
                    Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Pushed = c.Boolean(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.AllocationID)
             .ForeignKey("dbo.Fund", t => t.FundID, cascadeDelete: true)
             .ForeignKey("dbo.Reservation", t => t.ReservationID, cascadeDelete: true)
             .Index(t => t.ReservationID)
             .Index(t => t.FundID);

         CreateTable(
             "dbo.Reservation",
             c => new
                 {
                    ReservationID = c.Int(nullable: false, identity: true),
                    AccountID = c.Int(nullable: false),
                    Amount = c.Double(nullable: false),
                    Open = c.Boolean(nullable: false),
                    Cancelled = c.Boolean(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.ReservationID)
             .ForeignKey("dbo.Account", t => t.AccountID, cascadeDelete: true)
             .Index(t => t.AccountID);

         CreateTable(
             "dbo.Strategy",
             c => new
                 {
                    StrategyID = c.Int(nullable: false, identity: true),
                    FundID = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 100),
                    ShortDesc = c.String(nullable: false, maxLength: 200),
                    LongDesc = c.String(maxLength: 400),
                    InitialAUM = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CurrentAUM = c.Decimal(nullable: false, precision: 18, scale: 2),
                    MaximumAUM = c.Decimal(nullable: false, precision: 18, scale: 2),
                    AlgorithmClass = c.String(),
                    AlgorithmAssembly = c.String(),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.StrategyID)
             .ForeignKey("dbo.Fund", t => t.FundID, cascadeDelete: true)
             .Index(t => t.FundID);

         CreateTable(
             "dbo.AlgorithmInstance",
             c => new
                 {
                    AlgorithmInstanceID = c.Int(nullable: false, identity: true),
                    StrategyID = c.Int(nullable: false),
                    Status = c.Short(),
                    FirstTradeDateTime = c.DateTime(),
                    LastTradeDateTime = c.DateTime(),
                    RunStartDateTime = c.DateTime(),
                    RunStopDateTime = c.DateTime(),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.AlgorithmInstanceID)
             .ForeignKey("dbo.Strategy", t => t.StrategyID, cascadeDelete: true)
             .Index(t => t.StrategyID);

         CreateTable(
             "dbo.AlgorithmMessage",
             c => new
                 {
                    AlgorithmMessageID = c.Int(nullable: false, identity: true),
                    AlgorithmInstanceID = c.Int(nullable: false),
                    Message = c.String(nullable: false),
                    Severity = c.Int(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.AlgorithmMessageID)
             .ForeignKey("dbo.AlgorithmInstance", t => t.AlgorithmInstanceID, cascadeDelete: true)
             .Index(t => t.AlgorithmInstanceID);

         CreateTable(
             "dbo.AlgorithmParameter",
             c => new
                 {
                    AlgorithmParameterID = c.Int(nullable: false, identity: true),
                    StrategyID = c.Int(nullable: false),
                    ParameterName = c.String(nullable: false, maxLength: 100),
                    ParameterType = c.String(nullable: false, maxLength: 50),
                    ParameterValue = c.String(nullable: false, maxLength: 100),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.AlgorithmParameterID)
             .ForeignKey("dbo.Strategy", t => t.StrategyID, cascadeDelete: true)
             .Index(t => t.StrategyID);

         CreateTable(
             "dbo.StrategyTransaction",
             c => new
                 {
                    StrategyTransactionID = c.Int(nullable: false, identity: true),
                    StrategyID = c.Int(nullable: false),
                    BrokerID = c.Int(nullable: false),
                    AccountID = c.String(nullable: false, maxLength: 50),
                    BrokerTransactionID = c.String(nullable: false, maxLength: 100),
                    BrokerOrderID = c.String(),
                    BrokerTradeID = c.String(),
                    Instrument = c.String(maxLength: 100),
                    Type = c.String(nullable: false, maxLength: 50),
                    Time = c.DateTime(nullable: false),
                    Side = c.String(maxLength: 1),
                    Price = c.Double(nullable: false),
                    TakeProfit = c.Double(),
                    StopLoss = c.Double(),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.StrategyTransactionID)
             .ForeignKey("dbo.Broker", t => t.BrokerID, cascadeDelete: true)
             .ForeignKey("dbo.Strategy", t => t.StrategyID, cascadeDelete: true)
             .Index(t => t.StrategyID)
             .Index(t => t.BrokerID);

         CreateTable(
             "dbo.Broker",
             c => new
                 {
                    BrokerID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.BrokerID);

         CreateTable(
             "dbo.Redemption",
             c => new
                 {
                    RedemptionID = c.Int(nullable: false, identity: true),
                    Amount = c.Int(nullable: false),
                    DatePaid = c.DateTime(),
                    AccountID = c.Int(),
                    ParticipationID = c.Int(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.RedemptionID)
             .ForeignKey("dbo.Account", t => t.AccountID)
             .ForeignKey("dbo.Participation", t => t.ParticipationID, cascadeDelete: true)
             .Index(t => t.AccountID)
             .Index(t => t.ParticipationID);

         CreateTable(
             "dbo.Subscription",
             c => new
                 {
                    SubscriptionID = c.Int(nullable: false, identity: true),
                    AccountID = c.Int(nullable: false),
                    ProductID = c.Int(nullable: false),
                    Active = c.Boolean(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.SubscriptionID)
             .ForeignKey("dbo.Account", t => t.AccountID, cascadeDelete: true)
             .ForeignKey("dbo.Product", t => t.ProductID, cascadeDelete: true)
             .Index(t => t.AccountID)
             .Index(t => t.ProductID);

         CreateTable(
             "dbo.Product",
             c => new
                 {
                    ProductID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    Code = c.String(maxLength: 50),
                    ShortDesc = c.String(nullable: false, maxLength: 200),
                    LongDesc = c.String(maxLength: 400),
                    Active = c.Boolean(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.ProductID);

         CreateTable(
             "dbo.Communication",
             c => new
                 {
                    CommunicationID = c.Int(nullable: false, identity: true),
                    CommunicationTemplateID = c.Int(nullable: false),
                    AccountID = c.Int(nullable: false),
                    SendType = c.Short(nullable: false),
                    Postmark = c.DateTime(),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.CommunicationID)
             .ForeignKey("dbo.Account", t => t.AccountID, cascadeDelete: true)
             .ForeignKey("dbo.CommunicationTemplate", t => t.CommunicationTemplateID, cascadeDelete: true)
             .Index(t => t.CommunicationTemplateID)
             .Index(t => t.AccountID);

         CreateTable(
             "dbo.CommunicationTemplate",
             c => new
                 {
                    CommunicationTemplateID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    Subject = c.String(nullable: false, maxLength: 200),
                    Body = c.String(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.CommunicationTemplateID);

         CreateTable(
             "dbo.MetaFieldDefinition",
             c => new
                 {
                    MetaFieldDefinitionID = c.Int(nullable: false, identity: true),
                    TableName = c.String(nullable: false, maxLength: 50),
                    FieldName = c.String(nullable: false, maxLength: 50),
                    Caption = c.String(),
                    LookupTable = c.String(),
                    LookupDescriptionType = c.String(),
                    LookupDescriptionFormat = c.String(),
                    FormatType = c.String(),
                    MaximumValue = c.String(),
                    MaximumLength = c.String(),
                    RegularExpression = c.String(),
                    Required = c.Boolean(nullable: false),
                    ReadPermissionId = c.String(),
                    EditPermissionId = c.String(),
                    Enabled = c.Boolean(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.MetaFieldDefinitionID)
             .Index(t => new { t.TableName, t.FieldName }, unique: true, name: "IX_NU_TableNameFieldName");

         CreateTable(
             "dbo.MetaLookup",
             c => new
                 {
                    MetaLookupID = c.Int(nullable: false, identity: true),
                    Type = c.String(nullable: false, maxLength: 50),
                    Code = c.String(nullable: false, maxLength: 50),
                    ShortDesc = c.String(),
                    LongDesc = c.String(),
                    SortOrder = c.Int(nullable: false),
                    Enabled = c.Boolean(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.MetaLookupID)
             .Index(t => new { t.Type, t.Code }, unique: true, name: "IX_NU_TypeCode");

         CreateTable(
             "dbo.MetaResource",
             c => new
                 {
                    MetaResourceID = c.Int(nullable: false, identity: true),
                    Set = c.String(nullable: false, maxLength: 50),
                    Type = c.String(nullable: false, maxLength: 50),
                    Key = c.String(nullable: false, maxLength: 50),
                    Value = c.String(),
                    Category = c.String(),
                    CultureCode = c.String(maxLength: 5),
                    Translated = c.Boolean(nullable: false),
                    Enabled = c.Boolean(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.MetaResourceID)
             .Index(t => new { t.Set, t.Type, t.Key }, unique: true, name: "IX_NU_SetTypeKey");

         CreateTable(
             "dbo.MetaSetting",
             c => new
                 {
                    MetaSettingID = c.Int(nullable: false, identity: true),
                    Environment = c.String(nullable: false, maxLength: 50),
                    Type = c.String(nullable: false, maxLength: 50),
                    Code = c.String(nullable: false, maxLength: 50),
                    Value = c.String(),
                    SortOrder = c.Boolean(nullable: false),
                    Enabled = c.Boolean(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.MetaSettingID)
             .Index(t => new { t.Environment, t.Type, t.Code }, unique: true, name: "IX_NU_EnvironmentTypeCode");

         CreateTable(
             "dbo.Signal",
             c => new
                 {
                    SignalID = c.Int(nullable: false, identity: true),
                    ProductID = c.Int(nullable: false),
                    AlgorithmInstanceID = c.Int(nullable: false),
                    StrategyTransactionID = c.Int(),
                    Type = c.String(nullable: false, maxLength: 100),
                    Instrument = c.String(maxLength: 100),
                    Granularity = c.String(maxLength: 3),
                    Side = c.String(),
                    SignalPrice = c.Double(nullable: false),
                    SignalTime = c.String(maxLength: 50),
                    SendPostmark = c.DateTime(),
                    Active = c.Boolean(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateModified = c.DateTime(nullable: false),
                 })
             .PrimaryKey(t => t.SignalID)
             .ForeignKey("dbo.AlgorithmInstance", t => t.AlgorithmInstanceID, cascadeDelete: true)
             .ForeignKey("dbo.Product", t => t.ProductID, cascadeDelete: true)
             .ForeignKey("dbo.StrategyTransaction", t => t.StrategyTransactionID)
             .Index(t => t.ProductID)
             .Index(t => t.AlgorithmInstanceID)
             .Index(t => t.StrategyTransactionID);

      }

      public override void Down()
      {
         DropForeignKey("dbo.Signal", "StrategyTransactionID", "dbo.StrategyTransaction");
         DropForeignKey("dbo.Signal", "ProductID", "dbo.Product");
         DropForeignKey("dbo.Signal", "AlgorithmInstanceID", "dbo.AlgorithmInstance");
         DropForeignKey("dbo.Communication", "CommunicationTemplateID", "dbo.CommunicationTemplate");
         DropForeignKey("dbo.Communication", "AccountID", "dbo.Account");
         DropForeignKey("dbo.Subscription", "ProductID", "dbo.Product");
         DropForeignKey("dbo.Subscription", "AccountID", "dbo.Account");
         DropForeignKey("dbo.Redemption", "ParticipationID", "dbo.Participation");
         DropForeignKey("dbo.Redemption", "AccountID", "dbo.Account");
         DropForeignKey("dbo.StrategyTransaction", "StrategyID", "dbo.Strategy");
         DropForeignKey("dbo.StrategyTransaction", "BrokerID", "dbo.Broker");
         DropForeignKey("dbo.Strategy", "FundID", "dbo.Fund");
         DropForeignKey("dbo.AlgorithmParameter", "StrategyID", "dbo.Strategy");
         DropForeignKey("dbo.AlgorithmInstance", "StrategyID", "dbo.Strategy");
         DropForeignKey("dbo.AlgorithmMessage", "AlgorithmInstanceID", "dbo.AlgorithmInstance");
         DropForeignKey("dbo.Participation", "FundID", "dbo.Fund");
         DropForeignKey("dbo.Allocation", "ReservationID", "dbo.Reservation");
         DropForeignKey("dbo.Reservation", "AccountID", "dbo.Account");
         DropForeignKey("dbo.Allocation", "FundID", "dbo.Fund");
         DropForeignKey("dbo.Participation", "AccountID", "dbo.Account");
         DropIndex("dbo.Signal", new[] { "StrategyTransactionID" });
         DropIndex("dbo.Signal", new[] { "AlgorithmInstanceID" });
         DropIndex("dbo.Signal", new[] { "ProductID" });
         DropIndex("dbo.MetaSetting", "IX_NU_EnvironmentTypeCode");
         DropIndex("dbo.MetaResource", "IX_NU_SetTypeKey");
         DropIndex("dbo.MetaLookup", "IX_NU_TypeCode");
         DropIndex("dbo.MetaFieldDefinition", "IX_NU_TableNameFieldName");
         DropIndex("dbo.Communication", new[] { "AccountID" });
         DropIndex("dbo.Communication", new[] { "CommunicationTemplateID" });
         DropIndex("dbo.Subscription", new[] { "ProductID" });
         DropIndex("dbo.Subscription", new[] { "AccountID" });
         DropIndex("dbo.Redemption", new[] { "ParticipationID" });
         DropIndex("dbo.Redemption", new[] { "AccountID" });
         DropIndex("dbo.StrategyTransaction", new[] { "BrokerID" });
         DropIndex("dbo.StrategyTransaction", new[] { "StrategyID" });
         DropIndex("dbo.AlgorithmParameter", new[] { "StrategyID" });
         DropIndex("dbo.AlgorithmMessage", new[] { "AlgorithmInstanceID" });
         DropIndex("dbo.AlgorithmInstance", new[] { "StrategyID" });
         DropIndex("dbo.Strategy", new[] { "FundID" });
         DropIndex("dbo.Reservation", new[] { "AccountID" });
         DropIndex("dbo.Allocation", new[] { "FundID" });
         DropIndex("dbo.Allocation", new[] { "ReservationID" });
         DropIndex("dbo.Participation", new[] { "FundID" });
         DropIndex("dbo.Participation", new[] { "AccountID" });
         DropTable("dbo.Signal");
         DropTable("dbo.MetaSetting");
         DropTable("dbo.MetaResource");
         DropTable("dbo.MetaLookup");
         DropTable("dbo.MetaFieldDefinition");
         DropTable("dbo.CommunicationTemplate");
         DropTable("dbo.Communication");
         DropTable("dbo.Product");
         DropTable("dbo.Subscription");
         DropTable("dbo.Redemption");
         DropTable("dbo.Broker");
         DropTable("dbo.StrategyTransaction");
         DropTable("dbo.AlgorithmParameter");
         DropTable("dbo.AlgorithmMessage");
         DropTable("dbo.AlgorithmInstance");
         DropTable("dbo.Strategy");
         DropTable("dbo.Reservation");
         DropTable("dbo.Allocation");
         DropTable("dbo.Fund");
         DropTable("dbo.Participation");
         DropTable("dbo.Account");
      }
   }
}
