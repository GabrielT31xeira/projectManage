using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Texto:")]
        [Required(ErrorMessage = "O texto do comentário é obrigatório.")]
        [StringLength(500, ErrorMessage = "O texto do comentário deve ter no máximo 500 caracteres.")]
        public string Texto { get; set; }

        [DisplayName("Data:")]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; }

        // Chave estrangeira para Tarefa
        public int TarefaId { get; set; }
        public virtual Tarefa Tarefa { get; set; }
    }
}