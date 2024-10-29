using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projectManage.Models
{
    public class Arquivo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public string Caminho { get; set; }

        // Relação N:N com Tarefa
        public virtual ICollection<Tarefa> Tarefas { get; set; }
    }
}