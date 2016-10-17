namespace Tazeyab.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setups : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.ProjectSetup");
            //AlterColumn("dbo.ProjectSetup", "Id", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.ProjectSetup", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ProjectSetup");
            AlterColumn("dbo.ProjectSetup", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ProjectSetup", "Title");
        }
    }
}
