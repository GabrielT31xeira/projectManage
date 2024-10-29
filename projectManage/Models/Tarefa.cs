using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projectManage.Models
{
    public class Tarefa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }

        public string Status { get; set; }

        // Relação 1:N com Comentario
        public virtual ICollection<Comentario> Comentarios { get; set; }

        // Relação N:N com Arquivo
        public virtual ICollection<Arquivo> Arquivos { get; set; }

        // Chave estrangeira para Projeto
        public int ProjetoId { get; set; }
        public virtual Projeto Projeto { get; set; }
    }
}