namespace MarketMiner.Data.Migrations
{
   using System;
   using System.Data.Entity.Migrations;

   public partial class UpdateSignal : DbMigration
   {
      public override void Up()
      {
         DropForeignKey("dbo.Signal", "AlgorithmInstanceID", "dbo.AlgorithmInstance");
         DropIndex("dbo.Signal", new[] { "AlgorithmInstanceID" });
         DropColumn("dbo.Signal", "AlgorithmInstanceID");
      }

      public override void Down()
      {
         AddColumn("dbo.Signal", "AlgorithmInstanceID", c => c.Int(nullable: false));
         CreateIndex("dbo.Signal", "AlgorithmInstanceID");
         AddForeignKey("dbo.Signal", "AlgorithmInstanceID", "dbo.AlgorithmInstance", "AlgorithmInstanceID", cascadeDelete: true);
      }
   }
}
