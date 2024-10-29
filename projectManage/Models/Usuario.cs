using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;
using projectManage.Data;

namespace projectManage.Models
{
    public class Usuario
    {
        [Key]
        [Column("id_user")]
        public int Id { get; set; }

        [DisplayName("Email:")]
        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [Column("email")]
        public string email { get; set; }
        
        [DisplayName("Senha:")]
        [Required(ErrorMessage = "Digite a senha.")]
        [Column("senha")]
        public string senha { get; set; }

        [DisplayName("Nome:")]
        [Required(ErrorMessage = "Digite o nome.")]
        [Column("nome")]
        public string nome { get; set; }

        [DisplayName("Perfil:")]
        [Required(ErrorMessage = "Escolha um perfil.")]
        [Column("profile")]
        public string profile { get; set; }

        public string ConfirmationToken { get; set; }

        public bool IsEmailConfirmed { get; set; } = false ;

        public virtual ICollection<Projeto> Projetos { get; set; }

        public virtual ICollection<Equipe> Equipes { get; set; }
    }
}