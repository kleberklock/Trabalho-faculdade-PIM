using System.ComponentModel.DataAnnotations;

namespace SeuProjetoApi.Models
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "ID Empresarial é obrigatório")]
        public string IdEmpresarial { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; } = string.Empty;
    }
}