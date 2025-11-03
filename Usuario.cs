using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeuProjetoApi.Models
{
    [Table("USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Required]
        [Column("ID_EMPRESARIAL")]
        public string IdEmpresarial { get; set; } = string.Empty;

        [Required]
        [Column("NOME_USUARIO")]
        public string NomeUsuario { get; set; } = string.Empty;

        [Required]
        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;

        [Column("TELEFONE")]
        public string? Telefone { get; set; }

        [Required]
        [Column("SENHA")]
        public string Senha { get; set; } = string.Empty;

        [Required]
        [Column("CARGO")]
        public string Cargo { get; set; } = string.Empty;
    }
}