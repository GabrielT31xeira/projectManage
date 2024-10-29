using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace projectManage.Models
{
    public class Comentario
    {
        [Key]
        public int Id { get; set; }
        public string Texto { get; set; }
        public DateTime Data { get; set; }

        // Foreign key para Tarefa
        [ForeignKey("Tarefa")]
        public int TarefaId { get; set; }
        public virtual Tarefa Tarefa { get; set; }
    }
}