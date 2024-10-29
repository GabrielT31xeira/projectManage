namespace projectManage.Data
{ 
using projectManage.Models;
using System.Data.Entity;

public class AddDbContext : DbContext
{
    public AddDbContext() : base("AppDbContext") // Defina sua string de conexão aqui
    {
    }

    public DbSet<Projeto> Projetos { get; set; }
    public DbSet<Tarefa> Tarefas { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Equipe> Equipes { get; set; }
    public DbSet<Arquivo> Arquivos { get; set; }
    public DbSet<Comentario> Comentarios { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        // Configurar relacionamentos muitos para muitos

        modelBuilder.Entity<Projeto>()
            .HasMany(p => p.Usuarios)
            .WithMany(u => u.Projetos)
            .Map(m =>
            {
                m.ToTable("ProjetoUsuarios");
                m.MapLeftKey("ProjetoId");
                m.MapRightKey("UsuarioId");
            });

        modelBuilder.Entity<Equipe>()
            .HasMany(e => e.Usuarios)
            .WithMany(u => u.Equipes)
            .Map(m =>
            {
                m.ToTable("EquipeUsuarios");
                m.MapLeftKey("EquipeId");
                m.MapRightKey("UsuarioId");
            });

        modelBuilder.Entity<Tarefa>()
            .HasMany(t => t.Arquivos)
            .WithMany(a => a.Tarefas)
            .Map(m =>
            {
                m.ToTable("TarefaArquivos");
                m.MapLeftKey("TarefaId");
                m.MapRightKey("ArquivoId");
            });

        base.OnModelCreating(modelBuilder);
    }
}
}