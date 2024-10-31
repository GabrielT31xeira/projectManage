using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace projectManage.Models
{
    public class Tarefa
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da tarefa é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da tarefa deve ter no máximo 100 caracteres.")]
        [DisplayName("Nome:")]
        public string Nome { get; set; }

        [DisplayName("Descrição:")]
        [StringLength(500, ErrorMessage = "A descrição da tarefa deve ter no máximo 500 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [DataType(DataType.Date, ErrorMessage = "Data de início inválida.")]
        [DisplayName("Data de Início:")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de término é obrigatória.")]
        [DataType(DataType.Date, ErrorMessage = "Data de término inválida.")]
        [DisplayName("Data de Término:")]
        [CustomValidation(typeof(Tarefa), "ValidarDataFim")]
        public DateTime DataFim { get; set; }

        [Required(ErrorMessage = "O status da tarefa é obrigatório.")]
        [DisplayName("Status:")]
        public string Status { get; set; }

        // Relação 1:N com Comentario
        public virtual ICollection<Comentario> Comentarios { get; set; }

        // Relação N:N com Arquivo
        public virtual ICollection<Arquivo> Arquivos { get; set; }

        // Chave estrangeira para Projeto
        public int ProjetoId { get; set; }
        public virtual Projeto Projeto { get; set; }

        // Método de validação para DataFim
        public static ValidationResult ValidarDataFim(DateTime dataFim, ValidationContext context)
        {
            var instance = context.ObjectInstance as Tarefa;
            if (instance != null && dataFim < instance.DataInicio)
            {
                return new ValidationResult("A data de término não pode ser anterior à data de início.");
            }
            return ValidationResult.Success;
        }
    }
}
