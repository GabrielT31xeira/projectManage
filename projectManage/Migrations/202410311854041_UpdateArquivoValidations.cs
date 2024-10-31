namespace projectManage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateArquivoValidations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Arquivoes", "Caminho", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Arquivoes", "Caminho", c => c.String());
        }
    }
}
