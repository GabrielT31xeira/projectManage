namespace projectManage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDonoToProjeto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projetoes", "DonoId", c => c.Int(nullable: false));
            CreateIndex("dbo.Projetoes", "DonoId");
            AddForeignKey("dbo.Projetoes", "DonoId", "dbo.Usuarios", "id_user", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projetoes", "DonoId", "dbo.Usuarios");
            DropIndex("dbo.Projetoes", new[] { "DonoId" });
            DropColumn("dbo.Projetoes", "DonoId");
        }
    }
}
