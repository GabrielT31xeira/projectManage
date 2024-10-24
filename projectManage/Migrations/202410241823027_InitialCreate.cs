namespace projectManage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        id_user = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false),
                        email = c.String(nullable: false),
                        senha = c.String(nullable: false),
                        profile = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id_user);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
        }
    }
}
