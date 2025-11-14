using System.ComponentModel.DataAnnotations;

namespace SistemaChamadosApi.Models // Namespace correto: Models
{
    public class UsuarioCadastroDto
    {
        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "ID Empresarial deve ter 4 caracteres")]
        public string IdEmpresarial { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Senha deve ter no mínimo 5 caracteres")]
        public string Senha { get; set; } = string.Empty;
    }
}