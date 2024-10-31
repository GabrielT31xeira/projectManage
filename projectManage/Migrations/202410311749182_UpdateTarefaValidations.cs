namespace projectManage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTarefaValidations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tarefas", "Descricao", c => c.String(maxLength: 500));
            AlterColumn("dbo.Tarefas", "Status", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tarefas", "Status", c => c.String());
            AlterColumn("dbo.Tarefas", "Descricao", c => c.String());
        }
    }
}
