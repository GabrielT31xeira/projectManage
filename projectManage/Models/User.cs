using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;
 
namespace projectManage.Models
{
    public class User
    {
        [Key]
        [Column("id_user")]
        public int Id { get; set; }

        [DisplayName("Nome:")]
        [Required(ErrorMessage = "Digite o nome.")]
        [Column("nome")]
        public string nome { get; set; }

        [DisplayName("Email:")]
        [Required(ErrorMessage = "Digite o email.")]
        [Column("email")]
        public string email { get; set; }
        
        [DisplayName("Senha:")]
        [Required(ErrorMessage = "Digite a senha.")]
        [Column("senha")]
        public string senha { get; set; }

        [DisplayName("Perfil:")]
        [Required(ErrorMessage = "Escolha um perfil.")]
        [Column("profile")]
        public string Profile { get; set; }

        public string ConfirmationToken { get; set; }

        public bool IsEmailConfirmed { get; set; } = false ; 
    }
}