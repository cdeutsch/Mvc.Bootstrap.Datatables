namespace Mvc.Bootstrap.Datatables.Sample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCars : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Make = c.String(nullable: false, maxLength: 100),
                        Model = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cars");
        }
    }
}
