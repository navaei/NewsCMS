namespace Tazeyab.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mm : DbMigration
    {
        public override void Up()
        {
            //RenameColumn(table: "dbo.TagsPages", name: "PageId", newName: "__mig_tmp__0");
            //RenameColumn(table: "dbo.TagsPages", name: "TagId", newName: "PageId");
            //RenameColumn(table: "dbo.TagsRemoteWebParts", name: "RemoteWebPartId", newName: "__mig_tmp__1");
            //RenameColumn(table: "dbo.TagsRemoteWebParts", name: "TagId", newName: "RemoteWebPartId");
            //RenameColumn(table: "dbo.TagsPages", name: "__mig_tmp__0", newName: "TagId");
            //RenameColumn(table: "dbo.TagsRemoteWebParts", name: "__mig_tmp__1", newName: "TagId");
            //RenameIndex(table: "dbo.TagsPages", name: "IX_PageId", newName: "__mig_tmp__0");
            //RenameIndex(table: "dbo.TagsPages", name: "IX_TagId", newName: "IX_PageId");
            //RenameIndex(table: "dbo.TagsRemoteWebParts", name: "IX_RemoteWebPartId", newName: "__mig_tmp__1");
            //RenameIndex(table: "dbo.TagsRemoteWebParts", name: "IX_TagId", newName: "IX_RemoteWebPartId");
            //RenameIndex(table: "dbo.TagsPages", name: "__mig_tmp__0", newName: "IX_TagId");
            //RenameIndex(table: "dbo.TagsRemoteWebParts", name: "__mig_tmp__1", newName: "IX_TagId");
        }
        
        public override void Down()
        {
            //RenameIndex(table: "dbo.TagsRemoteWebParts", name: "IX_TagId", newName: "__mig_tmp__1");
            //RenameIndex(table: "dbo.TagsPages", name: "IX_TagId", newName: "__mig_tmp__0");
            //RenameIndex(table: "dbo.TagsRemoteWebParts", name: "IX_RemoteWebPartId", newName: "IX_TagId");
            //RenameIndex(table: "dbo.TagsRemoteWebParts", name: "__mig_tmp__1", newName: "IX_RemoteWebPartId");
            //RenameIndex(table: "dbo.TagsPages", name: "IX_PageId", newName: "IX_TagId");
            //RenameIndex(table: "dbo.TagsPages", name: "__mig_tmp__0", newName: "IX_PageId");
            //RenameColumn(table: "dbo.TagsRemoteWebParts", name: "TagId", newName: "__mig_tmp__1");
            //RenameColumn(table: "dbo.TagsPages", name: "TagId", newName: "__mig_tmp__0");
            //RenameColumn(table: "dbo.TagsRemoteWebParts", name: "RemoteWebPartId", newName: "TagId");
            //RenameColumn(table: "dbo.TagsRemoteWebParts", name: "__mig_tmp__1", newName: "RemoteWebPartId");
            //RenameColumn(table: "dbo.TagsPages", name: "PageId", newName: "TagId");
            //RenameColumn(table: "dbo.TagsPages", name: "__mig_tmp__0", newName: "PageId");
        }
    }
}
