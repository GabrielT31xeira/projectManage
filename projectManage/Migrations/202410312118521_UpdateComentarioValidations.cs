namespace projectManage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateComentarioValidations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comentarios", "Texto", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comentarios", "Texto", c => c.String());
        }
    }
}
