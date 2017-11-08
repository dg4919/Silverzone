namespace SilverzoneERP.Context.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class dfgkd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schedule_Olympiads",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ImageName = c.String(),
                        Caption = c.String(),
                        Link = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Schedule_Olympiads");
        }
    }
}
