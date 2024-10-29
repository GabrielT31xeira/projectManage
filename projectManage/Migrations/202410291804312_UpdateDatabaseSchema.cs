namespace projectManage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabaseSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Arquivoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Caminho = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tarefas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Descricao = c.String(),
                        DataInicio = c.DateTime(nullable: false),
                        DataFim = c.DateTime(nullable: false),
                        Status = c.String(),
                        ProjetoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projetoes", t => t.ProjetoId, cascadeDelete: true)
                .Index(t => t.ProjetoId);
            
            CreateTable(
                "dbo.Comentarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Texto = c.String(),
                        Data = c.DateTime(nullable: false),
                        TarefaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tarefas", t => t.TarefaId, cascadeDelete: true)
                .Index(t => t.TarefaId);
            
            CreateTable(
                "dbo.Projetoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Descricao = c.String(nullable: false, maxLength: 500),
                        DataInicio = c.DateTime(nullable: false),
                        DataFim = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        id_user = c.Int(nullable: false, identity: true),
                        email = c.String(nullable: false),
                        senha = c.String(nullable: false),
                        nome = c.String(nullable: false),
                        profile = c.String(nullable: false),
                        ConfirmationToken = c.String(),
                        IsEmailConfirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_user);
            
            CreateTable(
                "dbo.Equipes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TarefaArquivos",
                c => new
                    {
                        TarefaId = c.Int(nullable: false),
                        ArquivoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TarefaId, t.ArquivoId })
                .ForeignKey("dbo.Tarefas", t => t.TarefaId, cascadeDelete: true)
                .ForeignKey("dbo.Arquivoes", t => t.ArquivoId, cascadeDelete: true)
                .Index(t => t.TarefaId)
                .Index(t => t.ArquivoId);
            
            CreateTable(
                "dbo.EquipeUsuarios",
                c => new
                    {
                        EquipeId = c.Int(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EquipeId, t.UsuarioId })
                .ForeignKey("dbo.Equipes", t => t.EquipeId, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.EquipeId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.ProjetoUsuarios",
                c => new
                    {
                        ProjetoId = c.Int(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjetoId, t.UsuarioId })
                .ForeignKey("dbo.Projetoes", t => t.ProjetoId, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.ProjetoId)
                .Index(t => t.UsuarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjetoUsuarios", "UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.ProjetoUsuarios", "ProjetoId", "dbo.Projetoes");
            DropForeignKey("dbo.EquipeUsuarios", "UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.EquipeUsuarios", "EquipeId", "dbo.Equipes");
            DropForeignKey("dbo.Tarefas", "ProjetoId", "dbo.Projetoes");
            DropForeignKey("dbo.Comentarios", "TarefaId", "dbo.Tarefas");
            DropForeignKey("dbo.TarefaArquivos", "ArquivoId", "dbo.Arquivoes");
            DropForeignKey("dbo.TarefaArquivos", "TarefaId", "dbo.Tarefas");
            DropIndex("dbo.ProjetoUsuarios", new[] { "UsuarioId" });
            DropIndex("dbo.ProjetoUsuarios", new[] { "ProjetoId" });
            DropIndex("dbo.EquipeUsuarios", new[] { "UsuarioId" });
            DropIndex("dbo.EquipeUsuarios", new[] { "EquipeId" });
            DropIndex("dbo.TarefaArquivos", new[] { "ArquivoId" });
            DropIndex("dbo.TarefaArquivos", new[] { "TarefaId" });
            DropIndex("dbo.Comentarios", new[] { "TarefaId" });
            DropIndex("dbo.Tarefas", new[] { "ProjetoId" });
            DropTable("dbo.ProjetoUsuarios");
            DropTable("dbo.EquipeUsuarios");
            DropTable("dbo.TarefaArquivos");
            DropTable("dbo.Equipes");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Projetoes");
            DropTable("dbo.Comentarios");
            DropTable("dbo.Tarefas");
            DropTable("dbo.Arquivoes");
        }
    }
}
